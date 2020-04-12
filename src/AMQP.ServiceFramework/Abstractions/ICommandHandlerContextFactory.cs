using System.Reflection;

namespace AMQP.ServiceFramework.Abstractions
{
    public interface ICommandHandlerContextFactory
    {
        ICommandHandlerContext Create(MethodInfo methodInfo);
    }
}