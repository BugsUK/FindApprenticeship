namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Model
{
    using System;
    using Microsoft.ServiceBus.Messaging;

    public interface ISubscriptionClient
    {
        void OnMessage(Action<IBrokeredMessage> callback, OnMessageOptions onMessageOptions);
        void Close();
    }
}