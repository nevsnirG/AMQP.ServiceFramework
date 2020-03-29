using AMQP.ServiceFramework.Activation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace AMQP.ServiceFramework
{
    public abstract class ServiceBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly object _lock;

        private bool _initialized;
        
        public ServiceBuilder()
        {
            _lock = new object();
        }

        public void EnsureInitialization()
        {
            if (!_initialized)
            {
                lock (_lock)
                {
                    if (!_initialized)
                    {
                        Initialize(new ServiceBuilderContext());
                        _initialized = true;
                    }
                }
            }
        }

        protected virtual void Initialize(ServiceBuilderContext context)
        {
            context.Services.TryAddTransient<ICommandHandlerActivator, CommandHandlerActivator>();

            //We will build a new service provider in which the user can register services. This service provider will be accessible in the CommandHandlerActivator.
            var commandHandlerServiceProvider = GetUserServiceProvider();
            context.Services.TryAddSingleton(commandHandlerServiceProvider);
        }

        private IServiceProvider GetUserServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            RegisterDependencies(services);
            return services.BuildServiceProvider();
        }

        protected virtual void RegisterDependencies(IServiceCollection services)
        {
        }
    }
}