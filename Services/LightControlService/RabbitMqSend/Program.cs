using System;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMqSend
{
    class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "iotservice", durable: false, exclusive: false, autoDelete: false, arguments: null);

                while (true)
                {
                    string message = "10.0.0.255:" + Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: "iotservice", basicProperties: null, body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}