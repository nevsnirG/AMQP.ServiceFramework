using AMQP.ServiceFramework.Abstractions;
using System.Collections.Generic;
using System.Reflection;

namespace AMQP.ServiceFramework.Core.Extensions
{
    internal static class ICommandHandlerContextFactoryExtension
    {
        public static IEnumerable<ICommandHandlerContext> Create(this ICommandHandlerContextFactory commandHandlerContextFactory, IEnumerable<MethodInfo> methodInfos)
        {
            if (commandHandlerContextFactory is null)
                throw new System.ArgumentNullException(nameof(commandHandlerContextFactory));
            if (methodInfos is null)
                throw new System.ArgumentNullException(nameof(methodInfos));

            foreach (var methodInfo in methodInfos)
                yield return commandHandlerContextFactory.Create(methodInfo);
        }
    }
}