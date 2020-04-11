using AMQP.Plugin.Abstractions;
using AMQP.ServiceFramework.Activation;
using AMQP.ServiceFramework.Registries;
using System;

namespace AMQP.ServiceFramework
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
            if (!_initialized)
            {
                var messageParser = messageParserRegistry.Retrieve(_commandHandlerContext.ParameterType);
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
        private bool _disposedValue = false; // To detect redundant calls

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