using System;

namespace AMQP.ServiceFramework.Abstractions
{
    public abstract class MessageParser
    {
        public Type Type { get; }

        public abstract object Parse(byte[] body);

        public virtual bool CanParse(Type type)
        {
            return type == Type;
        }
    }
}