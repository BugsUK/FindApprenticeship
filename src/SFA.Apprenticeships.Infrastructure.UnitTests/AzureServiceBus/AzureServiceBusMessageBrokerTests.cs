namespace SFA.Apprenticeships.Infrastructure.UnitTests.AzureServiceBus
{
    using System;
    using System.Collections.Generic;
    using Application.Candidate;
    using Azure.ServiceBus.Factory;
    using Azure.ServiceBus.Model;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Microsoft.ServiceBus.Messaging;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class AzureServiceBusMessageBrokerTests
    {
        [Test]
        public void MessagesAreNotAutoCompleted()
        {
            var subscriber = new CreateCandidateRequestSubscriberMock(ServiceBusMessageStates.Complete);
            var subscribers = new List<IServiceBusSubscriber<CreateCandidateRequest>> { subscriber };
            var topicClient = new Mock<ITopicClient>();
            var subscriptionClient = new Mock<ISubscriptionClient>();
            var clientFactory = new Mock<IClientFactory>();
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>())).Returns(topicClient.Object);
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ReceiveMode>())).Returns(subscriptionClient.Object);
            var broker = new AzureServiceBusMessageBrokerBuilder<CreateCandidateRequest>().With(subscribers).With(clientFactory).Build();
            broker.Subscribe();

            //Subscription should be set up with AutoComplete = false
            subscriptionClient.Verify(c => c.OnMessage(It.IsAny<Action<IBrokeredMessage>>(), It.Is<OnMessageOptions>(o => !o.AutoComplete)));
        }

        [Test]
        public void MessagesAreCompletedByTheBroker()
        {
            var subscriber = new CreateCandidateRequestSubscriberMock(ServiceBusMessageStates.Complete);
            var subscribers = new List<IServiceBusSubscriber<CreateCandidateRequest>> { subscriber };
            var topicClient = new Mock<ITopicClient>();
            var subscriptionClient = new SubscriptionClientStub();
            var clientFactory = new Mock<IClientFactory>();
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>())).Returns(topicClient.Object);
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ReceiveMode>())).Returns(subscriptionClient);
            var broker = new AzureServiceBusMessageBrokerBuilder<CreateCandidateRequest>().With(subscribers).With(clientFactory).Build();
            broker.Subscribe();

            var message = new Mock<IBrokeredMessage>();
            message.Setup(m => m.GetBody<string>()).Returns("");
            subscriptionClient.Send(message.Object);
            message.Verify(m => m.Complete(), Times.Once);
        }

        [Test]
        public void MessagesAreAbandonedByTheBroker()
        {
            var subscriber = new CreateCandidateRequestSubscriberMock(ServiceBusMessageStates.Abandon);
            var subscribers = new List<IServiceBusSubscriber<CreateCandidateRequest>> { subscriber };
            var topicClient = new Mock<ITopicClient>();
            var subscriptionClient = new SubscriptionClientStub();
            var clientFactory = new Mock<IClientFactory>();
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>())).Returns(topicClient.Object);
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ReceiveMode>())).Returns(subscriptionClient);
            var broker = new AzureServiceBusMessageBrokerBuilder<CreateCandidateRequest>().With(subscribers).With(clientFactory).Build();
            broker.Subscribe();

            var message = new Mock<IBrokeredMessage>();
            message.Setup(m => m.GetBody<string>()).Returns("");
            subscriptionClient.Send(message.Object);
            message.Verify(m => m.Abandon(), Times.Once);
            message.Verify(m => m.Complete(), Times.Never);
        }

        [Test]
        public void MessagesAreDeadLetteredByTheBroker()
        {
            var subscriber = new CreateCandidateRequestSubscriberMock(ServiceBusMessageStates.DeadLetter);
            var subscribers = new List<IServiceBusSubscriber<CreateCandidateRequest>> { subscriber };
            var topicClient = new Mock<ITopicClient>();
            var subscriptionClient = new SubscriptionClientStub();
            var clientFactory = new Mock<IClientFactory>();
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>())).Returns(topicClient.Object);
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ReceiveMode>())).Returns(subscriptionClient);
            var broker = new AzureServiceBusMessageBrokerBuilder<CreateCandidateRequest>().With(subscribers).With(clientFactory).Build();
            broker.Subscribe();

            var message = new Mock<IBrokeredMessage>();
            message.Setup(m => m.GetBody<string>()).Returns("");
            subscriptionClient.Send(message.Object);
            message.Verify(m => m.DeadLetter(), Times.Once);
            message.Verify(m => m.Complete(), Times.Never);
        }

        [Test]
        public void MessagesWithUnknownStateAreDeadLettered()
        {
            var subscriber = new CreateCandidateRequestSubscriberMock(ServiceBusMessageStates.Unknown);
            var subscribers = new List<IServiceBusSubscriber<CreateCandidateRequest>> { subscriber };
            var topicClient = new Mock<ITopicClient>();
            var subscriptionClient = new SubscriptionClientStub();
            var clientFactory = new Mock<IClientFactory>();
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>())).Returns(topicClient.Object);
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ReceiveMode>())).Returns(subscriptionClient);
            var broker = new AzureServiceBusMessageBrokerBuilder<CreateCandidateRequest>().With(subscribers).With(clientFactory).Build();
            broker.Subscribe();

            var message = new Mock<IBrokeredMessage>();
            message.Setup(m => m.GetBody<string>()).Returns("");
            subscriptionClient.Send(message.Object);
            message.Verify(m => m.DeadLetter(), Times.Once);
            message.Verify(m => m.Complete(), Times.Never);
        }

        [Test]
        public void MessagesAreRequeuedAndCompletedByTheBroker()
        {
            var subscriber = new CreateCandidateRequestSubscriberMock(ServiceBusMessageStates.Requeue);
            var subscribers = new List<IServiceBusSubscriber<CreateCandidateRequest>> { subscriber };
            var topicClient = new Mock<ITopicClient>();
            BrokeredMessage newBrokeredMessage = null;
            topicClient.Setup(c => c.Send(It.IsAny<BrokeredMessage>())).Callback<BrokeredMessage>(bm => newBrokeredMessage = bm);
            var subscriptionClient = new SubscriptionClientStub();
            var clientFactory = new Mock<IClientFactory>();
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>())).Returns(topicClient.Object);
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ReceiveMode>())).Returns(subscriptionClient);
            var broker = new AzureServiceBusMessageBrokerBuilder<CreateCandidateRequest>().With(subscribers).With(clientFactory).Build();
            broker.Subscribe();

            var message = new Mock<IBrokeredMessage>();
            message.Setup(m => m.GetBody<string>()).Returns("");
            subscriptionClient.Send(message.Object);
            //Original messages is completed
            message.Verify(m => m.Complete(), Times.Once);
            //New message has been created and assigned a scheduled time 30 seconds in the future
            newBrokeredMessage.Should().NotBeNull();
            newBrokeredMessage.ScheduledEnqueueTimeUtc.Should().BeCloseTo(DateTime.UtcNow.AddSeconds(30), 1000);
        }

        [Test]
        public void SubsequentRequeuedMessagesAreScheduledForFiveMinutesHence()
        {
            var subscriber = new CreateCandidateRequestSubscriberMock(ServiceBusMessageStates.Requeue);
            var subscribers = new List<IServiceBusSubscriber<CreateCandidateRequest>> { subscriber };
            var topicClient = new Mock<ITopicClient>();
            BrokeredMessage newBrokeredMessage = null;
            topicClient.Setup(c => c.Send(It.IsAny<BrokeredMessage>())).Callback<BrokeredMessage>(bm => newBrokeredMessage = bm);
            var subscriptionClient = new SubscriptionClientStub();
            var clientFactory = new Mock<IClientFactory>();
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>())).Returns(topicClient.Object);
            clientFactory.Setup(f => f.CreateFromConnectionString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ReceiveMode>())).Returns(subscriptionClient);
            var broker = new AzureServiceBusMessageBrokerBuilder<CreateCandidateRequest>().With(subscribers).With(clientFactory).Build();
            broker.Subscribe();

            var message = new Mock<IBrokeredMessage>();
            message.Setup(m => m.ScheduledEnqueueTimeUtc).Returns(DateTime.UtcNow);
            message.Setup(m => m.GetBody<string>()).Returns("");
            subscriptionClient.Send(message.Object);
            //Original messages is completed
            message.Verify(m => m.Complete(), Times.Once);
            //New message has been created and assigned a scheduled time 5 minutes in the future
            newBrokeredMessage.Should().NotBeNull();
            newBrokeredMessage.ScheduledEnqueueTimeUtc.Should().BeCloseTo(DateTime.UtcNow.AddSeconds(300), 1000);
        }
    }
}