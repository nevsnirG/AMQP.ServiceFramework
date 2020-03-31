using AMQP.ServiceFramework.Activation;
using AMQP.ServiceFramework.Extensions;
using AMQP.ServiceFramework.Factories;
using AMQP.ServiceFramework.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace AMQP.ServiceFramework
{
    public abstract class ServiceBuilder : IServiceBuilder, IDisposable
    {
        private readonly object _lock;

        private ICommandHandlerRegistry _commandHandlerRegistry;
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
                        Initialize();

                        _initialized = true;
                    }
                }
            }
        }

        private void Initialize()
        {
            var services = new ServiceCollection();
            var config = new Configuration(services);
            Configure(config);

            //TODO - Improve using MediatR.
            var serviceProvider = config.Services.BuildServiceProvider();
            var assemblyResolver = serviceProvider.GetRequiredService<IAssemblyResolver>();
            var typeResolver = serviceProvider.GetRequiredService<ITypeResolver>();
            var methodResolver = serviceProvider.GetRequiredService<IMethodResolver>();
            var commandHandlerContextFactory = serviceProvider.GetRequiredService<ICommandHandlerContextFactory>();
            _commandHandlerRegistry = serviceProvider.GetRequiredService<ICommandHandlerRegistry>();

            var assembly = assemblyResolver.Resolve();
            var types = typeResolver.ResolveTypes(assembly);
            var methods = methodResolver.ResolveMethods(types);
            var commandHandlerContexts = commandHandlerContextFactory.Create(methods);
            _commandHandlerRegistry.Add(commandHandlerContexts);
        }

        private void Configure(IConfiguration configuration)
        {
            Setup(configuration);

            configuration.Services.TryAddTransient<ICommandHandlerRegistry, CommandHandlerRegistry>();
            configuration.Services.TryAddTransient<ICommandHandlerContextFactory, CommandHandlerContextFactory>();
            configuration.Services.TryAddTransient<ICommandHandlerActivator, CommandHandlerActivator>();
            configuration.Services.TryAddTransient<ITypeResolver, TopicClientTypeResolver>();
            configuration.Services.TryAddTransient<IMethodResolver, TopicSubscriptionMethodResolver>();
            configuration.Services.TryAddTransient<IAssemblyResolver, AssemblyResolver>();
        }

        /// <summary>
        /// Set up the service builder and register dependencies.
        /// </summary>
        /// <param name="config">The <see cref="IConfiguration"/> instance.</param>
        protected virtual void Setup(IConfiguration config)
        {
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _commandHandlerRegistry?.Dispose();
                }

                _disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}