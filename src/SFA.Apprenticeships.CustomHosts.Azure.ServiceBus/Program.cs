// TODO: avoid parameterising with IContainer.

namespace SFA.Apprenticeships.CustomHosts.Azure.ServiceBus
{
    using System;
    using System.Linq;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Consumers;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Infrastructure.Azure.ServiceBus;
    using Infrastructure.Azure.ServiceBus.Configuration;
    using StructureMap;
    using Moq;

    class Program
    {
        private static Mock<ILogService> _mockLogService;
        private static Mock<IConfigurationService> _mockConfigurationService;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
                return;
            }

            _mockLogService = new Mock<ILogService>();
            _mockConfigurationService = BuildMockConfigurationService(args[0]);

            var container = BuildContainer();

            var intialiser = container.GetInstance<IServiceBusInitialiser>();

            intialiser.Initialise();

            var bus = new AzureServiceBus(_mockLogService.Object, _mockConfigurationService.Object);

            bus.PublishMessage(new CreateCandidateRequest
            {
                CandidateId = Guid.NewGuid()
            });

            bus.PublishMessage(new SubmitApprenticeshipApplicationRequest
            {
                ApplicationId = Guid.NewGuid()
            });

            var instances = container
                .GetAllInstances<IServiceBusMessageBroker>()
                .ToList();

            var instanceCount = 0;

            foreach (var instance in instances)
            {
                instance.Subscribe();
                instanceCount++;
            }

            Console.WriteLine("Subscribed {0}", instanceCount);
            Console.ReadLine();

            foreach (var instance in instances)
            {
                instance.Unsubscribe();

                instanceCount++;
            }

            Console.WriteLine("Unsubscribed");
            Console.ReadLine();
        }

        private static IContainer BuildContainer()
        {
            return new Container(container =>
            {
                container
                    .For<ILogService>()
                    .Use(_mockLogService.Object);

                container
                    .For<IConfigurationService>()
                    .Use(_mockConfigurationService.Object);

                container
                    .For<IServiceBusInitialiser>()
                    .Use<AzureServiceBusInitialiser>();

                container
                    .For<IServiceBusSubscriber<CreateCandidateRequest>>()
                    .Use<CreateCandidateRequestSubscriber>();

                container
                    .For<IServiceBusSubscriber<CreateCandidateRequest>>()
                    .Use<AuditCreateCandidateRequestSubscriber>();

                container
                    .For<IServiceBusSubscriber<CreateCandidateRequest>>()
                    .Use<RequeueCreateCandidateRequestSubscriber>();

                container
                    .For<IServiceBusSubscriber<SubmitApprenticeshipApplicationRequest>>()
                    .Use<SubmitApprenticeshipApplicationRequestSubscriber>();

                container
                    .For<IServiceBusMessageBroker>()
                    .Use<AzureServiceBusMessageBroker<CreateCandidateRequest>>();

                container
                    .For<IServiceBusMessageBroker>()
                    .Use<AzureServiceBusMessageBroker<SubmitApprenticeshipApplicationRequest>>();
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
                        MessageType = "SFA.Apprenticeships.Application.Candidate.CreateCandidateRequest",
                        Subscriptions = new[]
                        {
                            new AzureServiceBusSubscriptionConfiguration
                            {
                                SubscriptionName = "create",
                            },
                            new AzureServiceBusSubscriptionConfiguration
                            {
                                SubscriptionName = "audit",
                            },
                            new AzureServiceBusSubscriptionConfiguration
                            {
                                SubscriptionName = "requeue",
                            }
                        }
                    },
                    new AzureServiceBusTopicConfiguration
                    {                        
                        TopicName = "apprenticeship-application-submit",
                        MessageType = "SFA.Apprenticeships.Application.Candidate.SubmitApprenticeshipApplicationRequest",
                        Subscriptions = new[]
                        {
                            new AzureServiceBusSubscriptionConfiguration
                            {
                                SubscriptionName = "default"
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
