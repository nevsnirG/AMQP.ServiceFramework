using Microsoft.Extensions.DependencyInjection;

namespace AMQP.ServiceFramework
{
    internal sealed class ServiceBuilderContext : IServiceBuilderContext
    {
        public IServiceCollection Services { get; }

        public ServiceBuilderContext()
        {
            Services = new ServiceCollection();
        }
    }
}