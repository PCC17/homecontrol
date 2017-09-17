using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace GateService
{
    static class Program
    {
        private static SerialCommunicator serialCommunicator;
        private static Server server;
        private const string ARDUINO_OPEN_SIGNAL = "GA;";
        public static readonly byte[] VALIDATION_MESSAGE = new byte[] { 1, 0, 1, 0, 1, 11, 0 };


        static void Main(string[] args)
        {
            Console.Write("Server-Port: ");
            int serverPort = Convert.ToInt32(Console.ReadLine());
            Console.Write("Serial-Port: ");
            string serialPort = Console.ReadLine();
            Console.Write("Baudrate: ");
            int baudrate = Convert.ToInt32(Console.ReadLine());

            serialCommunicator = new SerialCommunicator(serialPort, baudrate);
            serialCommunicator.Open();

            server = new Server(serverPort);
            server.serverReceivedHandler += new ServerReceivedHandler(ServerReceivedMsg);
            server.Start();

            Console.ReadLine();
        }

        static void ServerReceivedMsg(Object o, ServerReceivedEventArgs args)
        {
            TcpClient c = (TcpClient)o;
            if (args.Msg[3] == VALIDATION_MESSAGE[3])
            {
                serialCommunicator.Write(ARDUINO_OPEN_SIGNAL);
                Console.WriteLine("Client opened");
            }
        }
    }


    class SerialCommunicator
    {
        private SerialPort serialPort;

        public SerialCommunicator(string port, int baudRate)
        {
            serialPort = new SerialPort(port, baudRate);
        }

        public void Write(byte[] bytes, int offset, int count)
        {
            serialPort.Write(bytes, offset, count);
        }

        public void Write(string str)
        {
            serialPort.Write(str);
        }

        public void Open()
        {
            serialPort.Open();
        }

        public void Close()
        {
            serialPort.Close();
        }
    }

    class Server
    {
        public readonly int Port;
        public event ServerReceivedHandler serverReceivedHandler;
        private bool isAlive = false;
        private Thread listenThread;
        private TcpListener tcpListener;

        public Server(int port)
        {
            Port = port;
            tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, Port));
            listenThread = new Thread(new ThreadStart(Listen));
        }

        public void Start()
        {
            if (!isAlive)
            {
                isAlive = true;

                tcpListener.Start();
                listenThread.Start();
            }
        }

        public void Stop()
        {
            if (isAlive)
            {
                isAlive = false;

                tcpListener.Stop();
                listenThread.Abort();
            }
        }

        private void Listen()
        {
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Client conneted");
                Thread clientCommThread = new Thread(new ParameterizedThreadStart(HandleClientCommunication));
                clientCommThread.Start(client);
            }
        }

        private void HandleClientCommunication(Object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream nStream = tcpClient.GetStream();

            while (true)
            {
                if (nStream.DataAvailable)
                {
                    byte[] buffer = new byte[Program.VALIDATION_MESSAGE.Length];
                    int amount = nStream.Read(buffer, 0, Program.VALIDATION_MESSAGE.Length);

                    serverReceivedHandler((Object)tcpClient, new ServerReceivedEventArgs(buffer));
                }
                Thread.Sleep(50);
            }
        }


        public bool IsAlive
        {
            get { return isAlive; }
        }

    }
    public delegate void ServerReceivedHandler(Object o, ServerReceivedEventArgs args);

    public class ServerReceivedEventArgs : EventArgs
    {
        public readonly byte[] Msg;

        public ServerReceivedEventArgs(byte[] s)
        {
            Msg = s;
        }
    }
}

