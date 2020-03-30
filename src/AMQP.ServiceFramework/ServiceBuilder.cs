using AMQP.ServiceFramework.Activation;
using AMQP.ServiceFramework.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace AMQP.ServiceFramework
{
    public abstract class ServiceBuilder : IServiceBuilder
    {
        private readonly object _lock;

        private IServiceProvider _serviceProvider;
        private bool _initialized;

        protected ServiceBuilder()
        {
            _lock = new object();
        }

        /// <summary>
        /// Ensure the class has been initialized.
        /// </summary>
        public void EnsureInitialization()
        {
            if (!_initialized)
            {
                lock (_lock)
                {
                    if (!_initialized)
                    {
                        var services = new ServiceCollection();
                        var serviceBuilderContext = new ServiceBuilderContext(services);
                        Setup(serviceBuilderContext);

                        _initialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// <br>Set up the service builder and its required dependencies.</br>
        /// <br>Make sure to invoke the base method when overriding.</br>
        /// </summary>
        /// <param name="context">The <see cref="IServiceBuilderContext"/> instance.</param>
        protected virtual void Setup(IServiceBuilderContext context)
        {
            context.Services.TryAddTransient<ICommandHandlerContextFactory, CommandHandlerContextFactory>();
            context.Services.TryAddTransient<ICommandHandlerActivator, CommandHandlerActivator>();

            //We will build a new service provider in which the user can register services. This service provider will be accessible in the CommandHandlerActivator.
            var commandHandlerServiceProvider = GetUserServiceProvider();
            context.Services.TryAddSingleton(commandHandlerServiceProvider);

            _serviceProvider = context.Services.BuildServiceProvider();

            //set dependencies
            
        }

        /// <summary>
        /// Build and retrieve the registered dependencies for the command handlers.
        /// </summary>
        /// <returns>The <see cref="IServiceProvider"/> instance containing user registered dependencies.</returns>
        private IServiceProvider GetUserServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            RegisterDependencies(services);
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// <br>Register dependencies for the command handlers.</br>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register dependencies in.</param>
        protected virtual void RegisterDependencies(IServiceCollection services)
        {
        }
    }
}