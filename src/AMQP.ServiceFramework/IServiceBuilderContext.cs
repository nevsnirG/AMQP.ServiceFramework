using Microsoft.Extensions.DependencyInjection;

namespace AMQP.ServiceFramework
{
    public interface IServiceBuilderContext
    {
        IServiceCollection Services { get; }
    }
}