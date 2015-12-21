namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;

    public class AzureServiceBus : IServiceBus
    {
        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;
        private readonly ITopicNameFormatter _topicNameFormatter;

        private IDictionary<string, TopicClient> _topicClients;
        private readonly object _locker = new object();

        public AzureServiceBus(
            ILogService logService,
            IConfigurationService configurationService, ITopicNameFormatter topicNameFormatter)
        {
            _logService = logService;
            _configurationService = configurationService;
            _topicNameFormatter = topicNameFormatter;
        }

        public void PublishMessage<T>(T message) where T : class
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            EnsureTopicClientsCreated();

            var messageTypeName = message.GetType().FullName;

            if (!_topicClients.ContainsKey(messageTypeName))
            {
                _logService.Error("Failed to get service bus topic client for message type \"{0}\".", messageTypeName);
                return;
            }

            var brokeredMessage = GetBrokeredMessage(message);

            _topicClients[messageTypeName].Send(brokeredMessage);
        }

        public int PublishMessages<T>(IEnumerable<T> messages) where T : class
        {
            if (messages == null)
            {
                throw new ArgumentNullException("messages");
            }

            EnsureTopicClientsCreated();

            var messageTypeName = typeof(T).FullName;

            if (!_topicClients.ContainsKey(messageTypeName))
            {
                _logService.Error("Failed to get service bus topic client for message type \"{0}\".", messageTypeName);
                return 0;
            }

            var count = 0;
            var brokeredMessageBatch = Batch(messages.Select(GetBrokeredMessage), 100);
            foreach (var batch in brokeredMessageBatch)
            {
                _topicClients[messageTypeName].SendBatch(batch);
                count += batch.Count;
            }

            return count;
        }

        #region Helpers

        private void EnsureTopicClientsCreated()
        {
            if (_topicClients != null) return;

            lock (_locker)
            {
                if (_topicClients != null) return;

                var topicClients = new Dictionary<string, TopicClient>();
                var configuration = _configurationService.Get<AzureServiceBusConfiguration>();

                foreach (var topic in configuration.Topics)
                {
                    var topicName = _topicNameFormatter.GetTopicName(topic.TopicName);
                    var topicClient = TopicClient.CreateFromConnectionString(configuration.ConnectionString, topicName);

                    topicClients.Add(topic.MessageType, topicClient);
                }

                _topicClients = topicClients;
            }
        }

        private static BrokeredMessage GetBrokeredMessage<T>(T message) where T : class
        {
            var json = JsonConvert.SerializeObject(message);
            var brokeredMessage = new BrokeredMessage(json)
            {
                ContentType = "application/json"
            };
            return brokeredMessage;
        }

        private static IEnumerable<IList<TSource>> Batch<TSource>(IEnumerable<TSource> source, int size)
        {
            IList<TSource> batch = null;
            var count = 0;

            foreach (var item in source)
            {
                if (batch == null)
                    batch = new List<TSource>(size);

                batch.Add(item);
                count++;

                if (count != size)
                    continue;

                yield return batch;

                batch = null;
                count = 0;
            }

            if (batch != null && count > 0)
                yield return batch;
        }

        #endregion
    }
}