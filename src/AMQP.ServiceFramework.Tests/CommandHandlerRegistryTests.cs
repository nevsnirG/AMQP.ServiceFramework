using AMQP.ServiceFramework.Activation;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Xunit;

namespace AMQP.ServiceFramework.Tests
{
    [ExcludeFromCodeCoverage]
    public class CommandHandlerRegistryTests
    {
        private readonly Mock<ICommandHandlerActivator> _commandHandlerActivatorMock;

        private readonly Expression<Func<ICommandHandlerActivator, object>> _createExpression;
        private readonly Expression<Action<ICommandHandlerActivator>> _releaseExpression;

        public CommandHandlerRegistryTests()
        {
            _commandHandlerActivatorMock = new Mock<ICommandHandlerActivator>(MockBehavior.Strict);

            _createExpression = (activator) => activator.Create(It.IsAny<ICommandHandlerContext>());
            _releaseExpression = (activator) => activator.Release(It.IsAny<ICommandHandlerContext>(), It.IsAny<object>());
        }

        [Fact]
        public void Constructor_NullInput_ArgumentNullException()
        {
            //Act
            try
            {
                new CommandHandlerRegistry(null);

                //Assert
                Assert.True(false, "The constructor was supposed to throw an ArgumentNullException.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal("commandHandlerActivator", ex.ParamName);
            }
        }

        [Fact]
        public void Add_NullInput_ArgumentNullException()
        {
            //Arrange
            var activator = _commandHandlerActivatorMock.Object;
            var registry = new CommandHandlerRegistry(activator);

            //Act
            try
            {
                registry.Add(null);

                //Assert
                Assert.True(false, "The method was supposed to throw an ArgumentNullException.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal("commandHandlerContext", ex.ParamName);
            }
        }

        [Fact]
        public void Add_Success()
        {
            //Arrange
            _commandHandlerActivatorMock.Setup(_createExpression).
                Returns(new object());
            var activator = _commandHandlerActivatorMock.Object;
            var registry = new CommandHandlerRegistry(activator);
            var context = new CommandHandlerContext();

            //Act
            registry.Add(context);

            //Assert
            _commandHandlerActivatorMock.Verify(_createExpression);
        }

        [Fact]
        public void Dispose_OnlyCalledOnce()
        {
            //Arrange
            _commandHandlerActivatorMock.Setup(_createExpression).
                Returns(new object());
            _commandHandlerActivatorMock.Setup(_releaseExpression)
                .Verifiable("This method should only have been called once.");
            var activator = _commandHandlerActivatorMock.Object;
            var registry = new CommandHandlerRegistry(activator);
            var context = new CommandHandlerContext();

            //Act
            registry.Add(context);
            registry.Dispose();
            registry.Dispose();

            //Assert
            _commandHandlerActivatorMock.Verify(_createExpression);
            _commandHandlerActivatorMock.Verify(_releaseExpression, Times.Once());
        }
    }
}