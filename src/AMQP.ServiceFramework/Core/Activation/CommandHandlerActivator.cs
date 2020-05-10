using AMQP.ServiceFramework.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AMQP.ServiceFramework.Core.Activation
{
    internal sealed class CommandHandlerActivator : ICommandHandlerActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlerActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object Create(ICommandHandlerContext commandHandlerContext)
        {
            if (commandHandlerContext is null)
                throw new ArgumentNullException(nameof(commandHandlerContext));
            if (commandHandlerContext.DeclaringType is null)
                throw new ArgumentNullException(nameof(commandHandlerContext.DeclaringType));

            return ActivatorUtilities.CreateInstance(_serviceProvider, commandHandlerContext.DeclaringType);
        }

        public void Release(ICommandHandlerContext context, object commandHandler)
        {
            if (!(commandHandler is null) && commandHandler is IDisposable disposable)
                disposable.Dispose();
        }
    }
}