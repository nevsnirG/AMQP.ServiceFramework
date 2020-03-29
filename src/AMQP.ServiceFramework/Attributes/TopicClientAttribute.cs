using System;

namespace AMQP.ServiceFramework.Attributes
{
    public sealed class TopicClientAttribute : Attribute
    {
        public string Queue { get; }

        public TopicClientAttribute() : base() { }

        public TopicClientAttribute(string queue) : this()
        {
            Queue = queue;
        }
    }
}