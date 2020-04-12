using System;
using System.Collections.Generic;

namespace AMQP.ServiceFramework.Abstractions
{
    public interface ICommandHandlerRegistry : IDisposable, IEnumerable<KeyValuePair<ICommandHandlerContext, object>>
    {
        void Add(ICommandHandlerContext commandHandlerContext);
    }
}