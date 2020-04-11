using AMQP.ServiceFramework.Activation;
using AMQP.ServiceFramework.Registries;

namespace AMQP.ServiceFramework.Factories
{
    public interface ITopicSubscriptionFactory
    {
        ITopicSubscription CreateSubscription(ICommandHandlerContext commandHandlerContext, object instance);
    }
}