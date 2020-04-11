using System.Reflection;

namespace AMQP.ServiceFramework.Resolvers
{
    internal sealed class AssemblyResolver : IAssemblyResolver
    {
        public Assembly Resolve()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}