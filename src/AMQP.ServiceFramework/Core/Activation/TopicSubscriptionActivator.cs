﻿using AMQP.Plugin.Abstractions;
using AMQP.ServiceFramework.Abstractions;
using System;

namespace AMQP.ServiceFramework.Core.Activation
{
    internal sealed class TopicSubscriptionActivator : ITopicSubscriptionActivator
    {
        private readonly IConnection _connection;
        private readonly IMessageParserRegistry _messageParserRegistry;

        public TopicSubscriptionActivator(IConnection connection, IMessageParserRegistry messageParserRegistry)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _messageParserRegistry = messageParserRegistry ?? throw new ArgumentNullException(nameof(messageParserRegistry));
        }

        public void Activate(ITopicSubscription topicSubscription)
        {
            if (topicSubscription is null)
                throw new ArgumentNullException(nameof(topicSubscription));

            topicSubscription.EnsureInitialization(_connection, _messageParserRegistry);
        }
    }
}