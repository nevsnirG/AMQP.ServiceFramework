using AMQP.ServiceFramework.Activation;
using System;

namespace AMQP.ServiceFramework
{
    public interface ICommandHandlerRegistry : IDisposable
    {
        void Add(ICommandHandlerContext commandHandlerContext);
    }
}