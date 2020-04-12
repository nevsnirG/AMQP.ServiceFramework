using System;
using System.Reflection;

namespace AMQP.ServiceFramework.Abstractions
{
    public interface ICommandHandlerContext
    {
        string Queue { get; }
        string Topic { get; }
        MethodInfo TargetMethod { get; }
        Type DeclaringType { get; }
        Type ParameterType { get; }
        Type ParserType { get; internal set; }
    }
}