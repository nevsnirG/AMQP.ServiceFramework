using AMQP.ServiceFramework.Abstractions;
using System.Collections.Generic;

namespace AMQP.ServiceFramework.Core.Extensions
{
    internal static class ICommandHandlerRegistryExtension
    {
        public static void AddRange(this ICommandHandlerRegistry commandHandlerRegistry, IEnumerable<ICommandHandlerContext> commandHandlerContexts)
        {
            if (commandHandlerRegistry is null)
                throw new System.ArgumentNullException(nameof(commandHandlerRegistry));
            if (commandHandlerContexts is null)
                throw new System.ArgumentNullException(nameof(commandHandlerContexts));

            foreach (var commandHandlerContext in commandHandlerContexts)
                commandHandlerRegistry.Add(commandHandlerContext);
        }
    }
}