using Microsoft.Extensions.DependencyInjection;

namespace AMQP.ServiceFramework
{
    public class ServiceBuilderContext
    {
        public IServiceCollection Services { get; }

        public ServiceBuilderContext()
        {
            Services = new ServiceCollection();
        }
    }
}