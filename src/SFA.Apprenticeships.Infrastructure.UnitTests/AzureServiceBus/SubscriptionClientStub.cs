namespace SFA.Apprenticeships.Infrastructure.UnitTests.AzureServiceBus
{
    using System;
    using System.Threading.Tasks;
    using Azure.ServiceBus.Model;
    using Microsoft.ServiceBus.Messaging;

    public class SubscriptionClientStub : ISubscriptionClient
    {
        private Action<IBrokeredMessage> _callback;
        private OnMessageOptions _onMessageOptions;

        public void OnMessage(Action<IBrokeredMessage> callback, OnMessageOptions onMessageOptions)
        {
            _callback = callback;
            _onMessageOptions = onMessageOptions;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Send(IBrokeredMessage message)
        {
            _callback.Invoke(message);
            if (_onMessageOptions.AutoComplete)
            {
                message.Complete();
            }
        }
    }
}