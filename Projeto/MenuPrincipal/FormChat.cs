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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuPrincipal
{
    public partial class FormChat : Form
    {
        public string username;
        private const int PORT = 10000;
        NetworkStream networkStream;
        ProtocolSI protocolSI;
        TcpClient client;
        private RSACryptoServiceProvider rsa;

        public FormChat()
        {
            InitializeComponent();

            username = FormLogin.nomeDeUtilizador;
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

            string mensagemCifrada = encryptMessage(msg);

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

        /// <summary>
        /// Recebe as mensagens enviadas do servidor
        /// </summary>
        private string receberMensagem()
        {
            // VARIAVEL QUE RECEBE OS DADOS DO SERVIDOR
            string textorecebido = "";
            // CRIA UMA MENSAGEM DO TIPO USER_OPTION_1
            byte[] opt1 = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1);
            // ENVIA O PEDIDO PARA O SERVIDOR (WRITE)
            networkStream.Write(opt1, 0, opt1.Length);
            // ENQUANTO HOUVER COISAS PARA RECEBER
            while (true)
            {
                // LÊ A RESPOSTA QUE CHEGOU (READ)
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                // SE FOR O FIM DA RESPOSTA SAI FORA
                if (protocolSI.GetCmdType() == ProtocolSICmdType.EOF)
                {
                    break;
                }
                // SENÃO, E SE FOREM DADOS ESCREVE PARA A STRING
                else if (protocolSI.GetCmdType() == ProtocolSICmdType.DATA)
                {
                    textorecebido += protocolSI.GetStringFromData();
                }
            }
            // ATUALIZA O TEXTO DO CHAT
            return textorecebido;
        }

        /// <summary>
        /// Desencripta a menssagen
        /// </summary>
        /// <param name="mensagem"</param>
        /// <returns> Mensagem Cifrada</returns>
        private void decryptMessage(string mensagem)
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
    }
}
