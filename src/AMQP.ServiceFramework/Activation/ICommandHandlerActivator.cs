namespace AMQP.ServiceFramework.Activation
{
    public interface ICommandHandlerActivator
    {
        object Create(ICommandHandlerContext context);

        void Release(ICommandHandlerContext context, object commandHandler);
    }
}