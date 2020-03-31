using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMQP.ServiceFramework.Resolvers
{
    public interface IMethodResolver
    {
        IEnumerable<MethodInfo> MapClass(Type type);
    }
}