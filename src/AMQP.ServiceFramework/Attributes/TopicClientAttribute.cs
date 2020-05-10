using System;

namespace AMQP.ServiceFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TopicClientAttribute : Attribute
    {
        public TopicClientAttribute() : base() { }
    }
}