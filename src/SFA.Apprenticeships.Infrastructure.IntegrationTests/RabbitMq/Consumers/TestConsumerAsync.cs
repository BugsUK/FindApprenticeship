namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.RabbitMq.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;

    public class TestConsumerAsync : IConsumeAsync<TestMessage>
    {
        [AutoSubscriberConsumer(SubscriptionId = "TestMessageConsumerAsync")]
        public Task Consume(TestMessage message)
        {
            return Task.Run(() => ConsumeTask(message));
        }

        private void ConsumeTask(TestMessage message)
        {
            Console.WriteLine("TestMessageConsumerAsync received message with TestString:" + message.TestString);
            ConsumerCounter.IncrementCounter();
        }
    }
}
