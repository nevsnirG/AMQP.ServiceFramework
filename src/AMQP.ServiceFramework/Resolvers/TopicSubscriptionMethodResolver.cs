using AMQP.ServiceFramework.Attributes;
using AMQP.ServiceFramework.Factories;
using System;
using System.Collections.Generic;
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

            foreach (var method in type.GetMethods(BindingFlags.Instance))
            {
                var attribute = resolver.Invoke(method);
                if (!(attribute is null))
                    yield return method;
            }
        }
    }
}