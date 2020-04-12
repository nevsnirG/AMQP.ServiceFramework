using AMQP.Plugin.Abstractions;
using AMQP.ServiceFramework.Abstractions;
using AMQP.ServiceFramework.Attributes;
using AMQP.ServiceFramework.Core.Activation;
using AMQP.ServiceFramework.Core.Extensions;
using AMQP.ServiceFramework.Core.Factories;
using AMQP.ServiceFramework.Core.Registries;
using AMQP.ServiceFramework.Core.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace AMQP.ServiceFramework
{
    public abstract class ServiceBuilder : IServiceBuilder, IDisposable
    {
        private readonly object _lock;
        private readonly IConnection _connection;

        private ICommandHandlerRegistry _commandHandlerRegistry;
        private ITopicSubscriptionRegistry _topicSubscriptionRegistry;
        private bool _initialized;

        protected ServiceBuilder(IConnection connection)
        {
            _lock = new object();
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
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

        /// <summary>
        /// Set up the service builder and register dependencies.
        /// </summary>
        /// <param name="config">The <see cref="IConfiguration"/> instance.</param>
        protected virtual void Setup(IConfiguration config)
        {
        }

        private void Initialize()
        {
            var services = new ServiceCollection();
            services.TryAddSingleton(_connection);

            Configure(new Configuration(services));

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<ServiceBuilder>>();
            logger?.LogInformation("Discovering and initializing subscriptions...");

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

            //instantiate all topic subscriptions.
            var topicSubscriptionFactory = serviceProvider.GetRequiredService<ITopicSubscriptionFactory>();
            _topicSubscriptionRegistry = serviceProvider.GetRequiredService<ITopicSubscriptionRegistry>();
            foreach (var keyPair in _commandHandlerRegistry)
            {
                var topicSubscription = topicSubscriptionFactory.CreateSubscription(keyPair.Key, keyPair.Value);
                _topicSubscriptionRegistry.Add(topicSubscription);
            }
            logger?.LogInformation("All subscriptions initialized successfully.");
        }

        private void Configure(IConfiguration configuration)
        {
            Setup(configuration);

            configuration.Services.TryAddTransient<ITopicSubscriptionRegistry, TopicSubscriptionRegistry>();
            configuration.Services.TryAddTransient<ITopicSubscriptionFactory, TopicSubscriptionFactory>();
            configuration.Services.TryAddTransient<ITopicSubscriptionActivator, TopicSubscriptionActivator>();
            configuration.Services.TryAddTransient<IMessageParserRegistry, MessageParserRegistry>();
            configuration.Services.TryAddTransient<ICommandHandlerRegistry, CommandHandlerRegistry>();
            configuration.Services.TryAddTransient<ICommandHandlerContextFactory, CommandHandlerContextFactory>();
            configuration.Services.TryAddTransient<ICommandHandlerActivator, CommandHandlerActivator>();
            configuration.Services.TryAddTransient<ITypeResolver, TopicClientTypeResolver>();
            configuration.Services.TryAddTransient<IMethodResolver, TopicSubscriptionMethodResolver>();
            configuration.Services.TryAddTransient<IAssemblyResolver, AssemblyResolver>();
            configuration.Services.TryAddTransient<IAttributeResolverFactory<Type, TopicClientAttribute>, TypeAttributeResolverFactory>();
            configuration.Services.TryAddTransient<IAttributeResolverFactory<MethodInfo, TopicSubscriptionAttribute>, MethodInfoAttributeResolverFactory>();
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
                    _topicSubscriptionRegistry?.Dispose();
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