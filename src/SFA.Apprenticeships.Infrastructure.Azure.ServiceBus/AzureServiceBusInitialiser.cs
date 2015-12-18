namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    using SFA.Infrastructure.Interfaces;
    using Configuration;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    public class AzureServiceBusInitialiser : IServiceBusInitialiser
    {
        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;

        public AzureServiceBusInitialiser(
            ILogService logService,
            IConfigurationService configurationService)
        {
            _logService = logService;
            _configurationService = configurationService;
        }

        public void Initialise()
        {
            _logService.Debug("Initialising...");

            var configuration = _configurationService.Get<AzureServiceBusConfiguration>();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(configuration.ConnectionString);

            foreach (var topic in configuration.Topics)
            {
                if (!namespaceManager.TopicExists(topic.TopicName))
                {
                    var topicDescription = new TopicDescription(topic.TopicName);

                    try
                    {
                        _logService.Info("Creating topic '{0}'", topic.TopicName);

                        namespaceManager.CreateTopic(topicDescription);

                        _logService.Info("Created topic '{0}'", topic.TopicName);
                    }
                    catch (MessagingEntityAlreadyExistsException)
                    {
                        _logService.Info("Topic already exists '{0}'", topic.TopicName);
                    }
                }

                foreach (var subscription in topic.Subscriptions)
                {
                    if (!namespaceManager.SubscriptionExists(topic.TopicName, subscription.SubscriptionName))
                    {
                        try
                        {
                            _logService.Info("Creating subscription '{0}/{1}'", topic.TopicName, subscription.SubscriptionName);

                            namespaceManager.CreateSubscription(topic.TopicName, subscription.SubscriptionName);

                            _logService.Info("Created subscription '{0}/{1}'", topic.TopicName, subscription.SubscriptionName);
                        }
                        catch (MessagingEntityAlreadyExistsException)
                        {
                            _logService.Info("Subscription already exists '{0}/{1}'", topic.TopicName, subscription.SubscriptionName);
                        }
                    }
                }
            }

            _logService.Debug("Initialised");
        }
    }
}
