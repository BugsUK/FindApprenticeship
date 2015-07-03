namespace SFA.Apprenticeships.CustomHosts.Azure.ServiceBus
{
    using System;
    using Application.Candidate;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Infrastructure.Azure.ServiceBus;
    using Infrastructure.Azure.ServiceBus.Configuration;
    using StructureMap;
    using Moq;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
                return;
            }

            var container = BuildContainer();
            var mockConfigurationService = BuildMockConfigurationService(args[0]);

            var manager = new AzureServiceBusManager(container, mockConfigurationService.Object);
            var bus = new AzureServiceBus(mockConfigurationService.Object);

            bus.PublishMessage(new CreateCandidateRequest
            {
                CandidateId = Guid.NewGuid(),
                ProcessTime = DateTime.UtcNow.AddMinutes(5)
            });

            manager.Initialise();
            manager.Subscribe();

            Console.WriteLine("Subscribed");
            Console.ReadLine();

            manager.Unsubscribe();

            Console.WriteLine("Unsubscribed");

            bus.PublishMessage(new CreateCandidateRequest
            {
                CandidateId = Guid.NewGuid(),
                ProcessTime = DateTime.UtcNow.AddMinutes(5)
            });

            Console.ReadLine();
        }

        private static IContainer BuildContainer()
        {
            return new Container(container =>
            {
                container
                    .For<IServiceBusSubscriber<CreateCandidateRequest>>()
                    .Use<Consumers.CreateCandidateRequestConsumer>();

                container
                    .For<IServiceBusSubscriber<CreateCandidateRequest>>()
                    .Use<Consumers.AuditCreateCandidateRequestConsumer>();

                container
                    .For<IServiceBusSubscriber<CreateCandidateRequest>>()
                    .Use<Consumers.RequeueCreateCandidateRequestConsumer>();
            });
        }

        private static Mock<IConfigurationService> BuildMockConfigurationService(string connectionString)
        {
            var mockConfigurationService = new Mock<IConfigurationService>();

            var configuration = new AzureServiceBusConfiguration
            {
                ConnectionString = connectionString,
                DefaultMaxConcurrentMessagesPerNode = 5,
                DefaultMessageCountWarningLimit = 500,
                DefaultDeadLetterMessageCountWarningLimit = 100,
                DefaultSubscriptionName = "consume",
                Topics = new[]
                {
                    new AzureServiceBusTopicConfiguration
                    {                        
                        TopicName = "candidate-create",
                        MessageType = "SFA.Apprenticeships.Application.Candidate.CreateCandidateRequest, SFA.Apprenticeships.Application.Candidate",
                        Subscriptions = new[]
                        {
                            new AzureServiceBusSubscriptionConfiguration
                            {
                                SubscriptionName = "create",
                                SubscriberType = "SFA.Apprenticeships.CustomHosts.Azure.ServiceBus.CreateCandidateRequestConsumer, SFA.Apprenticeships.CustomHosts.Azure.ServiceBus"
                            },
                            new AzureServiceBusSubscriptionConfiguration
                            {
                                SubscriptionName = "audit",
                                SubscriberType = "SFA.Apprenticeships.CustomHosts.Azure.ServiceBus.AuditCreateCandidateRequestConsumer, SFA.Apprenticeships.CustomHosts.Azure.ServiceBus"
                            },
                            new AzureServiceBusSubscriptionConfiguration
                            {
                                SubscriptionName = "requeue",
                                SubscriberType = "SFA.Apprenticeships.CustomHosts.Azure.ServiceBus.OtherCreateCandidateRequestConsumer, SFA.Apprenticeships.CustomHosts.Azure.ServiceBus"
                            }
                        }
                    }
                }
            };

            mockConfigurationService.Setup(mock => mock
                .Get<AzureServiceBusConfiguration>())
                .Returns(configuration);

            return mockConfigurationService;
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: TODO");
        }
    }
}
