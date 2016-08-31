namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Messaging
{
    using Domain.Interfaces.Messaging;

    using SFA.Apprenticeships.Application.Interfaces;

    public abstract class AzureControlQueueConsumer
    {
        private readonly ILogService _logger;
        protected readonly IJobControlQueue<StorageQueueMessage> MessageService;
        private readonly string _jobName;
        
        protected string QueueName { get; private set; }

        protected AzureControlQueueConsumer(IJobControlQueue<StorageQueueMessage> messageService, ILogService logger, string jobName, string queueName)
        {
            MessageService = messageService;
            _jobName = jobName;
            QueueName = queueName;
            _logger = logger;
        }

        protected StorageQueueMessage GetLatestQueueMessage()
        {
            _logger.Debug("Checking control queue '" + QueueName + "' for '" + _jobName + "' job");

            var queueMessage = MessageService.GetMessage(QueueName);

            if (queueMessage == null)
            {
                _logger.Debug("No control message found on queue '" + QueueName + " for '" + _jobName + "' job");
                return null;
            }

            var foundSurplusMessages = false;

            while (true)
            {
                var nextQueueMessage = MessageService.GetMessage(QueueName);

                if (nextQueueMessage == null)
                {
                    // We have the latest message on the queue.
                    break;
                }

                MessageService.DeleteMessage(QueueName, queueMessage.MessageId, queueMessage.PopReceipt);
                queueMessage = nextQueueMessage;
                foundSurplusMessages = true;
            }

            if (foundSurplusMessages)
            {
                _logger.Warn("Found more than 1 control message found on queue '" + QueueName + " for '" + _jobName + "' job");
            }

            _logger.Info("Found valid control message on queue: '{0}' for job: '{1}' with message id: {2}", QueueName, _jobName, queueMessage.MessageId);

            return queueMessage;
        }

        protected void DeleteMessage(string messageId, string popReceipt)
        {
            MessageService.DeleteMessage(QueueName, messageId, popReceipt);
        }
    }
}
