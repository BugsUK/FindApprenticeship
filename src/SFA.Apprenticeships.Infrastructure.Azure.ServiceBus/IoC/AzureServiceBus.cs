namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.IoC
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;

    public class AzureServiceBus : IServiceBus
    {
        private readonly IConfigurationService _configurationService;
        private IDictionary<string, TopicClient> _topicClients;
        private readonly object _locker = new object();

        public AzureServiceBus(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void PublishMessage<T>(T message) where T : class
        {
            EnsureTopicClientsCreated();

            var messageType = message.GetType().FullName;
            TopicClient topicClient;

            if (!_topicClients.TryGetValue(messageType, out topicClient))
            {
                // TODO: AG: log.
                return;
            }
            
            var json = JsonConvert.SerializeObject(message);
            var brokeredMessage = new BrokeredMessage(json)
            {
                ContentType = "application/json"
            };

            topicClient.SendAsync(brokeredMessage);
        }

        #region Helpers

        private void EnsureTopicClientsCreated()
        {
            if (_topicClients != null) return;

            // TODO: AG: review locking.
            lock (_locker)
            {
                if (_topicClients != null) return;

                var topicClients = new Dictionary<string, TopicClient>();
                var configuration = _configurationService.Get<AzureServiceBusConfiguration>();

                foreach (var topic in configuration.Topics)
                {
                    var topicClient = TopicClient.CreateFromConnectionString(configuration.ConnectionString, topic.TopicName);

                    // TODO: AG: extract message type name from bit before comma.
                    topicClients.Add(GetTypeFullName(topic.MessageType), topicClient);
                }

                _topicClients = topicClients;
            }
        }

        private static string GetTypeFullName(string messageType)
        {
            if (string.IsNullOrWhiteSpace(messageType))
            {
                throw new ArgumentNullException("messageType");
            }

            var parts = messageType.Split(',');

            if (parts.Length == 2)
            {
                if (!string.IsNullOrWhiteSpace(parts[0]))
                {
                    return parts[0];
                }
            }

            throw new InvalidOperationException(string.Format(
                "Cannot message type name from string \"{0}\".", messageType));
        }

        #endregion
    }
}