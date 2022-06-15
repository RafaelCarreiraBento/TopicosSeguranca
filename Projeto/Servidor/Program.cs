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
                //Adiciona a stream do cliente à lista de NetworkStreams
                stream.Add(streamm);
                //Cria string a dizer que um novo cliente se conectou ao servidor.
                string clienteConectado = DateTime.Now + " - Cliente " + clientecounter + " conectado";
                Console.WriteLine(clienteConectado);
                File.AppendAllText(pathFicheiro, clienteConectado + Environment.NewLine, Encoding.UTF8);
                //Cria um  novo Cliente
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

        //Construtor
        public ClientHandler(TcpClient client, int clieentID)
        {
            this.client = client;
            this.clientID = clieentID;
        }

        public void Handle()
        {
            //Cria o thread do cliente
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
                    case ProtocolSICmdType.DATA:    // Quando recebe dados
                        //Mensagem que foi recebida
                        string msg = protocolSI.GetStringFromData();
                        File.AppendAllText(pathFicheiro, DateTime.Now + " - Mensagem recbida do cliente " +clientID +" - (Mensagem Encriptada)" + msg + Environment.NewLine, Encoding.UTF8);
                        Console.WriteLine(DateTime.Now + " - Mensagem recbida do cliente " + clientID + " - (Mensagem Encriptada)" + msg);
                        //Cria string a dizer que a mensagem do cliente vai ser enviada
                        string mensagemASerEnviada = DateTime.Now + " - Mensagem a ser Enviada para todos os clientes...";
                        Console.WriteLine(mensagemASerEnviada);
                        File.AppendAllText(pathFicheiro,mensagemASerEnviada + Environment.NewLine, Encoding.UTF8);
                        //Vai devolver as mensagens
                        Program.DevolveMensagens(msg);
                        ack = protocolSI.Make(ProtocolSICmdType.ACK);
                        networkStream.Write(ack, 0, ack.Length);
                        //Cria string a dizer que a mensagem do cliente já foi enviado para todos os clientes
                        string mensagemEnviada = DateTime.Now + " - Mensagem enviada com sucesso para todos os clientes!";
                        Console.WriteLine(mensagemEnviada);
                        File.AppendAllText(pathFicheiro, mensagemEnviada + Environment.NewLine, Encoding.UTF8);
                        break;
                    case ProtocolSICmdType.EOT:     // QUando é o fim do thead, quando o cliente sai do chat
                        //Cria string a dizer que o thread terminou
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
