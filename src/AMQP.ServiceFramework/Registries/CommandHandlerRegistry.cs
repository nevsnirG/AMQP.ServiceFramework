using AMQP.ServiceFramework.Activation;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AMQP.ServiceFramework.Registries
{
    internal sealed class CommandHandlerRegistry : ICommandHandlerRegistry
    {
        private readonly ICommandHandlerActivator _commandHandlerActivator;
        private readonly IDictionary<ICommandHandlerContext, object> _commandHandlers;

        public CommandHandlerRegistry(ICommandHandlerActivator commandHandlerActivator)
        {
            _commandHandlerActivator = commandHandlerActivator ?? throw new ArgumentNullException(nameof(commandHandlerActivator));
            _commandHandlers = new Dictionary<ICommandHandlerContext, object>();
        }

        public void Add(ICommandHandlerContext commandHandlerContext)
        {
            if (commandHandlerContext is null)
                throw new ArgumentNullException(nameof(commandHandlerContext));

            var instance = _commandHandlerActivator.Create(commandHandlerContext);
            _commandHandlers.Add(commandHandlerContext, instance);
        }

        public IEnumerator<KeyValuePair<ICommandHandlerContext, object>> GetEnumerator()
        {
            return _commandHandlers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _commandHandlers.GetEnumerator();
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
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