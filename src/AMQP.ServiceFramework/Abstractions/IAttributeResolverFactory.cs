using System;

namespace AMQP.ServiceFramework.Abstractions
{
    public interface IAttributeResolverFactory<T, T2> where T2 : Attribute
    {
        Func<T, T2> CreateAttributeResolver();
    }
}