namespace SFA.Apprenticeships.Infrastructure.Azure.ServiceBus.Model
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

    public class SubscriptionClientWrapper : ISubscriptionClient
    {
        private readonly SubscriptionClient _subscriptionClient;

        public SubscriptionClientWrapper(SubscriptionClient subscriptionClient)
        {
            _subscriptionClient = subscriptionClient;
        }

        public void OnMessage(Action<IBrokeredMessage> callback, OnMessageOptions onMessageOptions)
        {
            _subscriptionClient.OnMessageAsync(async brokeredMessage => await Task.Run(() =>
                callback(new BrokeredMessageWrapper(brokeredMessage))),
                onMessageOptions);
        }

        public void Close()
        {
            _subscriptionClient.Close();
        }
    }
}