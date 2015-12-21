namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    using Application.Interfaces.Logging;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    public class AzureServiceBusInitialiser : IServiceBusInitialiser
    {
        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;
        private readonly ITopicNameFormatter _topicNameFormatter;

        public AzureServiceBusInitialiser(
            ILogService logService,
            IConfigurationService configurationService, ITopicNameFormatter topicNameFormatter)
        {
            _logService = logService;
            _configurationService = configurationService;
            _topicNameFormatter = topicNameFormatter;
        }

        public void Initialise()
        {
            _logService.Debug("Initialising...");

            var configuration = _configurationService.Get<AzureServiceBusConfiguration>();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(configuration.ConnectionString);

            foreach (var topic in configuration.Topics)
            {
                var topicName = _topicNameFormatter.GetTopicName(topic.TopicName);
                if (!namespaceManager.TopicExists(topicName))
                {
                    var topicDescription = new TopicDescription(topicName);

                    try
                    {
                        _logService.Info("Creating topic '{0}'", topicName);

                        namespaceManager.CreateTopic(topicDescription);

                        _logService.Info("Created topic '{0}'", topicName);
                    }
                    catch (MessagingEntityAlreadyExistsException)
                    {
                        _logService.Info("Topic already exists '{0}'", topicName);
                    }
                }

                foreach (var subscription in topic.Subscriptions)
                {
                    if (!namespaceManager.SubscriptionExists(topicName, subscription.SubscriptionName))
                    {
                        try
                        {
                            _logService.Info("Creating subscription '{0}/{1}'", topicName, subscription.SubscriptionName);

                            namespaceManager.CreateSubscription(topicName, subscription.SubscriptionName);

                            _logService.Info("Created subscription '{0}/{1}'", topicName, subscription.SubscriptionName);
                        }
                        catch (MessagingEntityAlreadyExistsException)
                        {
                            _logService.Info("Subscription already exists '{0}/{1}'", topicName, subscription.SubscriptionName);
                        }
                    }
                }
            }

            _logService.Debug("Initialised");
        }
    }
}
