using AMQP.ServiceFramework.Attributes;
using System;
using System.Reflection;

namespace AMQP.ServiceFramework.Factories
{
    internal sealed class TypeAttributeResolverFactory : IAttributeResolverFactory<Type, TopicClientAttribute>
    {
        public Func<Type, TopicClientAttribute> CreateAttributeResolver()
        {
            return (type) =>
            {
                return type.GetCustomAttribute<TopicClientAttribute>(false);
            };
        }
    }
}