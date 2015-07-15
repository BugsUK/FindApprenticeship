namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using Application.Interfaces.Logging;
    using Azure.ServiceBus.Configuration;
    using Domain.Interfaces.Configuration;
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

                    _logger.Info("Limits for topic/subscription {0} are {1} active and {2} dead-lettered messages",
                        subscriptionPath, messageCountWarningLimit, deadLetterMessageCountWarningLimit);

                    var messageCountDetails = GetMessageCountDetails(
                        namespaceManager, topicConfiguration.TopicName, subscriptionConfiguration.SubscriptionName);

                    var messageCountNarrative = string.Format("Found {0} active and {1} dead-lettered message(s) for topic/subscription: {2}",
                            messageCountDetails.ActiveMessageCount, messageCountDetails.DeadLetterMessageCount, subscriptionPath);

                    if (messageCountDetails.ActiveMessageCount >= messageCountWarningLimit ||
                        messageCountDetails.DeadLetterMessageCount >= deadLetterMessageCountWarningLimit)
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

        private static MessageCountDetails GetMessageCountDetails(
            NamespaceManager namespaceManager, string topicName, string subscriptionName)
        {
            var subscriptionDescription = namespaceManager.GetSubscription(topicName, subscriptionName);

            return subscriptionDescription.MessageCountDetails;
        }

        #endregion
    }
}
