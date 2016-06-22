namespace SFA.Apprenticeships.Infrastructure.Azure.Common
{
    using System;
    using SFA.Infrastructure.Interfaces;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Configuration;

    using SFA.Apprenticeships.Application.Interfaces;

    public class AzureCloudClient : IAzureCloudClient
    {
        private readonly CloudQueueClient _cloudQueueClient;

        public AzureCloudClient(IConfigurationService configurationService)
        {
            var connectionString = configurationService.Get<AzureConfiguration>().StorageConnectionString;
            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            _cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
        }

        public CloudQueueMessage GetMessage(string queueName)
        {
            return _cloudQueueClient.GetQueueReference(queueName).GetMessage(TimeSpan.FromMinutes(5));    
        }

        public void DeleteMessage(string queueName, string id, string popReceipt)
        {
            _cloudQueueClient.GetQueueReference(queueName).DeleteMessage(id, popReceipt);
        }
    }
}
