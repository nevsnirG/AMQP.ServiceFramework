using System;
using System.Collections.Generic;

namespace AMQP.ServiceFramework.Registries
{
    internal sealed class MessageParserRegistry : IMessageParserRegistry
    {
        private readonly object _lock;
        private readonly IDictionary<Type, MessageParser> _parserTypes;

        public MessageParserRegistry()
        {
            _lock = new object();
            _parserTypes = new Dictionary<Type, MessageParser>();
        }

        public MessageParser Retrieve(Type type)
        {
            lock (_lock)
            {
                if (!_parserTypes.TryGetValue(type, out var parser))
                {
                    parser = Activator.CreateInstance(type) as MessageParser;
                    _parserTypes.Add(type, parser); //TODO - Handle exceptions.
                }
                return parser;
            }
        }
    }
}