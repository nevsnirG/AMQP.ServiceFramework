using System.Reflection;

namespace AMQP.ServiceFramework.Resolvers
{
    public interface IAssemblyResolver
    {
        Assembly Resolve();
    }
}