using AMQP.Plugin.Abstractions;
using AMQP.ServiceFramework.Abstractions;
using AMQP.ServiceFramework.Core.Exceptions;
using System;

namespace AMQP.ServiceFramework.Core.Registries
{
    internal sealed class TopicSubscription : ITopicSubscription
    {
        private readonly ICommandHandlerContext _commandHandlerContext;
        private readonly object _instance;

        private IConsumer _consumer;
        private bool _initialized;

        public TopicSubscription(ICommandHandlerContext commandHandlerContext, object instance)
        {
            _commandHandlerContext = commandHandlerContext ?? throw new ArgumentNullException(nameof(commandHandlerContext));
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public void EnsureInitialization(IConnection connection, IMessageParserRegistry messageParserRegistry)
        {
            if (connection is null)
                throw new ArgumentNullException(nameof(connection));
            if (messageParserRegistry is null)
                throw new ArgumentNullException(nameof(messageParserRegistry));

            if (!_initialized)
            {
                var messageParser = messageParserRegistry.Retrieve(_commandHandlerContext.ParserType);
                if (!messageParser.CanParse(_commandHandlerContext.ParameterType))
                    throw new ServiceBuilderException($"The {_commandHandlerContext.ParserType} can not parse messages of type {_commandHandlerContext.ParameterType}.");

                _consumer = connection.CreateConsumer(_commandHandlerContext.Topic);
                _consumer.RegisterConsumer(_commandHandlerContext.Queue, (sender, args) =>
                {
                    //TODO - Add manual acknowledge functionality.
                    var body = args.Body;
                    var message = messageParser.Parse(body);
                    _commandHandlerContext.TargetMethod.Invoke(_instance, new object[] { message });
                });
                _initialized = true;
            }
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _consumer?.Dispose();
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