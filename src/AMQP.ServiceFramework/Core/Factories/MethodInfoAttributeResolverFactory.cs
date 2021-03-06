﻿using AMQP.ServiceFramework.Abstractions;
using AMQP.ServiceFramework.Attributes;
using System;
using System.Reflection;

namespace AMQP.ServiceFramework.Core.Factories
{
    internal sealed class MethodInfoAttributeResolverFactory : IAttributeResolverFactory<MethodInfo, TopicSubscriptionAttribute>
    {
        public Func<MethodInfo, TopicSubscriptionAttribute> CreateAttributeResolver()
        {
            return (method) =>
            {
                return method.GetCustomAttribute<TopicSubscriptionAttribute>(false);
            };
        }
    }
}