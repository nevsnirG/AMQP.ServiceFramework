using AMQP.Plugin.Abstractions;
using System;

namespace AMQP.ServiceFramework.Abstractions
{
    public interface ITopicSubscription : IDisposable
    {
        void EnsureInitialization(IConnection connection, IMessageParserRegistry messageParserRegistry);
    }
}