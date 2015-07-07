namespace SFA.Apprenticeships.CustomHosts.Azure.ServiceBus.Consumers
{
    using System;
    using Application.Candidate;
    using Domain.Interfaces.Messaging;

    public class RequeueCreateCandidateRequestSubscriber : IServiceBusSubscriber<CreateCandidateRequest>
    {
        [ServiceBusTopicSubscription(TopicName = "candidate-create", SubscriptionName = "requeue")]
        public ServiceBusMessageResult Consume(CreateCandidateRequest message)
        {
            Console.WriteLine("REQUEUE: CreateCandidateRequest: {0}", message.CandidateId);

            return ServiceBusMessageResult.Complete();
        }
    }
}