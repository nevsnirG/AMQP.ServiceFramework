using System;

namespace AMQP.ServiceFramework.Registries
{
    public interface ITopicSubscriptionRegistry : IDisposable
    {
        void Add(ITopicSubscription topicSubscription);
    }
}