namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.AzureServiceBus.Configuration
{
    using Azure.ServiceBus.Configuration;
    using Common.IoC;
    using SFA.Infrastructure.Interfaces;
    using FluentAssertions;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture, Category("Integration")]
    public class AzureServiceBusConfigurationTests
    {
        [Test, Category("Integration")]
        public void LoadConfigurationFromDatabase()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationService = container.GetInstance<IConfigurationService>();
            var serviceBusConfiguration = configurationService.Get<AzureServiceBusConfiguration>();

            serviceBusConfiguration.Should().NotBeNull();

            serviceBusConfiguration.ConnectionString.Should().NotBeNullOrWhiteSpace();
            serviceBusConfiguration.DefaultMessageCountWarningLimit.Should().BePositive();
            serviceBusConfiguration.DefaultDeadLetterMessageCountWarningLimit.Should().BePositive();
            serviceBusConfiguration.DefaultMaxConcurrentMessagesPerNode.Should().BePositive();

            serviceBusConfiguration.Topics.Should().NotBeNull();
            serviceBusConfiguration.Topics.Length.Should().BePositive();

            foreach (var topicConfiguration in serviceBusConfiguration.Topics)
            {
                topicConfiguration.TopicName.Should().NotBeNullOrWhiteSpace();
                topicConfiguration.MessageType.Should().NotBeNullOrWhiteSpace();

                topicConfiguration.Subscriptions.Should().NotBeNull();
                topicConfiguration.Subscriptions.Length.Should().BeGreaterOrEqualTo(0);

                foreach (var subscriptionConfiguration in topicConfiguration.Subscriptions)
                {
                    // One or more subscription configuration items should be specified.
                    Assert.That(
                        !string.IsNullOrWhiteSpace(subscriptionConfiguration.SubscriptionName) ||
                        subscriptionConfiguration.MessageCountWarningLimit.HasValue ||
                        subscriptionConfiguration.DeadLetterMessageCountWarningLimit.HasValue ||
                        subscriptionConfiguration.MaxConcurrentMessagesPerNode.HasValue);
                }
            }
        }
    }
}
