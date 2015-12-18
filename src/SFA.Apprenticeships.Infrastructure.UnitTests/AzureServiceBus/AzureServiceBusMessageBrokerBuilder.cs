namespace SFA.Apprenticeships.Infrastructure.UnitTests.AzureServiceBus
{
    using System.Collections.Generic;
    using SFA.Infrastructure.Interfaces;
    using Azure.ServiceBus;
    using Azure.ServiceBus.Configuration;
    using Azure.ServiceBus.Factory;
    using Domain.Interfaces.Messaging;
    using Moq;

    public class AzureServiceBusMessageBrokerBuilder<TMessage> where TMessage : class
    {
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private IEnumerable<IServiceBusSubscriber<TMessage>> _subscribers = new List<IServiceBusSubscriber<TMessage>>();
        private Mock<IClientFactory> _clientFactory = new Mock<IClientFactory>();

        public AzureServiceBusMessageBrokerBuilder()
        {
            _configurationService.Setup(cs => cs.Get<AzureServiceBusConfiguration>())
                .Returns(new AzureServiceBusConfiguration
                {
                    ConnectionString = "Endpoint=sb://test",
                    DefaultMessageCountWarningLimit = 50,
                    DefaultDeadLetterMessageCountWarningLimit = 5,
                    DefaultMaxConcurrentMessagesPerNode = 5,
                    Topics = new[]
                    {
                        new AzureServiceBusTopicConfiguration
                        {
                            TopicName = "CreateCandidate",
                            MessageType = "SFA.Apprenticeships.Application.Candidate.CreateCandidateRequest",
                            Subscriptions = new []
                            {
                                new AzureServiceBusSubscriptionConfiguration
                                {
                                    SubscriptionName = "default",
                                    MaxConcurrentMessagesPerNode = 2
                                }
                            }
                        }
                    }
                });
        }

        public AzureServiceBusMessageBroker<TMessage> Build()
        {
            var broker = new AzureServiceBusMessageBroker<TMessage>(_logService.Object, _configurationService.Object, _subscribers, _clientFactory.Object);
            return broker;
        }

        public AzureServiceBusMessageBrokerBuilder<TMessage> With(IEnumerable<IServiceBusSubscriber<TMessage>> subscribers)
        {
            _subscribers = subscribers;
            return this;
        }

        public AzureServiceBusMessageBrokerBuilder<TMessage> With(Mock<IClientFactory> clientFactory)
        {
            _clientFactory = clientFactory;
            return this;
        }
    }
}