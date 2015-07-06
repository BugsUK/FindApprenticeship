// TODO: avoid parameterising with IContainer.

namespace SFA.Apprenticeships.CustomHosts.Azure.ServiceBus
{
    using System;
    using System.Reflection;
    using Application.Candidate;
    using Application.Interfaces.Logging;
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
            var mockLogService = new Mock<ILogService>();
            var mockConfigurationService = BuildMockConfigurationService(args[0]);

            var manager = new AzureServiceBusManager(container, mockLogService.Object, mockConfigurationService.Object);
            var bus = new AzureServiceBus(mockLogService.Object, mockConfigurationService.Object);

            manager.Initialise();

            bus.PublishMessage(new CreateCandidateRequest
            {
                CandidateId = Guid.NewGuid()
            });

            manager.Subscribe();

            Console.WriteLine("Subscribed");
            Console.ReadLine();

            manager.Unsubscribe();

            Console.WriteLine("Unsubscribed");
            Console.ReadLine();
        }

        private static IContainer BuildContainer()
        {
            return new Container(container => container.Scan(scanner =>
            {
                scanner.Assembly(Assembly.GetExecutingAssembly());
                scanner.AddAllTypesOf<IServiceBusSubscriber>();
            }));
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
