using System;

namespace AMQP.ServiceFramework.Activation
{
    public class CommandHandlerContext
    {
        public Type CommandHandlerType { get; internal set; }
    }
}