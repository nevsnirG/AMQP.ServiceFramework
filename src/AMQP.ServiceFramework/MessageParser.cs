using System;

namespace AMQP.ServiceFramework
{
    public abstract class MessageParser
    {
        public abstract Type Type { get; }

        public abstract object Parse(byte[] body);

        public virtual bool CanParse(Type type)
        {
            return type == Type;
        }
    }
}