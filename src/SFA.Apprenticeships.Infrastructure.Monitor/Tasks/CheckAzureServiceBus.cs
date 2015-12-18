namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using SFA.Infrastructure.Interfaces;
    using Azure.ServiceBus.Configuration;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    public class CheckAzureServiceBus : IMonitorTask
    {
        private readonly ILogService _logger;
        private readonly IConfigurationService _configurationService;

        public CheckAzureServiceBus(
            ILogService logger,
            IConfigurationService configurationService)
        {
            _logger = logger;
            _configurationService = configurationService;
        }

        public void Run()
        {
            CheckAzureServiceBusMessageCounts();
        }

        public string TaskName
        {
            get
            {
                return "Check Azure Service Bus";
            }
        }

        #region Helpers

        private void CheckAzureServiceBusMessageCounts()
        {
            _logger.Info("Checking Azure Service Bus Message counts");

            var serviceBusConfiguration = _configurationService.Get<AzureServiceBusConfiguration>();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConfiguration.ConnectionString);

            foreach (var topicConfiguration in serviceBusConfiguration.Topics)
            {
                var topicMessageCountDetails = GetTopicMessageCountDetails(
                    namespaceManager, topicConfiguration.TopicName);

                foreach (var subscriptionConfiguration in topicConfiguration.Subscriptions)
                {
                    var subscriptionPath = string.Format("{0}/{1}",
                        topicConfiguration.TopicName, subscriptionConfiguration.SubscriptionName);

                    var messageCountWarningLimit =
                        subscriptionConfiguration.MessageCountWarningLimit ??
                        serviceBusConfiguration.DefaultMessageCountWarningLimit;

                    var deadLetterMessageCountWarningLimit =
                        subscriptionConfiguration.DeadLetterMessageCountWarningLimit ??
                        serviceBusConfiguration.DefaultDeadLetterMessageCountWarningLimit;

                    _logger.Debug("Limits for topic/subscription {0} are {1} active and {2} dead-lettered messages",
                        subscriptionPath, messageCountWarningLimit, deadLetterMessageCountWarningLimit);

                    var subscriptionMessageCountDetails = GetSubscriptionMessageCountDetails(
                        namespaceManager, topicConfiguration.TopicName, subscriptionConfiguration.SubscriptionName);

                    var messageCountNarrative = string.Format("Found {0} active, {1} scheduled and {2} dead-lettered message(s) for topic/subscription: {3}",
                        subscriptionMessageCountDetails.ActiveMessageCount,
                        topicMessageCountDetails.ScheduledMessageCount,
                        subscriptionMessageCountDetails.DeadLetterMessageCount,
                        subscriptionPath);

                    if (subscriptionMessageCountDetails.ActiveMessageCount >= messageCountWarningLimit ||
                        subscriptionMessageCountDetails.DeadLetterMessageCount >= deadLetterMessageCountWarningLimit)
                    {
                        _logger.Warn(messageCountNarrative);
                    }
                    else
                    {
                        _logger.Info(messageCountNarrative);
                    }
                }
            }

            _logger.Info("Checked Azure Service Bus Message counts");
        }

        private static MessageCountDetails GetSubscriptionMessageCountDetails(
            NamespaceManager namespaceManager, string topicName, string subscriptionName)
        {
            var subscriptionDescription = namespaceManager.GetSubscription(topicName, subscriptionName);

            return subscriptionDescription.MessageCountDetails;
        }

        private static MessageCountDetails GetTopicMessageCountDetails(
            NamespaceManager namespaceManager, string topicName)
        {
            var topicDescription = namespaceManager.GetTopic(topicName);

            return topicDescription.MessageCountDetails;
        }

        #endregion
    }
}
