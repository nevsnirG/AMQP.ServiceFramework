using System;

namespace AMQP.ServiceFramework.Activation
{
    internal sealed class CommandHandlerContext : ICommandHandlerContext
    {
        public Type CommandHandlerType { get; internal set; }
    }
}