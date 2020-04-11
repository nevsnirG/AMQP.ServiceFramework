using AMQP.ServiceFramework.Registries;

namespace AMQP.ServiceFramework.Activation
{
    public interface ITopicSubscriptionActivator
    {
        void Activate(ITopicSubscription topicSubscription);
    }
}