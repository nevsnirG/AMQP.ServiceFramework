using AMQP.ServiceFramework.Abstractions;
using AMQP.ServiceFramework.Attributes;
using AMQP.ServiceFramework.Core.Activation;
using System;
using System.Reflection;

namespace AMQP.ServiceFramework.Core.Factories
{
    internal sealed class CommandHandlerContextFactory : ICommandHandlerContextFactory
    {
        public ICommandHandlerContext Create(MethodInfo methodInfo)
        {
            if (methodInfo is null)
                throw new ArgumentNullException(nameof(methodInfo));

            if (methodInfo.ReturnType != typeof(void))
                throw new ArgumentException("The specified method should be of type void.", nameof(methodInfo));

            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 0 || parameters.Length > 1)
                throw new ArgumentException("The specified method can only have 1 parameter.", nameof(methodInfo));

            var parameter = parameters[0];
            var declaringType = methodInfo.DeclaringType;
            var topicClientAttributes = declaringType.GetCustomAttributes(typeof(TopicClientAttribute), false);
            if (topicClientAttributes.Length == 0)
                throw new ArgumentException($"The specified method's declaring type is not attributed with the {nameof(TopicClientAttribute)}.", nameof(methodInfo));

            var topicClientAttribute = topicClientAttributes[0] as TopicClientAttribute;
            var topicSubscriptionAttributes = methodInfo.GetCustomAttributes(typeof(TopicSubscriptionAttribute), false);
            if (topicSubscriptionAttributes.Length == 0)
                throw new ArgumentException($"The specified method is not attributed with the {nameof(TopicSubscriptionAttribute)}.", nameof(methodInfo));

            var topicSubscriptionAttribute = topicSubscriptionAttributes[0] as TopicSubscriptionAttribute;
            //TODO - Check if topic is not null or empty and if parsertype is not null.

            return new CommandHandlerContext()
            {
                DeclaringType = declaringType,
                Queue = topicClientAttribute.Queue ?? string.Empty,
                TargetMethod = methodInfo,
                Topic = topicSubscriptionAttribute.Topic,
                ParameterType = parameter.ParameterType,
                ParserType = topicSubscriptionAttribute.ParserType
            };
        }
    }
}