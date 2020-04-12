using AMQP.ServiceFramework.Abstractions;
using System.Reflection;

namespace AMQP.ServiceFramework.Core.Resolvers
{
    internal sealed class AssemblyResolver : IAssemblyResolver
    {
        public Assembly Resolve()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}