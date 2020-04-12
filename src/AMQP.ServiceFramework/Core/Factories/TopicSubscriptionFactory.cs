using AMQP.ServiceFramework.Abstractions;
using AMQP.ServiceFramework.Core.Registries;

namespace AMQP.ServiceFramework.Core.Factories
{
    internal sealed class TopicSubscriptionFactory : ITopicSubscriptionFactory
    {
        public ITopicSubscription CreateSubscription(ICommandHandlerContext commandHandlerContext, object instance)
        {
            return new TopicSubscription(commandHandlerContext, instance);
        }
    }
}