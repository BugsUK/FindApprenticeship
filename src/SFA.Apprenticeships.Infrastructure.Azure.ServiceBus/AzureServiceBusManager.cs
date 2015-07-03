namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    using System;
    using System.Threading.Tasks;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using StructureMap;

    public class AzureServiceBusManager : IServiceBusManager
    {
        private readonly IContainer _container;
        private readonly IConfigurationService _configurationService;

        public AzureServiceBusManager(
            IContainer container,
            IConfigurationService configurationService)
        {
            _container = container;
            _configurationService = configurationService;
        }

        public void Initialise()
        {
            CreateTopicsAndSubscriptions();
        }

        public void Subscribe()
        {
            var configuration = _configurationService.Get<AzureServiceBusConfiguration>();

            foreach (var topic in configuration.Topics)
            {
                foreach (var subscription in topic.Subscriptions)
                {
                    var subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                        configuration.ConnectionString, topic.TopicName, subscription.SubscriptionName);

                    var messageType = Type.GetType(topic.MessageType);

                    if (messageType == null)
                    {
                        // TODO: AG: log.
                        return;
                    }

                    var subscriberType = typeof(IServiceBusSubscriber<>).MakeGenericType(messageType);
                    var subscribers = _container.GetAllInstances(subscriberType);
                    var methodInfo = subscriberType.GetMethod("Consume");

                    var options = new OnMessageOptions
                    {
                        MaxConcurrentCalls = subscription.MaxConcurrentMessagesPerNode ?? configuration.DefaultMaxConcurrentMessagesPerNode,
                        AutoComplete = true
                    };

                    subscriptionClient.OnMessageAsync(brokeredMessage => Task.Run(() =>
                    {
                        try
                        {
                            var json = brokeredMessage.GetBody<string>();
                            var message = JsonConvert.DeserializeObject(json, messageType);

                            foreach (var subscriber in subscribers)
                            {
                                methodInfo.Invoke(subscriber, new[] { message });
                            }
                        }
                        catch (Exception)
                        {
                            // TODO: log.
                            // TODO: handle retries, deferrals etc.
                            brokeredMessage.Abandon();
                        }
                    }), options);
                }
            }
        }

        #region Helpers

        private void CreateTopicsAndSubscriptions()
        {
            var configuration = _configurationService.Get<AzureServiceBusConfiguration>();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(configuration.ConnectionString);

            foreach (var topic in configuration.Topics)
            {
                if (!namespaceManager.TopicExists(topic.TopicName))
                {
                    var topicDescription = new TopicDescription(topic.TopicName);

                    namespaceManager.CreateTopic(topicDescription);
                }

                foreach (var subscription in topic.Subscriptions)
                {
                    if (!namespaceManager.SubscriptionExists(topic.TopicName, subscription.SubscriptionName))
                    {
                        namespaceManager.CreateSubscription(topic.TopicName, subscription.SubscriptionName);
                    }
                }
            }
        }

        #endregion
    }
}