using System;

namespace AMQP.ServiceFramework.Abstractions
{
    public interface IMessageParserRegistry
    {
        MessageParser Retrieve(Type type);
    }
}