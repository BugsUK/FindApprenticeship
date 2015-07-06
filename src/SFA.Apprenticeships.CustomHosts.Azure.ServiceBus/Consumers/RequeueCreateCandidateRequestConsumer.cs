namespace SFA.Apprenticeships.CustomHosts.Azure.ServiceBus.Consumers
{
    using System;
    using Application.Candidate;
    using Domain.Interfaces.Messaging;

    public class RequeueCreateCandidateRequestConsumer : IServiceBusSubscriber<CreateCandidateRequest>
    {
        [ServiceBusTopicSubscription(TopicName = "candidate-create", SubscriptionName = "requeue")]
        public ServiceBusMessageResult Consume(CreateCandidateRequest message)
        {
            Console.WriteLine("REQUEUE: {0}", message.CandidateId);

            // return ServiceBusMessageResult.Reqeue(DateTime.UtcNow.AddSeconds(30));
            return ServiceBusMessageResult.Complete();
        }
    }
}
