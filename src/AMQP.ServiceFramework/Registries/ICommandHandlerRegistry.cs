using AMQP.ServiceFramework.Activation;
using System;
using System.Collections.Generic;

namespace AMQP.ServiceFramework.Registries
{
    public interface ICommandHandlerRegistry : IDisposable, IEnumerable<KeyValuePair<ICommandHandlerContext, object>>
    {
        void Add(ICommandHandlerContext commandHandlerContext);
    }
}