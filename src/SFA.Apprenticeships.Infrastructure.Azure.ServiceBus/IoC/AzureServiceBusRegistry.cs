namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.IoC
{
    using StructureMap.Configuration.DSL;

    public class AzureServiceBusRegistry : Registry
    {
        public AzureServiceBusRegistry()
        {
            For<IServiceBusManager>().Singleton().Use<AzureServiceBusManager>();
            // For<IBootstrapSubcribers>().Singleton().Use<AzureServiceBusSubcriberBootstrapper>();
        }
    }
}
