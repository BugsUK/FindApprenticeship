namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;

    public class AzureServiceBusMessageBroker<TMessage> : IServiceBusMessageBroker<TMessage>
        where TMessage : class
    {
        private const string ConsumeMethodName = "Consume";

        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;

        private readonly IList<IServiceBusSubscriber<TMessage>> _subscribers;
        private readonly IList<SubscriberInfo> _subscriptionInfos;

        private class SubscriberInfo
        {
            public string SubscriptionPath { get; set; }

            public TopicClient TopicClient { get; set; }

            public SubscriptionClient SubscriptionClient { get; set; }

            public IServiceBusSubscriber<TMessage> Subscriber { get; set; }
        }

        public AzureServiceBusMessageBroker(
            ILogService logService,
            IConfigurationService configurationService,
            IEnumerable<IServiceBusSubscriber<TMessage>> subscribers)
        {
            _logService = logService;
            _configurationService = configurationService;
            _subscribers = subscribers.ToList();
            _subscriptionInfos = new List<SubscriberInfo>();
        }

        public void Subscribe()
        {
            if (_subscribers.Count == 0)
            {
                throw new InvalidOperationException(string.Format(
                    "No subscribers specified for type '{0}'", typeof(TMessage).FullName));
            }

            foreach (var subscriber in _subscribers)
            {
                _subscriptionInfos.Add(SubscribeOne(subscriber));
            }
        }

        private SubscriberInfo SubscribeOne(IServiceBusSubscriber<TMessage> subscriber)
        {
            string topicName;
            string subscriptionName;

            if (!TryGetSubscriberTopicSubscriptionNames(subscriber, out topicName, out subscriptionName))
            {
                throw new InvalidOperationException(string.Format(
                    "No subscriber topic/subscription attribute specified for type '{0}'", typeof (TMessage).FullName));
            }

            var serviceBusConfiguration = _configurationService.Get<AzureServiceBusConfiguration>();

            var subscriptionConfiguration = serviceBusConfiguration.Topics
                .Where(each => each.TopicName == topicName)
                .SelectMany(each => each.Subscriptions)
                .FirstOrDefault(each => each.SubscriptionName == subscriptionName);

            if (subscriptionConfiguration == null)
            {
                throw new InvalidOperationException(string.Format(
                    "No subscriber configuration found for topic/subscription \"{0}/{1}\"", topicName, subscriptionName));
            }

            var topicClient = TopicClient.CreateFromConnectionString(
                serviceBusConfiguration.ConnectionString, topicName);

            var subscriptionPath = string.Format("{0}/{1}", topicName, subscriptionName);

            var options = new OnMessageOptions
            {
                MaxConcurrentCalls =
                    subscriptionConfiguration.MaxConcurrentMessagesPerNode ??
                    serviceBusConfiguration.DefaultMaxConcurrentMessagesPerNode,
                AutoComplete = true
            };

            var subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                serviceBusConfiguration.ConnectionString,
                topicName,
                subscriptionName,
                ReceiveMode.PeekLock);

            _logService.Info("Subscribing to topic/subscription '{0}'", subscriptionPath);

            var subscriberInfo = new SubscriberInfo
            {
                SubscriptionPath = subscriptionPath,
                TopicClient = topicClient,
                SubscriptionClient = subscriptionClient,
                Subscriber = subscriber
            };

            subscriptionClient.OnMessageAsync(brokeredMessage => Task.Run(() =>
                ConsumeMessage(subscriberInfo, brokeredMessage)),
                options);

            _logService.Info("Subscribed to topic/subscription '{0}'", subscriptionPath);

            return subscriberInfo;
        }

        public void Unsubscribe()
        {
            foreach (var subscriptionInfo in _subscriptionInfos)
            {
                try
                {
                    _logService.Info("Unsubscribing from topic/subscription '{0}'...", subscriptionInfo.SubscriptionPath);

                    subscriptionInfo.TopicClient.Close();
                    subscriptionInfo.SubscriptionClient.Close();

                    _logService.Info("Unsubscribed from topic/subscription '{0}'...", subscriptionInfo.SubscriptionPath);
                }
                catch
                {
                    _logService.Warn("Failed to unsubscribe from topic/subscription: '{0}'", subscriptionInfo.SubscriptionPath);
                }
            }
        }

        #region Helpers

        private static bool TryGetSubscriberTopicSubscriptionNames(IServiceBusSubscriber<TMessage> subscriber, out string topicName, out string subscriptionName)
        {
            var consumeMethod = subscriber.GetType().GetMethod(ConsumeMethodName);

            var subscriptionAttribute = consumeMethod
                .GetCustomAttributes(typeof(ServiceBusTopicSubscriptionAttribute), false)
                .SingleOrDefault() as ServiceBusTopicSubscriptionAttribute;

            if (subscriptionAttribute == null)
            {
                topicName = null;
                subscriptionName = null;

                return false;
            }

            topicName = subscriptionAttribute.TopicName;
            subscriptionName = subscriptionAttribute.SubscriptionName;

            return true;
        }

        private void ConsumeMessage(SubscriberInfo subscriberInfo, BrokeredMessage brokeredMessage)
        {
            try
            {
                var messageBody = brokeredMessage.GetBody<string>();
                var message = JsonConvert.DeserializeObject<TMessage>(messageBody);

                _logService.Debug("Consuming message id '{0}', topic/subscription '{1}', body '{2}'",
                    brokeredMessage.MessageId, subscriberInfo.SubscriptionPath, messageBody);

                var result = subscriberInfo.Subscriber.Consume(message);

                HandleMessageResult(subscriberInfo, result, brokeredMessage, messageBody);

                _logService.Debug("Consumed message id '{0}', topic/subscription '{1}', body '{2}'",
                    brokeredMessage.MessageId, subscriberInfo.SubscriptionPath, messageBody);
            }
            catch (Exception e)
            {
                _logService.Error(
                    "Unexpected exception consuming message id '{0}, topic/subscription '{1}': message will be dead-lettered",
                    e, brokeredMessage.MessageId, subscriberInfo.SubscriptionPath);

                brokeredMessage.DeadLetter();
            }
        }

        private void HandleMessageResult(SubscriberInfo subscriberInfo, ServiceBusMessageResult result, BrokeredMessage brokeredMessage, string messageBody)
        {
            if (result == null)
            {
                _logService.Warn("No message result for message id '{0}', topic/subscription '{1}': message will be dead-lettered",
                    brokeredMessage.MessageId, subscriberInfo.SubscriptionPath);

                brokeredMessage.DeadLetter();
                return;
            }

            _logService.Debug("Handling message id '{0}', topic/subscription '{1}', state '{2}', body '{3}' ",
                brokeredMessage.MessageId, subscriberInfo.SubscriptionPath, result.State, messageBody);

            switch (result.State)
            {
                case ServiceBusMessageStates.Complete:
                    break;

                case ServiceBusMessageStates.Abandon:
                    brokeredMessage.Abandon();
                    break;

                case ServiceBusMessageStates.DeadLetter:
                    brokeredMessage.DeadLetter();
                    break;

                case ServiceBusMessageStates.Requeue:
                    var newBrokeredMessage = new BrokeredMessage(messageBody)
                    {
                        ScheduledEnqueueTimeUtc = result.RequeueDateTimeUtc ?? GetDefaultReqeueDateTimeUtc()
                    };

                    subscriberInfo.TopicClient.Send(newBrokeredMessage);
                    break;

                default:
                    _logService.Error(
                        "Invalid message state '{0}' for message id '{1}', topic/subscription '{2}': message will be dead-lettered",
                        result.State, brokeredMessage.MessageId, subscriberInfo.SubscriptionPath);

                    brokeredMessage.DeadLetter();
                    break;
            }

            _logService.Debug("Handled message id '{0}', topic/subscription '{1}', state '{2}', body '{3}' ",
                brokeredMessage.MessageId, subscriberInfo.SubscriptionPath, result.State, messageBody);
        }

        private static DateTime GetDefaultReqeueDateTimeUtc()
        {
            return DateTime.UtcNow.AddMinutes(5);
        }

        #endregion
    }
}