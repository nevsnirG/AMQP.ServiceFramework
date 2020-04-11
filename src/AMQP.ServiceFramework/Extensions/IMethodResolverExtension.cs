using AMQP.ServiceFramework.Resolvers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMQP.ServiceFramework.Extensions
{
    internal static class IMethodResolverExtension
    {
        public static IEnumerable<MethodInfo> ResolveMethods(this IMethodResolver methodResolver, IEnumerable<Type> types)
        {
            if (methodResolver is null)
                throw new ArgumentNullException(nameof(methodResolver));
            if (types is null)
                throw new ArgumentNullException(nameof(types));

            foreach (var type in types)
                foreach (var method in methodResolver.ResolveMethods(type))
                    yield return method;
        }
    }
}