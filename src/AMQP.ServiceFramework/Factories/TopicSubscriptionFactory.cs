using AMQP.ServiceFramework.Activation;

namespace AMQP.ServiceFramework.Factories
{
    internal sealed class TopicSubscriptionFactory : ITopicSubscriptionFactory
    {
        public ITopicSubscription CreateSubscription(ICommandHandlerContext commandHandlerContext, object instance)
        {
            return new TopicSubscription(commandHandlerContext, instance);
        }
    }
}