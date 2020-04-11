using Microsoft.Extensions.DependencyInjection;

namespace AMQP.ServiceFramework
{
    internal sealed class Configuration : IConfiguration
    {
        public IServiceCollection Services { get; }

        public Configuration(IServiceCollection services)
        {
            Services = services;
        }
    }
}