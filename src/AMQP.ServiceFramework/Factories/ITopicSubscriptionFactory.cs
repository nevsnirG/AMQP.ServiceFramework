using AMQP.ServiceFramework.Activation;

namespace AMQP.ServiceFramework.Factories
{
    public interface ITopicSubscriptionFactory
    {
        ITopicSubscription CreateSubscription(ICommandHandlerContext commandHandlerContext, object instance);
    }
}