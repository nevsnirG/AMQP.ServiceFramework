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

        public object Create(CommandHandlerContext context)
        {
            return ActivatorUtilities.CreateInstance(_serviceProvider, context.CommandHandlerType);
        }

        public void Release(CommandHandlerContext context, object commandHandler)
        {
        }
    }
}