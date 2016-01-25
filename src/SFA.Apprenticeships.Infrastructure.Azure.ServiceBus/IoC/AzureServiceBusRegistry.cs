namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.IoC
{
    using System;
    using Configuration;
    using Domain.Interfaces.Messaging;
    using Factory;
    using ServiceBus;
    using StructureMap.Configuration.DSL;

    public class AzureServiceBusRegistry : Registry
    {
        public AzureServiceBusRegistry(AzureServiceBusConfiguration configuration)
        {
            var topicNameFormatterType = Type.GetType(configuration.TopicNameFormatter);
            if (topicNameFormatterType == null)
            {
                throw new Exception($"TopicNameFormatter {configuration.TopicNameFormatter} was not recognized as a valid class");
            }
            var topicNameFormatter = (ITopicNameFormatter)Activator.CreateInstance(topicNameFormatterType);

            For<ITopicNameFormatter>().Singleton().Use(topicNameFormatter);
            For<IServiceBusInitialiser>().Singleton().Use<AzureServiceBusInitialiser>();
            For<IServiceBus>().Singleton().Use<AzureServiceBus>();
            For<IClientFactory>().Singleton().Use<ClientFactory>();
        }
    }
}