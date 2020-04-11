﻿using AMQP.ServiceFramework.Activation;
using AMQP.ServiceFramework.Attributes;
using AMQP.ServiceFramework.Extensions;
using AMQP.ServiceFramework.Factories;
using AMQP.ServiceFramework.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

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
            Configure(new Configuration(services));

            var serviceProvider = services.BuildServiceProvider();

            //resolve the assembly.
            var assemblyResolver = serviceProvider.GetRequiredService<IAssemblyResolver>();
            var assembly = assemblyResolver.Resolve();

            //resolve all types in the assembly.
            var typeResolver = serviceProvider.GetRequiredService<ITypeResolver>();
            var types = typeResolver.ResolveTypes(assembly);

            //resolve all methods in the types.
            var methodResolver = serviceProvider.GetRequiredService<IMethodResolver>();
            var methods = methodResolver.ResolveMethods(types);

            //create CommandHandlerContexts from all methods.
            var commandHandlerContextFactory = serviceProvider.GetRequiredService<ICommandHandlerContextFactory>();
            var commandHandlerContexts = commandHandlerContextFactory.Create(methods);

            //add all CommandHandlerContexts to the registry.
            _commandHandlerRegistry = serviceProvider.GetRequiredService<ICommandHandlerRegistry>();
            _commandHandlerRegistry.AddRange(commandHandlerContexts);
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
            configuration.Services.TryAddTransient<IAttributeResolverFactory<Type, TopicClientAttribute>, TypeAttributeResolverFactory>();
            configuration.Services.TryAddTransient<IAttributeResolverFactory<MethodInfo, TopicSubscriptionAttribute>, MethodInfoAttributeResolverFactory>();
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