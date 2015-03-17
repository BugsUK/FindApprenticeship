namespace SFA.Apprenticeships.Infrastructure.Azure.Common.Messaging
{
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Messaging;

    public abstract class AzureControlQueueConsumer
    {
        private readonly ILogService _logger;
        protected readonly IJobControlQueue<StorageQueueMessage> MessageService;
        private readonly string _jobName;
        private readonly string _queueName;

        protected AzureControlQueueConsumer(IJobControlQueue<StorageQueueMessage> messageService, ILogService logger, string jobName, string queueName)
        {
            MessageService = messageService;
            _jobName = jobName;
            _queueName = queueName;
            _logger = logger;
        }

        protected StorageQueueMessage GetLatestQueueMessage()
        {
            _logger.Debug("Checking control queue '" + _queueName + "' for '" + _jobName + "' job");

            var queueMessage = MessageService.GetMessage(_queueName);

            if (queueMessage == null)
            {
                _logger.Debug("No control message found on queue '" + _queueName + " for '" + _jobName + "' job");
                return null;
            }

            var foundSurplusMessages = false;

            while (true)
            {
                var nextQueueMessage = MessageService.GetMessage(_queueName);

                if (nextQueueMessage == null)
                {
                    // We have the latest message on the queue.
                    break;
                }

                MessageService.DeleteMessage(_queueName, queueMessage.MessageId, queueMessage.PopReceipt);
                queueMessage = nextQueueMessage;
                foundSurplusMessages = true;
            }

            if (foundSurplusMessages)
            {
                _logger.Warn("Found more than 1 control message found on queue '" + _queueName + " for '" + _jobName + "' job");
            }

            _logger.Info("Found valid control message on queue: '{0}' for job: '{1}' with message id: {2}", _queueName, _jobName, queueMessage.MessageId);

            return queueMessage;
        }

        protected void DeleteMessage(string messageId, string popReceipt)
        {
            MessageService.DeleteMessage(_queueName, messageId, popReceipt);
        }
    }
}
