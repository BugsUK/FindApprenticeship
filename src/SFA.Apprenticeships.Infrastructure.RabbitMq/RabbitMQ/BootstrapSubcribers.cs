﻿namespace SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ
{
    using System.Reflection;
    using Application.Interfaces.Logging;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using EasyNetQ;
    using EasyNetQ.AutoSubscribe;
    using Interfaces;
    using IContainer = StructureMap.IContainer;

    internal class BootstrapSubcribers : IBootstrapSubcribers
    {
        private readonly IBus _bus;
        private readonly ILogService _logService;
        private readonly RabbitHost _defaultHostConfiguration;

        public BootstrapSubcribers(IBus bus, IConfigurationService configurationService, ILogService logService)
        {
            _defaultHostConfiguration = configurationService.Get<RabbitConfiguration>(RabbitConfiguration.RabbitConfigurationName).MessagingHost;
            _bus = bus;
            _logService = logService;
        }

        public void LoadSubscribers(Assembly assembly, string subscriptionId, IContainer container)
        {
            _logService.Debug("Loading Subscribers from assembly: {0}, subscriptionId: {1}, container: {2}", assembly, subscriptionId, container);

            var autosubscriber = new AutoSubscriber(_bus, subscriptionId)
            {
                ConfigureSubscriptionConfiguration = configuration => configuration.WithPrefetchCount(_defaultHostConfiguration.PreFetchCount),
                AutoSubscriberMessageDispatcher = new StructureMapMessageDispatcher(container)
            };

            autosubscriber.Subscribe(assembly);
            autosubscriber.SubscribeAsync(assembly);

            _logService.Debug("Loaded and Subscribed Subscribers from assembly: {0}, subscriptionId: {1}, container: {2}", assembly, subscriptionId, container);
        }
    }
}
