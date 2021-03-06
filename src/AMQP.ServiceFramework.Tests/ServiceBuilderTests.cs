using AMQP.Plugin.Abstractions;
using Moq;
using Moq.Protected;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace AMQP.ServiceFramework.Tests
{
    [ExcludeFromCodeCoverage]
    public class ServiceBuilderTests
    {
        private readonly Mock<IConnection> _connectionMock;
        private readonly Mock<ServiceBuilder> _serviceBuilderMock;

        public ServiceBuilderTests()
        {
            _connectionMock = new Mock<IConnection>(MockBehavior.Strict);
            _serviceBuilderMock = new Mock<ServiceBuilder>(MockBehavior.Strict, _connectionMock.Object);
        }

        [Fact]
        public void EnsureInitialization_OnlyRunsOnce()
        {
            //Arrange
            _serviceBuilderMock.Protected()
                .Setup("Setup", ItExpr.IsAny<IConfiguration>())
                .Verifiable("This method should only have been called once.");
            _serviceBuilderMock.Protected();
            IServiceBuilder serviceBuilder = _serviceBuilderMock.Object;

            //Act
            for (var i = 0; i < 2; i++)
                serviceBuilder.EnsureInitialization();

            //Assert
            _serviceBuilderMock.Protected()
                .Verify("Setup", Times.Once(), ItExpr.IsAny<IConfiguration>());
        }
    }
}