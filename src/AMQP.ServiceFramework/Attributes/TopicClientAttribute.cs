using System;

namespace AMQP.ServiceFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TopicClientAttribute : Attribute
    {
        public string Queue { get; } = string.Empty;

        public TopicClientAttribute() : base() { }

        public TopicClientAttribute(string queue) : this()
        {
            Queue = queue;
        }
    }
}