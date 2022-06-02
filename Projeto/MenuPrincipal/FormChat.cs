using EI.SI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

        public FormChat()
        {
            InitializeComponent();

            username = FormLogin.nomeDeUtilizador;

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
    }
}
