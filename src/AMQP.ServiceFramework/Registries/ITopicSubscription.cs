using AMQP.Plugin.Abstractions;
using System;

namespace AMQP.ServiceFramework.Registries
{
    public interface ITopicSubscription : IDisposable
    {
        void EnsureInitialization(IConnection connection, IMessageParserRegistry messageParserRegistry);
    }
}