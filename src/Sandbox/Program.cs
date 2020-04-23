using AMQP.Plugin.Abstractions;
using AMQP.Plugin.RabbitMQ;
using AMQP.ServiceFramework;
using AMQP.ServiceFramework.Attributes;
using System;
using System.Text;

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

            using var publisher = connection.CreatePublisher("test_topic");
            //using var publisher2 = connection.CreatePublisher("test_topic2");
            using var publisher3 = connection.CreatePublisher("test_topic3");

            var body = Encoding.ASCII.GetBytes("1");
            publisher.SendMessage(body);
            body = Encoding.UTF8.GetBytes("3");
            publisher3.SendMessage(body);

            Console.ReadLine();
        }
    }

    [TopicClient]
    internal sealed class CommandHandler : ISubscription<TestValue1>, ISubscription<TestValue2>, ISubscription<TestValue3>
    {
        [TopicSubscription("test_topic", typeof(TestValue1Parser))]
        public void Handle(TestValue1 input)
        {
            Console.WriteLine("TestValue1: {0}", input.Value);
        }

        [TopicSubscription("test_topic3", typeof(TestValue3Parser))]
        public void Handle(TestValue3 input)
        {
            Console.WriteLine("TestValue3: {0}", input.Value);
        }

        [TopicSubscription("test_topic2", typeof(TestValue2Parser))]
        public void Handle(TestValue2 input)
        {
            Console.WriteLine("TestValue2: {0}", input.Value);
        }
    }

    internal sealed class TestValue1Parser : MessageParser
    {
        public override Type Type => typeof(TestValue1);

        public override object Parse(byte[] body)
        {
            return new TestValue1(Encoding.ASCII.GetString(body));
        }
    }

    internal sealed class TestValue2Parser : MessageParser
    {
        public override Type Type => typeof(TestValue2);

        public override object Parse(byte[] body)
        {
            return new TestValue2(Encoding.UTF8.GetString(body));
        }
    }

    internal sealed class TestValue3Parser : MessageParser
    {
        public override Type Type => typeof(TestValue3);

        public override object Parse(byte[] body)
        {
            return new TestValue3(Encoding.UTF8.GetString(body));
        }
    }

    internal sealed class TestValue1
    {
        public string Value { get; set; }

        public TestValue1(string value)
        {
            Value = value;
        }
    }

    internal sealed class TestValue2
    {
        public string Value { get; set; }

        public TestValue2(string value)
        {
            Value = value;
        }
    }

    internal sealed class TestValue3
    {
        public string Value { get; set; }

        public TestValue3(string value)
        {
            Value = value;
        }
    }
}