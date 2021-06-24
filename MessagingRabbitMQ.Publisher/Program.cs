using RabbitMQ.Client;
using System;
using System.Text;

namespace MessagingRabbitMQ.Publisher
{
    class Program
    {
        private const string QUEUE = "my-queue";
        private const string EXCHANGE = "my-exchange";

        static void Main(string[] args)
        {

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: QUEUE,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);


            while (true)
            {
                Console.WriteLine("Write your message: ");

                var message = Console.ReadLine();

                string messageToPublish = $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - " + $"{message}";

                var body = Encoding.UTF8.GetBytes(messageToPublish);

                channel.BasicPublish(exchange: "",
                                     routingKey: QUEUE,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
