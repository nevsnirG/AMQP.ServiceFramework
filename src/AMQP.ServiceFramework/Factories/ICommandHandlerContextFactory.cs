using AMQP.ServiceFramework.Activation;
using System.Reflection;

namespace AMQP.ServiceFramework.Factories
{
    public interface ICommandHandlerContextFactory
    {
        ICommandHandlerContext Create(MethodInfo methodInfo);
    }
}