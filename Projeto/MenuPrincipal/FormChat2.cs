using EI.SI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuPrincipal
{
    public partial class FormChat2 : Form
    {
        public string username;
        private const int PORT = 10000;
        NetworkStream networkStream;
        ProtocolSI protocolSI;
        TcpClient client;
        
        public FormChat2()
        {
            InitializeComponent();

            //Recebe o username que deu Login do outro form e coloca-o na label
            username = FormLogin2.nomeDeUtilizador;
            lbUser.Text = username;

            //CRIAR UM CONJUNTO IP+PORTO
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, PORT);

            //CRIAR O CLIENTE TCP
            client = new TcpClient();

            //CRIAR A LIGAÇÃO
            client.Connect(endPoint);

            //OBTER A LIGAÇÃO DO SERVIDOR
            networkStream = client.GetStream();
            protocolSI = new ProtocolSI();
        }

        private void btLogout_Click(object sender, EventArgs e)
        {
            //Diz ao servidor que o cliente vai se desconectar
            byte[] eot = protocolSI.Make(ProtocolSICmdType.EOT);
            networkStream.Write(eot, 0, eot.Length);
            networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

            //Fecha todas as ligações e o form
            networkStream.Close();
            client.Close();
            this.Close();
        }

        private void btEnviar_Click(object sender, EventArgs e)
        {
            //Se estiver alguma mensagem escrita
            if (tbmensagem.Text != "")
            {
                //Envia a mensagem para o servidor com a hora
                string msg = DateTime.Now.ToString("HH:mm") + " - " + username + ": " + tbmensagem.Text;
                //Limpa a textBox da mensagem
                tbmensagem.Clear();
                //Cifra a mensagem
                string mensagemCifrada = cifrarTexto(msg);

                byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, mensagemCifrada);
                //ProtocolSICmdType - INTERPRETA O TIPO DE MENSAGEM RECEBIDO
                //protocolSI.Make - CRIAR A MENSAGEM DO TIPO ESPECÍFICO

                //ENVIAR A MENSAGEM PELA LIGAÇÃO
                networkStream.Write(packet, 0, packet.Length);

                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                }
            }
            else MessageBox.Show("Escreva uma Mensagem.");
        }

        /// <summary>
        /// Cifrar o texto
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>Retorna a mensagem Cifrada</returns>
        private string cifrarTexto(string msg)
        {
            //VARIÁVEL PARA GUARDAR O TEXTO
            byte[] txtDecifrado = Encoding.UTF8.GetBytes(msg);

            //VARIÁVEL PARA GUARDAR O TEXTO CIFRADO
            byte[] txtCifrado;

            //RESERVAR ESPAÇO NA MEMÓRIA PARA COLOCAR O TEXTO E CIFRÁ-LO
            MemoryStream ms = new MemoryStream();

            //INICIALIZAR O SISTEMA DE CIFRAGEM (WRITE)
            CryptoStream cs = new CryptoStream(ms, FormMenu.aes.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(txtDecifrado, 0, txtDecifrado.Length);
            cs.Close();

            //GUARDAR OS DADOS CIFRADOS QUE ESTÃO NA MEMÓRIA
            txtCifrado = ms.ToArray();

            //CONVERTER OS BYTES PARA UMA BASE64(TEXTO)
            string txtDecifradoB64 = Convert.ToBase64String(txtCifrado);

            return txtDecifradoB64;
        }

        /// <summary>
        /// Decifra o texto
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>Retorna a mensagem decifrada</returns>
        private string decifrarTexto(string msg)
        {
            //VARIÁVEL PARA GUARDAR DO TEXTO CIFRADO EM BYTES
            byte[] txtCifrado = Convert.FromBase64String(msg);

            MemoryStream ms = new MemoryStream(txtCifrado);

            CryptoStream cs = new CryptoStream(ms, FormMenu.aes.CreateDecryptor(), CryptoStreamMode.Read);

            //VARIÁVEL PARA GUARDAR O TEXTO DECIFRADO
            byte[] txtDecifrado = new byte[ms.Length];

            //DECIFRA OS DADOS
            int byteslidos = cs.Read(txtDecifrado, 0, txtDecifrado.Length);
            cs.Close();

            //CONVERTER PARA TEXTO
            string textoDecifrado = Encoding.UTF8.GetString(txtDecifrado, 0, byteslidos);

            return textoDecifrado;
        }

        private void FormChat2_Load(object sender, EventArgs e)
        {
            //Cria mensagem a dizer que o utilizador entrou no chat
            string userJoined = DateTime.Now.ToString("HH:mm")+ " - " + username + " entrou no chat!";
            //Cifra mensagem
            string userJoinedCifrado = cifrarTexto(userJoined);
            //Envia mensagem para o Servidor
            byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, userJoinedCifrado);
            networkStream.Write(packet, 0, packet.Length);

            while(protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
            {
                networkStream.Read(protocolSI.Buffer,0,protocolSI.Buffer.Length);
            }
            //Cria o thread
            Thread thread = new Thread(RecebeMensagem);
            thread.Start();
        }

        private void RecebeMensagem()
        {
            ProtocolSI MessageReturn = new ProtocolSI();
            //Está sempre à escuta até o utilizador dar logout
            while (true && networkStream.CanRead)
            {
                //Enquanto houver dados para receber
                while (networkStream.DataAvailable)
                {
                    //Recebe os dados
                    int lidos = networkStream.Read(MessageReturn.Buffer,0,MessageReturn.Buffer.Length);
                    //Se houver mais dados
                    if ( MessageReturn.GetCmdType() == ProtocolSICmdType.DATA)
                    {
                        string msg = MessageReturn.GetStringFromData();
                        //Decifra a mensagem
                        msg = decifrarTexto(msg);
                        //Escreve a mensagem na label
                        lbChat.BeginInvoke(new MethodInvoker(delegate { lbChat.Text += "\r\n"+msg; }));
                    }
                }
            }
        }
    }
}
