using AMQP.ServiceFramework.Activation;
using AMQP.ServiceFramework.Attributes;
using System;
using System.Reflection;

namespace AMQP.ServiceFramework.Factories
{
    internal sealed class CommandHandlerContextFactory : ICommandHandlerContextFactory
    {
        public ICommandHandlerContext Create(MethodInfo methodInfo)
        {
            if (methodInfo is null)
                throw new ArgumentNullException(nameof(methodInfo));

            //LOW - Check return type is an event or void and add to context.
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 0 || parameters.Length > 1)
                throw new ArgumentException("The specified method can only have 1 parameter.", nameof(methodInfo));

            var declaringType = methodInfo.DeclaringType;
            var topicClientAttribute = declaringType.GetCustomAttribute<TopicClientAttribute>(false);
            if (topicClientAttribute is null)
                throw new ArgumentException($"The declaring type of the specified method is not attributed with the {nameof(TopicClientAttribute)}.", nameof(methodInfo));

            var topicSubscriptionAttribute = methodInfo.GetCustomAttribute<TopicSubscriptionAttribute>(false);
            if (topicSubscriptionAttribute is null)
                throw new ArgumentException($"The specified method is not attributed with the {nameof(TopicSubscriptionAttribute)}.", nameof(methodInfo));

            return new CommandHandlerContext()
            {
                DeclaringType = declaringType,
                Queue = topicClientAttribute.Queue ?? string.Empty,
                TargetMethod = methodInfo,
                Topic = topicSubscriptionAttribute.Topic
            };
        }
    }
}