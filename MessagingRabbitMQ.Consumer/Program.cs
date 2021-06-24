using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MessagingRabbitMQ.Consumer
{
    class Program
    {
        private const string QUEUE = "my-queue";

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

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var byteArray = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(byteArray);

                Console.WriteLine(Environment.NewLine + "[MESSAGE RECEIVED] " + message);

                channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            channel.BasicConsume(QUEUE, false, consumer);

            Console.WriteLine("Waiting new messages...Press some key to exit");
            Console.ReadKey();
        }
    }
}
