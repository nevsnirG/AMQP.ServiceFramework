using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMQP.ServiceFramework.Resolvers
{
    internal sealed class TopicClientTypeResolver : IClassResolver
    {
        public IEnumerable<Type> MapAssembly(Assembly assembly)
        {
            throw new NotImplementedException();
        }
    }
}