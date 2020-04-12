namespace AMQP.ServiceFramework.Abstractions
{
    public interface ITopicSubscriptionActivator
    {
        void Activate(ITopicSubscription topicSubscription);
    }
}