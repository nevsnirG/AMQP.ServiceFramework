using System;

namespace AMQP.ServiceFramework.Registries
{
    public interface IMessageParserRegistry
    {
        MessageParser Retrieve(Type type);
    }
}