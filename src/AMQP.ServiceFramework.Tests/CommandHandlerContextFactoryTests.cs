using AMQP.ServiceFramework.Abstractions;
using AMQP.ServiceFramework.Attributes;
using AMQP.ServiceFramework.Core.Factories;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace AMQP.ServiceFramework.Tests
{
    [ExcludeFromCodeCoverage]
    public class CommandHandlerContextFactoryTests
    {
        private readonly Mock<Type> _declaringTypeMock;
        private readonly Mock<MethodInfo> _methodInfoMock;
        private readonly Mock<ParameterInfo> _parameterInfoMock;

        public CommandHandlerContextFactoryTests()
        {
            _declaringTypeMock = new Mock<Type>(MockBehavior.Strict);
            _methodInfoMock = new Mock<MethodInfo>(MockBehavior.Strict);
            _parameterInfoMock = new Mock<ParameterInfo>(MockBehavior.Strict);

            _methodInfoMock.Setup((method) => method.DeclaringType)
                    .Returns(_declaringTypeMock.Object);
            _parameterInfoMock.Setup((parameter) => parameter.ParameterType)
                .Returns(typeof(CommandHandlerContextFactoryTests));
        }

        [Fact]
        public void Create_NullInput_ArgumentNullException()
        {
            //Arrange
            var commandHandlerContextFactory = new CommandHandlerContextFactory();

            try
            {
                //Act
                commandHandlerContextFactory.Create(null);
                Assert.True(false, "The method was supposed to throw an ArgumentNullException.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal("methodInfo", ex.ParamName);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Create_MethodInfo_InvalidParameterCount(int count)
        {
            //Arrange
            Expression<Func<MethodInfo, ParameterInfo[]>> getParametersMock = (method) => method.GetParameters();
            _methodInfoMock.Setup(getParametersMock)
                .Returns(new ParameterInfo[count]);
            _methodInfoMock.Setup((methodInfo) => methodInfo.ReturnType)
                .Returns(typeof(void));
            var commandHandlerContextFactory = new CommandHandlerContextFactory();

            try
            {
                //Act
                commandHandlerContextFactory.Create(_methodInfoMock.Object);

                //Assert
                Assert.True(false, "The method was supposed to throw an ArgumentException.");
            }
            catch (ArgumentException ex)
            {
                _methodInfoMock.Verify(getParametersMock);
                Assert.Equal("methodInfo", ex.ParamName);
                Assert.StartsWith("The specified method can only have 1 parameter.", ex.Message);
            }
        }

        [Fact]
        public void Create_DeclaringTypeNoTopicClientAttribute_ArgumentException()
        {
            //Arrange
            Expression<Func<MethodInfo, ParameterInfo[]>> getParametersMock = (method) => method.GetParameters();
            _methodInfoMock.Setup(getParametersMock)
                .Returns(new ParameterInfo[1]);
            _methodInfoMock.Setup((methodInfo) => methodInfo.ReturnType)
                .Returns(typeof(void));
            Expression<Func<Type, object[]>> getCustomAttributesMock = (type) => type.GetCustomAttributes(It.IsAny<Type>(), It.IsAny<bool>());
            _declaringTypeMock.Setup(getCustomAttributesMock)
                .Returns(() =>
                {
                    return new object[0];
                });
            var commandHandlerContextFactory = new CommandHandlerContextFactory();

            try
            {
                //Act
                commandHandlerContextFactory.Create(_methodInfoMock.Object);

                //Assert
                Assert.True(false, "The method was supposed to throw an ArgumentException.");
            }
            catch (ArgumentException ex)
            {
                _methodInfoMock.Verify(getParametersMock);
                _declaringTypeMock.Verify(getCustomAttributesMock);
                Assert.Equal("methodInfo", ex.ParamName);
                Assert.StartsWith($"The specified method's declaring type is not attributed with the {nameof(TopicClientAttribute)}.", ex.Message);
            }
        }

        [Fact]
        public void Create_MethodInfoNoTopicSubscriptionAttribute_ArgumentException()
        {
            //Arrange
            Expression<Func<MethodInfo, ParameterInfo[]>> getParametersMock = (method) => method.GetParameters();
            _methodInfoMock.Setup(getParametersMock)
                .Returns(new ParameterInfo[1]);
            _methodInfoMock.Setup((methodInfo) => methodInfo.ReturnType)
                .Returns(typeof(void));
            Expression<Func<Type, object[]>> getDeclaringTypeCustomAttributesMock = (type) => type.GetCustomAttributes(It.IsAny<Type>(), It.IsAny<bool>());
            _declaringTypeMock.Setup(getDeclaringTypeCustomAttributesMock)
                .Returns(() =>
                {
                    return new object[1]
                    {
                        new TopicClientAttribute()
                    };
                });
            Expression<Func<MethodInfo, object[]>> getMethodInfoCustomAttributesMock = (method) => method.GetCustomAttributes(It.IsAny<Type>(), It.IsAny<bool>());
            _methodInfoMock.Setup(getMethodInfoCustomAttributesMock)
                .Returns(() => new object[0]);
            var commandHandlerContextFactory = new CommandHandlerContextFactory();

            try
            {
                //Act
                commandHandlerContextFactory.Create(_methodInfoMock.Object);

                //Assert
                Assert.True(false, "The method was supposed to throw an ArgumentException.");
            }
            catch (ArgumentException ex)
            {
                _methodInfoMock.Verify(getParametersMock);
                _declaringTypeMock.Verify(getDeclaringTypeCustomAttributesMock);
                _methodInfoMock.Verify(getMethodInfoCustomAttributesMock);
                Assert.Equal("methodInfo", ex.ParamName);
                Assert.StartsWith($"The specified method is not attributed with the {nameof(TopicSubscriptionAttribute)}.", ex.Message);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("queue")]
        public void Create_Success(string queue)
        {
            //Arrange
            const string topic = nameof(topic);
            Expression<Func<MethodInfo, ParameterInfo[]>> getParametersMock = (method) => method.GetParameters();
            _methodInfoMock.Setup(getParametersMock)
                .Returns(() =>
                {
                    return new ParameterInfo[1]
                    {
                        _parameterInfoMock.Object
                    };
                });
            _methodInfoMock.Setup((methodInfo) => methodInfo.ReturnType)
                .Returns(typeof(void));
            Expression<Func<Type, object[]>> getDeclaringTypeCustomAttributesMock = (type) => type.GetCustomAttributes(It.IsAny<Type>(), It.IsAny<bool>());
            _declaringTypeMock.Setup(getDeclaringTypeCustomAttributesMock)
                .Returns((Type type, bool b) =>
                {
                    if (b)
                        Assert.True(false, "The parameter should be false.");

                    return new object[1]
                    {
                        new TopicClientAttribute(queue)
                    };
                });
            Expression<Func<MethodInfo, object[]>> getMethodInfoCustomAttributesMock = (method) => method.GetCustomAttributes(It.IsAny<Type>(), It.IsAny<bool>());
            _methodInfoMock.Setup(getMethodInfoCustomAttributesMock)
                .Returns((Type type, bool b) =>
                {
                    if (b)
                        Assert.True(false, "The parameter should be false.");

                    return new object[1]
                    {
                        new TopicSubscriptionAttribute(topic, typeof(MessageParser))
                    };
                });
            var commandHandlerContextFactory = new CommandHandlerContextFactory();

            //Act
            var context = commandHandlerContextFactory.Create(_methodInfoMock.Object);

            //Assert
            Assert.Equal(_declaringTypeMock.Object, context.DeclaringType);
            Assert.Equal(_parameterInfoMock.Object.ParameterType, context.ParameterType);
            Assert.Equal(queue ?? string.Empty, context.Queue);
            Assert.Equal(topic, context.Topic);
            Assert.Equal(_methodInfoMock.Object, context.TargetMethod);
            _methodInfoMock.Verify(getParametersMock);
            _declaringTypeMock.Verify(getDeclaringTypeCustomAttributesMock);
            _methodInfoMock.Verify(getMethodInfoCustomAttributesMock);
        }
    }
}