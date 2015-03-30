namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.RabbitMq.Consumers
{
    using System;
    using EasyNetQ.AutoSubscribe;

    public class TestConsumerSync : IConsume<TestMessage>
    {
        [AutoSubscriberConsumer(SubscriptionId = "TestMessageConsumerSync")]
        public void Consume(TestMessage message)
        {
            Console.WriteLine("TestMessageConsumerSync received message with TestString:" + message.TestString);
            ConsumerCounter.IncrementCounter();
        }
    }
}
