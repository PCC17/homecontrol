using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace IotCommunictionService
{
    class Program
    {
        const char seperator = ':';
        static Socket socket;
        static int iotDevicesPort = 33123;

        public static void Main()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "iotservice", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Received;
                channel.BasicConsume(queue: "iotservice", autoAck: true, consumer: consumer);
                Console.ReadLine();
            }
        }

        private static void Received(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            ProcessMessage(message);
            Console.WriteLine("Received {0}", message);
        }

        private static void ProcessMessage(string msg)
        {
            string[] split = msg.Split(seperator);
            if(split.Length == 2)
                SendUdp(split[0], iotDevicesPort, split[1]);
        }

        static void SendUdp(string dstIp, int dstPort, string data)
        {
            IPAddress serverAddr = IPAddress.Parse(dstIp);
            IPEndPoint endPoint = new IPEndPoint(serverAddr, dstPort);
            byte[] send_buffer = Encoding.ASCII.GetBytes(data);
            socket.SendTo(send_buffer, endPoint);
        }
    }
}