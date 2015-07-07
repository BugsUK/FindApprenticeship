// TODO: AG: review all logging.
// TODO: AG: review all throws.
// TODO: AG: review all try / catch.
// TODO: do we need events?

namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    using System;
    using System.Collections;
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

            public object Subscriber { get; set; }

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
            _logService.Debug("Initialising...");

            var configuration = _configurationService.Get<AzureServiceBusConfiguration>();
            var namespaceManager = NamespaceManager.CreateFromConnectionString(configuration.ConnectionString);

            foreach (var topic in configuration.Topics)
            {
                if (!namespaceManager.TopicExists(topic.TopicName))
                {
                    var topicDescription = new TopicDescription(topic.TopicName);

                    _logService.Info("Creating topic '{0}'", topic.TopicName);
                    namespaceManager.CreateTopic(topicDescription);
                }

                foreach (var subscription in topic.Subscriptions)
                {
                    if (!namespaceManager.SubscriptionExists(topic.TopicName, subscription.SubscriptionName))
                    {
                        _logService.Info("Creating subscription '{0}/{1}'", topic.TopicName, subscription.SubscriptionName);
                        namespaceManager.CreateSubscription(topic.TopicName, subscription.SubscriptionName);
                    }
                }
            }

            _logService.Debug("Initialised");
        }

        public void Subscribe()
        {
            Task.Run(() =>
            {
                _logService.Info("Subscribing...");

                var serviceBusConfiguration = _configurationService.Get<AzureServiceBusConfiguration>();

                foreach (var topicConfiguration in serviceBusConfiguration.Topics)
                {
                    var topicClient = TopicClient.CreateFromConnectionString(
                        serviceBusConfiguration.ConnectionString, topicConfiguration.TopicName);

                    foreach (var subscriptionConfiguration in topicConfiguration.Subscriptions)
                    {
                        var messageType = GetMessageType(topicConfiguration);
                        var subscribers = GetMessageTypeSubscribers(messageType);
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

                            _logService.Info("Subscribing to topic/subscription '{0}'", subscriberInfo.Path);

                            subscriptionClient.OnMessageAsync(brokeredMessage => Task.Run(() =>
                                ConsumeMessage(subscriberInfo, brokeredMessage)),
                                options);

                            _subscriberInfos.Add(subscriberInfo);
                        
                            _logService.Info("Subscribed to topic/subscription '{0}'", subscriberInfo.Path);
                        }

                        if (!foundSubscriber)
                        {
                            throw new InvalidOperationException(string.Format(
                                "No subscribers found for topic/subscription '{0}'.", subscriptionPath));
                        }
                    }
                }

                _logService.Info("Subscribed");
                _completedEvent.WaitOne();
            });
        }

        private void ConsumeMessage(SubscriberInfo subscriberInfo, BrokeredMessage brokeredMessage)
        {
            try
            {
                var messageBody = brokeredMessage.GetBody<string>();
                var message = JsonConvert.DeserializeObject(messageBody, subscriberInfo.MessageType);

                _logService.Debug("Consuming message id '{0}', topic/subscription '{1}', body '{2}'",
                    brokeredMessage.MessageId, subscriberInfo.Path, messageBody);

                var result = subscriberInfo.ConsumeMethod.Invoke(subscriberInfo.Subscriber, new[]
                {
                    message
                }) as ServiceBusMessageResult;

                HandleMessageResult(subscriberInfo, result, brokeredMessage, messageBody);

                _logService.Debug("Consumed message id '{0}', topic/subscription '{1}', body '{2}'",
                    brokeredMessage.MessageId, subscriberInfo.Path, messageBody);
            }
            catch (Exception e)
            {
                _logService.Error(
                    "Unexpected exception consuming message id '{0}, topic/subscription '{1}': message will be dead-lettered",
                    e, brokeredMessage.MessageId, subscriberInfo.Path);

                brokeredMessage.DeadLetter();
            }
        }

        private void HandleMessageResult(SubscriberInfo subscriberInfo, ServiceBusMessageResult result, BrokeredMessage brokeredMessage, string messageBody)
        {
            if (result == null)
            {
                _logService.Warn("No message result for message id '{0}', topic/subscription '{1}': message will be dead-lettered",
                    brokeredMessage.MessageId, subscriberInfo.Path);

                brokeredMessage.DeadLetter();
                return;
            }

            _logService.Debug("Handling message id '{0}', topic/subscription '{1}', state '{2}', body '{3}' ",
                brokeredMessage.MessageId, subscriberInfo.Path, result.State, messageBody);

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
                        result.State, brokeredMessage.MessageId, subscriberInfo.Path);

                    brokeredMessage.DeadLetter();
                    break;
            }

            _logService.Debug("Handled message id '{0}', topic/subscription '{1}', state '{2}', body '{3}' ",
                brokeredMessage.MessageId, subscriberInfo.Path, result.State, messageBody);
        }

        public void Unsubscribe()
        {
            _logService.Info("Unsubscribing...");

            _completedEvent.Set();

            foreach (var subscriberInfo in _subscriberInfos)
            {
                try
                {
                    _logService.Info("Unsubscribing from topic/subscription '{0}'...", subscriberInfo.Path);

                    subscriberInfo.TopicClient.Close();
                    subscriberInfo.SubscriptionClient.Close();

                    _logService.Info("Unsubscribed from topic/subscription '{0}'...", subscriberInfo.Path);
                }
                catch
                {
                    _logService.Warn("Failed to unsubscribe from topic/subscription: '{0}'", subscriberInfo.Path);
                }
            }

            _logService.Info("Unsubscribed");
        }

        #region Helpers

        private static MethodInfo GetConsumeMethod(object subscriber, string topicName, string subscriptionName)
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

        private IEnumerable GetMessageTypeSubscribers(Type messageType)
        {
            return _container.GetAllInstances(typeof(IServiceBusSubscriber<>).MakeGenericType(messageType));
        }

        private static DateTime GetDefaultReqeueDateTimeUtc()
        {
            return DateTime.UtcNow.AddMinutes(5);
        }

        #endregion
    }
}