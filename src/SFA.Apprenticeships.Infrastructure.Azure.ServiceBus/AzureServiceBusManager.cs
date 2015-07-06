// TODO: AG: review all logging.
// TODO: AG: review all throws.
// TODO: AG: review all try / catch.
// TODO: do we need events?

namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using IContainer = StructureMap.IContainer;

    public class AzureServiceBusManager : IServiceBusManager
    {
        private const string ConsumeMethodName = "Consume";

        private readonly IContainer _container;
        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;

        private readonly ManualResetEvent _completedEvent;
        private readonly IList<SubscriberInfo> _subscriberInfos;

        public class SubscriberInfo
        {
            public string Path { get; set; }

            public TopicClient TopicClient { get; set; }

            public SubscriptionClient SubscriptionClient { get; set; }

            public Type MessageType { get; set; }

            public IServiceBusSubscriber Subscriber { get; set; }

            public MethodInfo ConsumeMethod { get; set; }
        }

        public AzureServiceBusManager(
            IContainer container,
            ILogService logService,
            IConfigurationService configurationService)
        {
            _container = container;
            _logService = logService;
            _configurationService = configurationService;
            _completedEvent = new ManualResetEvent(false);
            _subscriberInfos = new List<SubscriberInfo>();
        }

        public void Initialise()
        {
            // TODO: AG: add logging.
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

        public void Subscribe()
        {
            Task.Run(() =>
            {
                var serviceBusConfiguration = _configurationService.Get<AzureServiceBusConfiguration>();

                foreach (var topicConfiguration in serviceBusConfiguration.Topics)
                {
                    var topicClient = TopicClient.CreateFromConnectionString(
                        serviceBusConfiguration.ConnectionString, topicConfiguration.TopicName);

                    foreach (var subscriptionConfiguration in topicConfiguration.Subscriptions)
                    {
                        var messageType = GetMessageType(topicConfiguration);
                        var subscribers = GetSubscribers();
                        var foundSubscriber = false;

                        var subscriptionPath = string.Format("{0}/{1}",
                            topicConfiguration.TopicName, subscriptionConfiguration.SubscriptionName);

                        foreach (var subscriber in subscribers)
                        {
                            var consumeMethod = GetConsumeMethod(
                                subscriber, topicConfiguration.TopicName, subscriptionConfiguration.SubscriptionName);

                            if (consumeMethod == null)
                            {
                                continue;
                            }

                            if (foundSubscriber)
                            {
                                throw new InvalidOperationException(string.Format(
                                    "Expected only one subscriber for topic/subscription '{0}'.", subscriptionPath));
                            }

                            foundSubscriber = true;

                            var options = new OnMessageOptions
                            {
                                MaxConcurrentCalls =
                                    subscriptionConfiguration.MaxConcurrentMessagesPerNode ??
                                    serviceBusConfiguration.DefaultMaxConcurrentMessagesPerNode,
                                AutoComplete = true
                            };

                            var subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                                serviceBusConfiguration.ConnectionString,
                                topicConfiguration.TopicName,
                                subscriptionConfiguration.SubscriptionName,
                                ReceiveMode.PeekLock);

                            var subscriberInfo = new SubscriberInfo
                            {
                                Path = subscriptionPath,
                                TopicClient = topicClient,
                                SubscriptionClient = subscriptionClient,
                                MessageType = messageType,
                                Subscriber = subscriber,
                                ConsumeMethod = consumeMethod
                            };

                            subscriptionClient.OnMessageAsync(brokeredMessage => Task.Run(() =>
                                ConsumeMessage(subscriberInfo, brokeredMessage)),
                                options);

                            _subscriberInfos.Add(subscriberInfo);
                        }

                        if (!foundSubscriber)
                        {
                            throw new InvalidOperationException(string.Format(
                                "No subscribers found for topic/subscription '{0}'.", subscriptionPath));
                        }
                    }
                }

                _completedEvent.WaitOne();
            });
        }

        private void ConsumeMessage(SubscriberInfo subscriberInfo, BrokeredMessage brokeredMessage)
        {
            try
            {
                // TODO: add debug logging.
                var messageBody = brokeredMessage.GetBody<string>();
                var message = JsonConvert.DeserializeObject(messageBody, subscriberInfo.MessageType);

                var result = subscriberInfo.ConsumeMethod.Invoke(subscriberInfo.Subscriber, new[]
                {
                    message
                }) as ServiceBusMessageResult;

                HandleMessageResult(subscriberInfo, result, brokeredMessage, messageBody);
            }
            catch (Exception e)
            {
                _logService.Error(
                    "Unexpected exception consuming message id '{0}', message will be dead-lettered",
                    e, brokeredMessage.MessageId);

                brokeredMessage.DeadLetter();
            }
        }

        private void HandleMessageResult(SubscriberInfo subscriberInfo, ServiceBusMessageResult result, BrokeredMessage brokeredMessage, string messageBody)
        {
            if (result == null)
            {
                _logService.Warn("No message result for message id '{0}', message will be dead-lettered",
                    brokeredMessage.MessageId);

                brokeredMessage.DeadLetter();
                return;
            }

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
                        "Invalid message state '{0}' for message id '{1}', message will be dead-lettered",
                        result.State, brokeredMessage.MessageId);

                    brokeredMessage.DeadLetter();
                    break;
            }
        }

        public void Unsubscribe()
        {
            _logService.Debug("Unsubscribing...");

            _completedEvent.Set();

            foreach (var subscriberInfo in _subscriberInfos)
            {
                try
                {
                    subscriberInfo.TopicClient.Close();
                    subscriberInfo.SubscriptionClient.Close();
                }
                catch
                {
                    _logService.Warn("Failed to close: '{0}'", subscriberInfo.Path);
                }
            }

            _logService.Debug("Unsubscribed");
        }

        #region Helpers

        private static MethodInfo GetConsumeMethod(IServiceBusSubscriber subscriber, string topicName, string subscriptionName)
        {
            var consumeMethod = subscriber.GetType().GetMethod(ConsumeMethodName);

            var subscriptionAttribute = consumeMethod
                .GetCustomAttributes(typeof(ServiceBusTopicSubscriptionAttribute), false)
                .SingleOrDefault() as ServiceBusTopicSubscriptionAttribute;

            if (subscriptionAttribute != null &&
                subscriptionAttribute.TopicName == topicName &&
                subscriptionAttribute.SubscriptionName == subscriptionName)
            {
                return consumeMethod;
            }

            return null;
        }

        private static Type GetMessageType(AzureServiceBusTopicConfiguration topic)
        {
            var messageType = Type.GetType(topic.MessageType);

            if (messageType == null)
            {
                throw new InvalidOperationException(string.Format(
                    "Invalid message type: '{0}'.", topic.MessageType));
            }
            return messageType;
        }

        private IEnumerable<IServiceBusSubscriber> GetSubscribers()
        {
            return _container.GetAllInstances<IServiceBusSubscriber>();
        }

        private static DateTime GetDefaultReqeueDateTimeUtc()
        {
            // TODO: review default, get from configuration.
            return DateTime.UtcNow.AddMinutes(5);
        }

        #endregion
    }
}