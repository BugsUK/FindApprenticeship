namespace SFA.Apprenticeships.Domain.Interfaces.Messaging
{
    public interface IJobControlQueue<out T> where T : StorageQueueMessage
    {
        T GetMessage(string queueName);

        void DeleteMessage(string queueName, string messageId, string popReceipt);
    }
}
