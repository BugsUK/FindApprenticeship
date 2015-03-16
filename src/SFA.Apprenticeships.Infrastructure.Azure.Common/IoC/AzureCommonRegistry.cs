namespace SFA.Apprenticeships.Infrastructure.Azure.Common.IoC
{
    using Configuration;
    using Domain.Interfaces.Messaging;
    using Messaging;
    using StructureMap.Configuration.DSL;

    public class AzureCommonRegistry : Registry
    {
        public AzureCommonRegistry()
        {
            For<IJobControlQueue<StorageQueueMessage>>().Use<AzureControlQueue>();
            For<IAzureCloudConfig>().Singleton().Use<AzureCloudConfig>();
            For<IAzureCloudClient>().Use<AzureCloudClient>();
        }
    }
}
