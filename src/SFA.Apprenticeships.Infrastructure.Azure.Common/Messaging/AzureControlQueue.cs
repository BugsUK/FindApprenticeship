namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Messaging
{
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Messaging;

    public class AzureControlQueue : IJobControlQueue<StorageQueueMessage>
    {
        private readonly ILogService _logger;
        private readonly IAzureCloudClient _azureCloudClient;

        public AzureControlQueue(IAzureCloudClient azureCloud, ILogService logger)
        {
            _azureCloudClient = azureCloud;
            _logger = logger;
        }

        public StorageQueueMessage GetMessage(string queueName)
        {
            _logger.Debug("Checking Azure control queue for control message: '{0}'", queueName);

            // If queue name is not specified, get it from configuration.
            var message = _azureCloudClient.GetMessage(queueName);

            if (message == null)
            {
                _logger.Debug("Azure control queue empty: '{0}'", queueName);
                return null;
            }

            _logger.Debug("Azure control queue item returned: '{0}'", queueName);

            var storageMessage = AzureMessageHelper.DeserialiseQueueMessage<StorageQueueMessage>(message);

            storageMessage.MessageId = message.Id;
            storageMessage.PopReceipt = message.PopReceipt;

            _logger.Debug("Azure control queue item deserialised: '{0}'", queueName);

            return storageMessage;
        }

        public void DeleteMessage(string queueName, string messageId, string popReceipt)
        {
            _logger.Debug("Deleting Azure control queue item: '{0}'", queueName);

            _azureCloudClient.DeleteMessage(queueName, messageId, popReceipt);

            _logger.Debug("Deleted Azure control queue item: '{0}'", queueName);
        }
    }
}
