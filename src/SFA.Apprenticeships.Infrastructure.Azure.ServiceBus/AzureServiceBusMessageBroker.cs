namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SFA.Infrastructure.Interfaces;
    using Configuration;
    using Domain.Interfaces.Messaging;
    using Factory;
    using Microsoft.ServiceBus.Messaging;
    using Model;
    using Newtonsoft.Json;

    public class AzureServiceBusMessageBroker<TMessage> : IServiceBusMessageBroker<TMessage>
        where TMessage : class
    {
        private const string ConsumeMethodName = "Consume";

        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;
        private readonly IClientFactory _clientFactory;
        private readonly ITopicNameFormatter _topicNameFormatter;

        private readonly IList<IServiceBusSubscriber<TMessage>> _subscribers;
        private readonly IList<SubscriberInfo> _subscriptionInfos;

        private class SubscriberInfo
        {
            public string SubscriptionPath { get; set; }

            public ITopicClient TopicClient { get; set; }

            public ISubscriptionClient SubscriptionClient { get; set; }

            public IServiceBusSubscriber<TMessage> Subscriber { get; set; }
        }

        public AzureServiceBusMessageBroker(
            ILogService logService,
            IConfigurationService configurationService,
            IEnumerable<IServiceBusSubscriber<TMessage>> subscribers,
            IClientFactory clientFactory,
            ITopicNameFormatter topicNameFormatter)
        {
            _logService = logService;
            _configurationService = configurationService;
            _clientFactory = clientFactory;
            _topicNameFormatter = topicNameFormatter;
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
                    "No subscriber topic/subscription attribute specified for type '{0}'", typeof(TMessage).FullName));
            }

            var serviceBusConfiguration = _configurationService.Get<AzureServiceBusConfiguration>();

            var subscriptionConfiguration = serviceBusConfiguration.Topics
                .Where(each => each.TopicName == topicName)
                .SelectMany(each => each.Subscriptions)
                .FirstOrDefault(each => each.SubscriptionName == subscriptionName);

            if (subscriptionConfiguration == null)
            {
                _logService.Info(
                    "No subscriber configuration found for topic/subscription \"{0}/{1}\", will use default configuration",
                    topicName, subscriptionName);

                subscriptionConfiguration = new AzureServiceBusSubscriptionConfiguration();
            }

            //Ensure topic name is correctly formatted after its configuration has been retrieved
            topicName = _topicNameFormatter.GetTopicName(topicName);

            var subscriptionPath = string.Format("{0}/{1}", topicName, subscriptionConfiguration.SubscriptionName);

            _logService.Info("Subscribing to topic/subscription '{0}'", subscriptionPath);

            var topicClient = _clientFactory.CreateFromConnectionString(
                serviceBusConfiguration.ConnectionString, topicName);

            var options = new OnMessageOptions
            {
                MaxConcurrentCalls =
                    subscriptionConfiguration.MaxConcurrentMessagesPerNode ??
                    serviceBusConfiguration.DefaultMaxConcurrentMessagesPerNode,
                    AutoComplete = false
            };

            options.ExceptionReceived += LogSubscriptionClientException;

            var subscriptionClient = _clientFactory.CreateFromConnectionString(
                serviceBusConfiguration.ConnectionString,
                topicName,
                subscriptionConfiguration.SubscriptionName,
                ReceiveMode.PeekLock);

            var subscriberInfo = new SubscriberInfo
            {
                SubscriptionPath = subscriptionPath,
                TopicClient = topicClient,
                SubscriptionClient = subscriptionClient,
                Subscriber = subscriber
            };

            subscriptionClient.OnMessage(brokeredMessage => ConsumeMessage(subscriberInfo, brokeredMessage), options);

            _logService.Info("Subscribed to topic/subscription '{0}' with max of {1} concurrent call(s) per node",
                subscriptionPath, options.MaxConcurrentCalls);

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

        private void ConsumeMessage(SubscriberInfo subscriberInfo, IBrokeredMessage brokeredMessage)
        {
            try
            {
                var messageBody = brokeredMessage.GetBody<string>();
                var message = JsonConvert.DeserializeObject<TMessage>(messageBody);

                _logService.Debug("Consuming message id '{0}', topic/subscription '{1}', body '{2}'",
                    brokeredMessage.MessageId, subscriberInfo.SubscriptionPath, messageBody);

                var state = subscriberInfo.Subscriber.Consume(message);

                HandleMessageResult(subscriberInfo, state, brokeredMessage, messageBody);

                _logService.Debug("Consumed message id '{0}', topic/subscription '{1}', body '{2}'",
                    brokeredMessage.MessageId, subscriberInfo.SubscriptionPath, messageBody);
            }
            catch (Exception e)
            {
                var deadLetterReason = string.Format(
                    "Unexpected exception consuming message id '{0}, topic/subscription '{1}': message will be dead-lettered",
                    brokeredMessage.MessageId, subscriberInfo.SubscriptionPath);

                _logService.Error(deadLetterReason, e);

                brokeredMessage.DeadLetter();
            }
        }

        private void HandleMessageResult(SubscriberInfo subscriberInfo, ServiceBusMessageStates state, IBrokeredMessage brokeredMessage, string messageBody)
        {
            _logService.Debug("Handling message id '{0}', topic/subscription '{1}', state '{2}', body '{3}' ",
                brokeredMessage.MessageId, subscriberInfo.SubscriptionPath, state, messageBody);

            switch (state)
            {
                case ServiceBusMessageStates.Complete:
                    brokeredMessage.Complete();
                    break;

                case ServiceBusMessageStates.Abandon:
                    brokeredMessage.Abandon();
                    break;

                case ServiceBusMessageStates.DeadLetter:
                    brokeredMessage.DeadLetter();
                    break;

                case ServiceBusMessageStates.Requeue:
                    var scheduledEnqueueTimeUtc = brokeredMessage.ScheduledEnqueueTimeUtc == DateTime.MinValue ? DateTime.UtcNow.AddSeconds(30) : GetDefaultRequeueDateTimeUtc();

                    var newBrokeredMessage = new BrokeredMessage(messageBody)
                    {
                        ScheduledEnqueueTimeUtc = scheduledEnqueueTimeUtc
                    };

                    subscriberInfo.TopicClient.Send(newBrokeredMessage);
                    brokeredMessage.Complete();
                    break;

                default:
                    var deadLetterReason = string.Format(
                        "Invalid message state '{0}' for message id '{1}', topic/subscription '{2}': message will be dead-lettered",
                        state, brokeredMessage.MessageId, subscriberInfo.SubscriptionPath);

                    _logService.Error(deadLetterReason);

                    brokeredMessage.DeadLetter();
                    break;
            }

            _logService.Debug("Handled message id '{0}', topic/subscription '{1}', state '{2}', body '{3}' ",
                brokeredMessage.MessageId, subscriberInfo.SubscriptionPath, state, messageBody);
        }

        private static DateTime GetDefaultRequeueDateTimeUtc()
        {
            return DateTime.UtcNow.AddMinutes(5);
        }

        private void LogSubscriptionClientException(object sender, ExceptionReceivedEventArgs eventArgs)
        {
            _logService.Error("Subscription client exception, action '{0}'", eventArgs.Exception, eventArgs.Action);
        }

        #endregion
    }
}