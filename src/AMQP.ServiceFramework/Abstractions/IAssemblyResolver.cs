using System.Reflection;

namespace AMQP.ServiceFramework.Abstractions
{
    public interface IAssemblyResolver
    {
        Assembly Resolve();
    }
}