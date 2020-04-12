using AMQP.ServiceFramework.Abstractions;
using System;
using System.Collections.Generic;

namespace AMQP.ServiceFramework.Core.Registries
{
    internal sealed class TopicSubscriptionRegistry : ITopicSubscriptionRegistry
    {
        private readonly ITopicSubscriptionActivator _topicSubscriptionActivator;
        private readonly IList<ITopicSubscription> _topicSubscriptions;

        public TopicSubscriptionRegistry(ITopicSubscriptionActivator topicSubscriptionActivator)
        {
            _topicSubscriptionActivator = topicSubscriptionActivator;
            _topicSubscriptions = new List<ITopicSubscription>();
        }

        public void Add(ITopicSubscription topicSubscription)
        {
            if (topicSubscription is null)
                throw new ArgumentNullException(nameof(topicSubscription));

            _topicSubscriptionActivator.Activate(topicSubscription);
            _topicSubscriptions.Add(topicSubscription);
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var topicSubscription in _topicSubscriptions)
                    {
                        topicSubscription.Dispose();
                    }

                    _topicSubscriptions.Clear();
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
