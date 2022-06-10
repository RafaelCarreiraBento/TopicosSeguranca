using EI.SI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servidor
{
    internal class Program
    {
        private const int PORT = 10000;
        public static List<NetworkStream> stream = new List<NetworkStream>();

        static void Main(string[] args)
        {
            //CRIAR UM CONJUNTO IP+PORTO
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT);

            //CRIAR TCP LISTENER
            TcpListener listener = new TcpListener(endPoint);
            listener.Start();

            //Variável com o caminho do Ficheiro LOG
            string pathFicheiro = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\logServidor.txt";

            //Servidor Pronto
            string servidorPronto = DateTime.Now + "  - Servidor Pronto para receber mensagens!";
            File.AppendAllText(pathFicheiro, servidorPronto + Environment.NewLine, Encoding.UTF8);
            Console.WriteLine(servidorPronto);

            //Variável que conta os clientes que já se conectaram
            int clientecounter = 1; 
            
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream streamm = client.GetStream();
                stream.Add(streamm);
                string clienteConectado = DateTime.Now + " - Cliente " + clientecounter + " conectado";
                Console.WriteLine(clienteConectado);
                File.AppendAllText(pathFicheiro, clienteConectado + Environment.NewLine, Encoding.UTF8);
                ClientHandler clientHandler = new ClientHandler(client,clientecounter);
                clientHandler.Handle();
                clientecounter++;
            }
        }

        /// <summary>
        /// Envia a mensagem de volta para o cliente
        /// </summary>
        /// <param name="msg"></param>
        public static void DevolveMensagens (string msg)
        {
            foreach (NetworkStream stream in stream)
            {
                if (stream.CanWrite)
                {
                    ProtocolSI protocolSI = new ProtocolSI();
                    byte[] output = protocolSI.Make(ProtocolSICmdType.DATA, msg);
                    stream.Write(output, 0, output.Length);
                    stream.Flush();
                }
            }
        }
    }

    class ClientHandler
    {
        private TcpClient client;
        private int clientID;

        // CONSTRUTOR
        public ClientHandler(TcpClient client, int clieentID)
        {
            this.client = client;
            this.clientID = clieentID;
        }

        public void Handle()
        {
            Thread thread = new Thread(threadHandler);
            thread.Start();
        }

        private void threadHandler()
        {
            //Variável com o caminho do Ficheiro LOG
            string pathFicheiro = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\logServidor.txt";

            NetworkStream networkStream = this.client.GetStream();
            ProtocolSI protocolSI = new ProtocolSI();

            while (protocolSI.GetCmdType() != ProtocolSICmdType.EOT)
            {
                //LER OS DADOS DO CLIENTE
                int byteRead = networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                //CRIAR A RESPOSTA PARA O CLIENTE
                byte[] ack;
                switch (protocolSI.GetCmdType())
                {
                    case ProtocolSICmdType.DATA:
                        string msg = protocolSI.GetStringFromData();
                        File.AppendAllText(pathFicheiro, DateTime.Now + " - Cliente " +clientID +" - (Mensagem Encriptada)" + msg + Environment.NewLine, Encoding.UTF8);
                        Console.WriteLine(DateTime.Now + " - Cliente " + clientID + " - (Mensagem Encriptada)" + msg);
                        Program.DevolveMensagens(msg);
                        //CRIAR RESPOTA PARA CLIENTE
                        ack = protocolSI.Make(ProtocolSICmdType.ACK);
                        networkStream.Write(ack, 0, ack.Length);
                        break;
                    case ProtocolSICmdType.EOT:     // FIM DO THREAD DO CLIENTE; QUANDO O CLIENTE SAI DO CHAT
                        string fimThread = DateTime.Now + " - Fim do Thread do Cliente do Cliente " + clientID;
                        File.AppendAllText(pathFicheiro, fimThread + Environment.NewLine, Encoding.UTF8);
                        Console.WriteLine(fimThread);
                        ack = protocolSI.Make(ProtocolSICmdType.ACK);
                        networkStream.Write(ack, 0, ack.Length);
                        break;
                }
            }

            //FECHAR AS LIGAÇÕES
            networkStream.Close();
            client.Close();
        }
    }
}
