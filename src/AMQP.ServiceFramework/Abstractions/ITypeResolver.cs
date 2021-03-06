﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMQP.ServiceFramework.Abstractions
{
    public interface ITypeResolver
    {
        IEnumerable<Type> ResolveTypes(Assembly assembly);
    }
}