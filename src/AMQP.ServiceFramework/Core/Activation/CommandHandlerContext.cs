using AMQP.ServiceFramework.Abstractions;
using System;
using System.Reflection;

namespace AMQP.ServiceFramework.Core.Activation
{
    internal sealed class CommandHandlerContext : ICommandHandlerContext
    {
        public string Queue { get; set; }
        public string Topic { get; set; }
        public MethodInfo TargetMethod { get; set; }
        public Type DeclaringType { get; set; }
        public Type ParameterType { get; set; }
        public Type ParserType { get; set; }
    }
}