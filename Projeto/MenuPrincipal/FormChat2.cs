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
        Thread receiver;
        private RSACryptoServiceProvider rsa;
        
        public FormChat2()
        {
            InitializeComponent();

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

            rsa = new RSACryptoServiceProvider();

            //CRIAR CHAVE PRIVADA/PÚBLICA
            string chave = rsa.ToXmlString(true);

        }

        private void btLogout_Click(object sender, EventArgs e)
        {
            byte[] eot = protocolSI.Make(ProtocolSICmdType.EOT);
            networkStream.Write(eot, 0, eot.Length);
            networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

            //FECHAR TODAS AS LIGAÇÕES
            networkStream.Close();
            client.Close();
            this.Close();
        }

        private void btEnviar_Click(object sender, EventArgs e)
        {
            if (tbmensagem.Text != "")
            {
                //ENVIAR A MENSAGEM DO CLIENTE PARA O SERVIDOR 
                string msg = DateTime.Now.ToString("HH:mm") + " - " + username + ": " + tbmensagem.Text;
                tbmensagem.Clear();
                //string mensagemCifrada = encryptMessage(msg);
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
            int byteslidos = 0;

            //DECIFRA OS DADOS
            byteslidos = cs.Read(txtDecifrado, 0, txtDecifrado.Length);
            cs.Close();

            //CONVERTER PARA TEXTO
            string textoDecifrado = Encoding.UTF8.GetString(txtDecifrado, 0, byteslidos);

            return textoDecifrado;
        }

        private void FormChat2_Load(object sender, EventArgs e)
        {
            string userJoined = DateTime.Now.ToString("HH:mm")+ " - " + username + " entrou no chat!";
            string userJoinedCifrado = cifrarTexto(userJoined);
            byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, userJoinedCifrado);
            networkStream.Write(packet, 0, packet.Length);

            while(protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
            {
                networkStream.Read(protocolSI.Buffer,0,protocolSI.Buffer.Length);
            }

            Thread thread = new Thread(RecebeMensagem);
            receiver = thread;
            receiver.Start();
        }

        private void RecebeMensagem()
        {
            ProtocolSI MessageReturn = new ProtocolSI();
            //Está sempre à escuta até o utilizador dar logout
            while (true && networkStream.CanRead)
            {
                //Enquanto houver dados
                while (networkStream.DataAvailable)
                {
                    int lidos = networkStream.Read(MessageReturn.Buffer,0,MessageReturn.Buffer.Length);
                    if ( MessageReturn.GetCmdType() == ProtocolSICmdType.DATA)
                    {
                        string msg = MessageReturn.GetStringFromData();
                        msg = decifrarTexto(msg);
                        lbChat.BeginInvoke(new MethodInvoker(delegate { lbChat.Text += "\n"+msg; }));
                    }
                }
            }
        }
    }
}
