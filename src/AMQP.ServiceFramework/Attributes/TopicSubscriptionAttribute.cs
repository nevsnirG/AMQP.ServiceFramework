using System;

namespace AMQP.ServiceFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class TopicSubscriptionAttribute : Attribute
    {
        public string Topic { get; }
        public Type ParserType { get; }

        public TopicSubscriptionAttribute(string topic, Type parserType) : base()
        {
            if (string.IsNullOrEmpty(topic))
                throw new ArgumentException(nameof(topic));

            Topic = topic;
            ParserType = parserType ?? throw new ArgumentNullException(nameof(parserType));
        }
    }
}