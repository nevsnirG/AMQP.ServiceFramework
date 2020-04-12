using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMQP.ServiceFramework.Abstractions
{
    public interface IMethodResolver
    {
        IEnumerable<MethodInfo> ResolveMethods(Type type);
    }
}