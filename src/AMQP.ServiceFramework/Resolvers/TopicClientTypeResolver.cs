using AMQP.ServiceFramework.Attributes;
using AMQP.ServiceFramework.Factories;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMQP.ServiceFramework.Resolvers
{
    internal sealed class TopicClientTypeResolver : ITypeResolver
    {
        private readonly IAttributeResolverFactory<Type, TopicClientAttribute> _attributeResolverFactory;

        public TopicClientTypeResolver(IAttributeResolverFactory<Type, TopicClientAttribute> attributeResolverFactory)
        {
            _attributeResolverFactory = attributeResolverFactory ?? throw new ArgumentNullException(nameof(attributeResolverFactory));
        }

        public IEnumerable<Type> ResolveTypes(Assembly assembly)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            var resolver = _attributeResolverFactory.CreateAttributeResolver();

            foreach (var type in assembly.GetTypes())
            {
                var attribute = resolver.Invoke(type);
                if (!(attribute is null))
                {
                    yield return type;
                }
            }
        }
    }
}