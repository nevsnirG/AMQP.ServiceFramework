using AMQP.ServiceFramework.Attributes;
using AMQP.ServiceFramework.Factories;
using Moq;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace AMQP.ServiceFramework.Tests
{
    public class CommandHandlerContextFactoryTests
    {
        private readonly Mock<MethodInfo> _methodInfoMock;

        private readonly Expression<Func<MethodInfo, ParameterInfo[]>> _getParametersMock;

        //private readonly Expression<Func<MethodInfo, TopicSubscriptionAttribute>> 

        public CommandHandlerContextFactoryTests()
        {
            _methodInfoMock = new Mock<MethodInfo>(MockBehavior.Strict);

            _getParametersMock = (method) => method.GetParameters();
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
            _methodInfoMock.Setup(_getParametersMock)
                .Returns(new ParameterInfo[count]);
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
                _methodInfoMock.Verify(_getParametersMock);
                Assert.Equal("methodInfo", ex.ParamName);
                Assert.StartsWith("The specified method can only have 1 parameter.", ex.Message);
            }
        }
    }
}