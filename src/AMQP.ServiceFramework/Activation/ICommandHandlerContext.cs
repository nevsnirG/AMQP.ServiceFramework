using System;
using System.Reflection;

namespace AMQP.ServiceFramework.Activation
{
    public interface ICommandHandlerContext
    {
        string Queue { get; }
        string Topic { get; }
        MethodInfo TargetMethod { get; }
        Type DeclaringType { get; }
        Type ParameterType { get; } //TODO - Convert to IEvent type or something.
    }
}