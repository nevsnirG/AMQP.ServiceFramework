using AMQP.ServiceFramework.Attributes;
using AMQP.ServiceFramework.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AMQP.ServiceFramework.Resolvers
{
    internal sealed class TopicSubscriptionMethodResolver : IMethodResolver
    {
        private readonly IAttributeResolverFactory<MethodInfo, TopicSubscriptionAttribute> _attributeResolverFactory;

        public TopicSubscriptionMethodResolver(IAttributeResolverFactory<MethodInfo, TopicSubscriptionAttribute> attributeResolverFactory)
        {
            _attributeResolverFactory = attributeResolverFactory ?? throw new ArgumentNullException(nameof(attributeResolverFactory));
        }

        public IEnumerable<MethodInfo> ResolveMethods(Type type)
        {
            var resolver = _attributeResolverFactory.CreateAttributeResolver();

            var interfaceType = typeof(ISubscription<>);
            var interfaces = from i in type.GetInterfaces()
                             where i.Name == interfaceType.Name
                             select i;

            foreach (var i in interfaces)
            {
                foreach (var iMethod in i.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    foreach (var cMethod in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (iMethod.ToString().Equals(cMethod.ToString(), StringComparison.CurrentCulture))
                        {
                            var attribute = resolver.Invoke(cMethod);
                            if (!(attribute is null))
                                yield return cMethod;
                        }
                    }
                }
            }
        }
    }
}