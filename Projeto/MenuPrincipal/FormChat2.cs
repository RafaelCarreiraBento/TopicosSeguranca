using EI.SI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private RSACryptoServiceProvider rsa;
        private Thread receiver;
        
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
            //ENVIAR A MENSAGEM DO CLIENTE PARA O SERVIDOR 
            string msg = username + ": " + tbmensagem.Text;
            tbmensagem.Clear();
           // string mensagemCifrada = encryptMessage(msg);

            byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msg);
            //ProtocolSICmdType - INTERPRETA O TIPO DE MENSAGEM RECEBIDO
            //protocolSI.Make - CRIAR A MENSAGEM DO TIPO ESPECÍFICO

            //ENVIAR A MENSAGEM PELA LIGAÇÃO
            networkStream.Write(packet, 0, packet.Length);

            while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
            {
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            }
        }

        /// <summary>
        /// Desencripta a menssagen
        /// </summary>
        /// <param name="mensagem"</param>
        /// <returns> Mensagem Cifrada</returns>
        public void decryptMessage(string mensagem)
        {
            byte[] dados = Convert.FromBase64String(mensagem);

            //DECIFRAR DADOS ATRAVÉS DO RSA
            byte[] dadosEnc = rsa.Decrypt(dados, true);
            lbChat.Text += Encoding.UTF8.GetString(dadosEnc);
        }

        /// <summary>
        /// Encripta a mensagem
        /// </summary>
        /// <param name="mensagem"></param>
        /// <returns>Mensagem Cifrada</returns>
        private string encryptMessage(string mensagem)
        {
            //OBTER CHAVE SIMÉTRIRCA PARA CIFRAR
            byte[] dados = Encoding.UTF8.GetBytes(mensagem);

            //CIFRAR DADOS UTILIZANDO RSA
            byte[] dadosEnc = rsa.Encrypt(dados, true);

            string mensagemCifrada = Convert.ToBase64String(dadosEnc);
            return mensagemCifrada;
        }

        private void FormChat2_Load(object sender, EventArgs e)
        {
            byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, username);
            networkStream.Write(packet, 0, packet.Length);

            while(protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
            {
                networkStream.Read(protocolSI.Buffer,0,protocolSI.Buffer.Length);
            }

            Thread thread = new Thread(RecebeMensagem);
            receiver = thread;
            thread.Start();
        }

        private void RecebeMensagem()
        {
            ProtocolSI MessageReturn = new ProtocolSI();
            while (true && networkStream.CanRead)
            {
                while (networkStream.DataAvailable)
                {
                    int lidos = networkStream.Read(MessageReturn.Buffer,0,MessageReturn.Buffer.Length);
                    if ( MessageReturn.GetCmdType() == ProtocolSICmdType.DATA)
                    {
                        string msg = MessageReturn.GetStringFromData();
                        lbChat.BeginInvoke(new MethodInvoker(delegate { lbChat.Text += "\n"+msg; }));
                    }
                }
            }
        }
    }
}
