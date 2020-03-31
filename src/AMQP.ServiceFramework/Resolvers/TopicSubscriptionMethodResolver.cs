using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMQP.ServiceFramework.Resolvers
{
    internal sealed class TopicSubscriptionMethodResolver : IMethodResolver
    {
        public IEnumerable<MethodInfo> MapClass(Type type)
        {
            throw new NotImplementedException();
        }
    }
}