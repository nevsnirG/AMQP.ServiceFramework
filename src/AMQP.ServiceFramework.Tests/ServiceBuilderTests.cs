using Moq;
using Moq.Protected;
using Xunit;

namespace AMQP.ServiceFramework.Tests
{
    public class ServiceBuilderTests
    {
        private readonly Mock<ServiceBuilder> _serviceBuilderMock;

        public ServiceBuilderTests()
        {
            _serviceBuilderMock = new Mock<ServiceBuilder>(MockBehavior.Strict);
        }

        [Fact]
        public void EnsureInitialization_OnlyRunsOnce()
        {
            //Arrange
            _serviceBuilderMock.Protected()
                .Setup("Setup", ItExpr.IsAny<IServiceBuilderContext>())
                .Verifiable("This method should only have been called once.");
            _serviceBuilderMock.Protected();
            IServiceBuilder serviceBuilder = _serviceBuilderMock.Object;

            //Act
            for (var i = 0; i < 2; i++)
                serviceBuilder.EnsureInitialization();

            //Assert
            _serviceBuilderMock.Protected()
                .Verify("Setup", Times.Once(), ItExpr.IsAny<IServiceBuilderContext>());
        }
    }
}