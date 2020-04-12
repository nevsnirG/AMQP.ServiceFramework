using System;

namespace AMQP.ServiceFramework.Abstractions
{
    public interface ITopicSubscriptionRegistry : IDisposable
    {
        void Add(ITopicSubscription topicSubscription);
    }
}