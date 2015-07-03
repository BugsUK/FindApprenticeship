namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using IContainer = StructureMap.IContainer;

    public class AzureServiceBusManager : IServiceBusManager
    {
        private readonly IContainer _container;
        private readonly IConfigurationService _configurationService;
        
        private readonly ManualResetEvent _completedEvent;
        private readonly IList<SubscriptionClient> _subscriptionClients;

        public AzureServiceBusManager(
            IContainer container,
            IConfigurationService configurationService)
        {
            _container = container;
            _configurationService = configurationService;
            _completedEvent = new ManualResetEvent(false);
            _subscriptionClients = new List<SubscriptionClient>();
        }

        public void Initialise()
        {
            CreateTopicsAndSubscriptions();
        }

        public void Subscribe()
        {
            Task.Run(() =>
            {
                var configuration = _configurationService.Get<AzureServiceBusConfiguration>();

                foreach (var topic in configuration.Topics)
                {
                    var topicClient = TopicClient.CreateFromConnectionString(
                        configuration.ConnectionString, topic.TopicName);

                    foreach (var subscription in topic.Subscriptions)
                    {
                        var messageType = Type.GetType(topic.MessageType);

                        if (messageType == null)
                        {
                            throw new InvalidOperationException(string.Format(
                                "Invalid message type: \"{0}\"", topic.MessageType));
                        }

                        var genericSubscriberType = typeof(IServiceBusSubscriber<>).MakeGenericType(messageType);
                        var subscribers = _container.GetAllInstances(genericSubscriberType);

                        var subscriberCount = 0;

                        foreach (var subscriber in subscribers)
                        {
                            var consumeMethod = subscriber.GetType().GetMethod("Consume");

                            var subscriptionAttribute = consumeMethod
                                .GetCustomAttributes(typeof(ServiceBusTopicSubscriptionAttribute), true)
                                .SingleOrDefault() as ServiceBusTopicSubscriptionAttribute;

                            if (subscriptionAttribute == null ||
                                subscriptionAttribute.TopicName != topic.TopicName ||
                                subscriptionAttribute.SubscriptionName != subscription.SubscriptionName)
                            {
                                continue;
                            }

                            if (subscriberCount > 0)
                            {
                                throw new InvalidOperationException(string.Format(
                                    "Expected only one subscriber found for topic/subscription \"{0}/{1}\"",
                                    topic.TopicName,
                                    subscription.SubscriptionName));
                            }

                            var options = new OnMessageOptions
                            {
                                MaxConcurrentCalls =
                                    subscription.MaxConcurrentMessagesPerNode ??
                                    configuration.DefaultMaxConcurrentMessagesPerNode,
                                AutoComplete = true                                
                            };

                            var subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                                configuration.ConnectionString,
                                topic.TopicName,
                                subscription.SubscriptionName,
                                ReceiveMode.ReceiveAndDelete);

                            subscriptionClient.OnMessageAsync(brokeredMessage => Task.Run(() =>
                            {
                                try
                                {
                                    var json = brokeredMessage.GetBody<string>();
                                    var message = JsonConvert.DeserializeObject(json, messageType);

                                    var result = consumeMethod.Invoke(subscriber, new[]
                                    {
                                        message
                                    }) as ServiceBusMessageResult;

                                    if (result == null)
                                    {
                                        // TODO: AG: log.
                                        brokeredMessage.Abandon();
                                        return;
                                    }

                                    switch (result.State)
                                    {
                                        case ServiceBusMessageStates.Complete:
                                            // Nothing to do.
                                            Console.WriteLine("-> Complete");
                                            break;

                                        case ServiceBusMessageStates.Abandon:
                                            // brokeredMessage.Abandon();

                                            Console.WriteLine("-> Abandon");
                                            break;

                                        case ServiceBusMessageStates.DeadLetter:
                                            brokeredMessage.DeadLetter();
                                            Console.WriteLine("-> DeadLetter");
                                            break;

                                        case ServiceBusMessageStates.Requeue:
                                            var newBrokeredMessage = new BrokeredMessage(json)
                                            {
                                                ScheduledEnqueueTimeUtc = result.RequeueDateTimeUtc ?? GetDefaultReqeueDateTimeUtc()
                                            };

                                            topicClient.SendAsync(newBrokeredMessage);
                                            Console.WriteLine("-> Requeue");
                                            break;

                                        default:
                                            // TODO: log.
                                            Console.WriteLine("Invalid service bus message state: \"{0}\", message will be abandoned.", result.State);
                                            break;
                                    }
                                }
                                catch (Exception)
                                {
                                    // TODO: log.
                                    // TODO: handle retries, deferrals etc.
                                    brokeredMessage.DeadLetter();
                                }
                            }), options);

                            _subscriptionClients.Add(subscriptionClient);
                            subscriberCount++;
                        }

                        if (subscriberCount == 0)
                        {
                            throw new InvalidOperationException(string.Format(
                                "No subscribers found for topic/subscription \"{0}/{1}\"",
                                topic.TopicName,
                                subscription.SubscriptionName));
                        }
                    }
                }

                _completedEvent.WaitOne();
            });
        }

        public void Unsubscribe()
        {
            Console.WriteLine("Unsubscribing...");

            foreach (var subscriptionClient in _subscriptionClients)
            {
                try
                {
                    subscriptionClient.Close();
                }
                catch
                {
                    // TODO: AG: log.
                    Console.WriteLine("Failed to close subscription client.");
                }
            }

            _completedEvent.Set();
            Console.WriteLine("...done");
        }

        #region Helpers

        private void CreateTopicsAndSubscriptions()
        {
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

        private DateTime GetDefaultReqeueDateTimeUtc()
        {
            // TODO: review default, get from configuration.
            return DateTime.UtcNow.AddMinutes(5);
        }

        #endregion
    }
}