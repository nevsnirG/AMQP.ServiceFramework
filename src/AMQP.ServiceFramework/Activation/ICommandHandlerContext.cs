using System;

namespace AMQP.ServiceFramework.Activation
{
    public interface ICommandHandlerContext
    {
        Type CommandHandlerType { get; }
    }
}