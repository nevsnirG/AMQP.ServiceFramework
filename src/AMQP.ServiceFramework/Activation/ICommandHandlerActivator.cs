namespace AMQP.ServiceFramework.Activation
{
    public interface ICommandHandlerActivator
    {
        object Create(CommandHandlerContext context);

        void Release(CommandHandlerContext context, object commandHandler);
    }
}