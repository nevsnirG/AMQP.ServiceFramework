using Microsoft.Extensions.DependencyInjection;

namespace AMQP.ServiceFramework
{
    public interface IConfiguration
    {
        IServiceCollection Services { get; }
    }
}