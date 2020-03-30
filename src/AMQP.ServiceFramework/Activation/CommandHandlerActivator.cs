using Microsoft.Extensions.DependencyInjection;
using System;

namespace AMQP.ServiceFramework.Activation
{
    internal sealed class CommandHandlerActivator : ICommandHandlerActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlerActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object Create(ICommandHandlerContext context)
        {
            return ActivatorUtilities.CreateInstance(_serviceProvider, context.DeclaringType);
        }

        public void Release(ICommandHandlerContext context, object commandHandler)
        {
            //TODO - Implement Release method.
        }
    }
}