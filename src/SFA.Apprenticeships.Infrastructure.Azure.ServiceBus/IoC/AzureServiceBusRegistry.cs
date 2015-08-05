namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.IoC
{
    using Domain.Interfaces.Messaging;
    using Factory;
    using ServiceBus;
    using StructureMap.Configuration.DSL;

    public class AzureServiceBusRegistry : Registry
    {
        public AzureServiceBusRegistry()
        {
            For<IServiceBusInitialiser>().Singleton().Use<AzureServiceBusInitialiser>();
            For<IServiceBus>().Singleton().Use<AzureServiceBus>();
            For<IClientFactory>().Singleton().Use<ClientFactory>();
        }
    }
}