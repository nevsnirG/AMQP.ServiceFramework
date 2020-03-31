using AMQP.ServiceFramework.Activation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMQP.ServiceFramework
{
    internal sealed class CommandHandlerRegistry : ICommandHandlerRegistry
    {
        private readonly ICommandHandlerActivator _commandHandlerActivator;
        private readonly IDictionary<ICommandHandlerContext, object> _commandHandlers;

        public CommandHandlerRegistry(ICommandHandlerActivator commandHandlerActivator)
        {
            _commandHandlerActivator = commandHandlerActivator ?? throw new ArgumentNullException(nameof(commandHandlerActivator));
        }

        public void Add(ICommandHandlerContext commandHandlerContext)
        {
            var instance = _commandHandlerActivator.Create(commandHandlerContext);
            _commandHandlers.Add(commandHandlerContext, instance);
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var pair in _commandHandlers)
                    {
                        _commandHandlerActivator.Release(pair.Key, pair.Value);
                    }

                    _commandHandlers.Clear();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}