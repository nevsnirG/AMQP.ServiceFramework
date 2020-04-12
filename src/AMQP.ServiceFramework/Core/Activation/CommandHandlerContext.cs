using AMQP.ServiceFramework.Abstractions;
using System;
using System.Reflection;

namespace AMQP.ServiceFramework.Core.Activation
{
    internal sealed class CommandHandlerContext : ICommandHandlerContext
    {
        public string Queue { get; internal set; }
        public string Topic { get; internal set; }
        public MethodInfo TargetMethod { get; internal set; }
        public Type DeclaringType { get; internal set; }
        public Type ParameterType { get; internal set; }
        public Type ParserType { get; internal set; }
    }
}