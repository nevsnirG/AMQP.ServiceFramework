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

            var parameter = parameters[0];
            var declaringType = methodInfo.DeclaringType;
            var topicClientAttributes = declaringType.GetCustomAttributes(typeof(TopicSubscriptionAttribute), false);
            if (topicClientAttributes.Length == 0)
                throw new ArgumentException($"The specified method's declaring type is not attributed with the {nameof(TopicClientAttribute)}.", nameof(methodInfo));

            var topicClientAttribute = topicClientAttributes[0] as TopicClientAttribute;
            var topicSubscriptionAttributes = methodInfo.GetCustomAttributes(typeof(TopicSubscriptionAttribute), false);
            if (topicSubscriptionAttributes.Length == 0)
                throw new ArgumentException($"The specified method is not attributed with the {nameof(TopicSubscriptionAttribute)}.", nameof(methodInfo));

            var topicSubscriptionAttribute = topicSubscriptionAttributes[0] as TopicSubscriptionAttribute;
            return new CommandHandlerContext()
            {
                DeclaringType = declaringType,
                Queue = topicClientAttribute.Queue ?? string.Empty,
                TargetMethod = methodInfo,
                Topic = topicSubscriptionAttribute.Topic,
                ParameterType = parameter.ParameterType
            };
        }
    }
}