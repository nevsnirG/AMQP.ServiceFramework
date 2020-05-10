using AMQP.Plugin.Abstractions;
using AMQP.Plugin.RabbitMQ;
using AMQP.ServiceFramework;
using AMQP.ServiceFramework.Attributes;
using System;

namespace Sandbox
{
    public class Program : ServiceBuilder
    {
        public Program(IConnection connection) : base(connection)
        {
        }

        static void Main()
        {
            var connectionFactory = new RabbitMQConnectionFactory("amqp://guest:guest@localhost:5672/");
            using var connection = connectionFactory.CreateConnection("Instagram");
            using var program = new Program(connection);
            program.EnsureInitialization();

            Console.ReadLine();
        }
    }

    [TopicClient]
    internal sealed class CommandHandler : ISubscription<string>
    {
        [TopicSubscription("test_topic", typeof(string))]
        public void Handle(string input)
        {
            Console.WriteLine("Input: {0}", input);
        }
    }
}