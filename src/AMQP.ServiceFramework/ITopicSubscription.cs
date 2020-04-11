using AMQP.Plugin.Abstractions;
using AMQP.ServiceFramework.Registries;
using System;

namespace AMQP.ServiceFramework
{
    public interface ITopicSubscription : IDisposable
    {
        void EnsureInitialization(IConnection connection, IMessageParserRegistry messageParserRegistry);
    }
}