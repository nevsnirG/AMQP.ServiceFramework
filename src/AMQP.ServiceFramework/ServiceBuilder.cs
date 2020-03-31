using AMQP.ServiceFramework.Activation;
using AMQP.ServiceFramework.Factories;
using AMQP.ServiceFramework.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace AMQP.ServiceFramework
{
    public abstract class ServiceBuilder : IServiceBuilder
    {
        private readonly object _lock;

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
                        var serviceBuilderContext = new Configuration(services);
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
        /// <param name="config">The <see cref="IConfiguration"/> instance.</param>
        protected virtual void Setup(IConfiguration config)
        {
            config.Services.TryAddTransient<ICommandHandlerContextFactory, CommandHandlerContextFactory>();
            config.Services.TryAddTransient<ICommandHandlerActivator, CommandHandlerActivator>();
            config.Services.TryAddTransient<IClassResolver, TopicClientTypeResolver>();
            config.Services.TryAddTransient<IMethodResolver, TopicSubscriptionMethodResolver>();
            config.Services.TryAddTransient<IAssemblyResolver, AssemblyResolver>();

            //We will build a new service provider in which the user can register services. This service provider will be accessible in the CommandHandlerActivator.
            var commandHandlerServiceProvider = GetUserServiceProvider();
            config.Services.TryAddSingleton(commandHandlerServiceProvider);

            var serviceProvider = config.Services.BuildServiceProvider();
            Initialize(serviceProvider);            
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

        private void Initialize(IServiceProvider serviceProvider)
        {
            var assemblyResolver = serviceProvider.GetRequiredService<IAssemblyResolver>();
            var typeResolver = serviceProvider.GetRequiredService<IClassResolver>();
            var methodResolver = serviceProvider.GetRequiredService<IMethodResolver>();
            var commandHandlerContextFactory = serviceProvider.GetRequiredService<ICommandHandlerContextFactory>();

            var assembly = assemblyResolver.Resolve();
            var types = typeResolver.MapAssembly(assembly);

            foreach (var type in types)
            {
                var methods = methodResolver.MapClass(type);

            }
        }
    }
}