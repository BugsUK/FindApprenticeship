﻿namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    //TODO: Consistent messaging API naming - Enqueue, Dequeue, Pop etc, investigate between Azure/Rabbit.
    //todo: should move to domain interfaces?
    public interface IMessageService<T>
    {
        T GetMessage();

        void DeleteMessage(string id, string popReceipt);

        void AddMessage(T queueMessage);
    }
}
