namespace AMQP.ServiceFramework.Abstractions
{
    public interface ITopicSubscriptionFactory
    {
        ITopicSubscription CreateSubscription(ICommandHandlerContext commandHandlerContext, object instance);
    }
}