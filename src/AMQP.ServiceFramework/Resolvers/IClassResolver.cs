using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMQP.ServiceFramework.Resolvers
{
    public interface IClassResolver
    {
        IEnumerable<Type> MapAssembly(Assembly assembly);
    }
}