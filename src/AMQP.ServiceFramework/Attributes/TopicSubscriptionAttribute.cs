using System;

namespace AMQP.ServiceFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class TopicSubscriptionAttribute : Attribute
    {
        public string Topic { get; }
        public string Queue { get; }
        public Type ParserType { get; }

        public TopicSubscriptionAttribute(string topic, Type parserType) : base()
        {
            topic = topic.Trim();
            if (string.IsNullOrEmpty(topic))
                throw new ArgumentException(nameof(topic));

            Topic = topic;

            if (parserType is null)
                throw new ArgumentNullException(nameof(parserType));
            if (!typeof(MessageParser).IsAssignableFrom(parserType))
                throw new ArgumentException($"The specified parser type is not a subclass of {typeof(MessageParser).Name}.", nameof(parserType));

            ParserType = parserType;
        }

        public TopicSubscriptionAttribute(string topic, string queue, Type parserType) : this(topic, parserType)
        {
            Queue = queue.Trim();
        }
    }
}