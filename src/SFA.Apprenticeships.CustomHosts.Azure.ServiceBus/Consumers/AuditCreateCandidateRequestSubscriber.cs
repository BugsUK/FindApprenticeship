namespace SFA.Apprenticeships.CustomHosts.Azure.ServiceBus.Consumers
{
    using System;
    using Application.Candidate;
    using Domain.Interfaces.Messaging;

    public class AuditCreateCandidateRequestSubscriber : IServiceBusSubscriber<CreateCandidateRequest>
    {
        [ServiceBusTopicSubscription(TopicName = "candidate-create", SubscriptionName = "audit")]
        public ServiceBusMessageResult Consume(CreateCandidateRequest message)
        {
            Console.WriteLine("AUDIT: CreateCandidateRequest: {0}", message.CandidateId);

            return ServiceBusMessageResult.Complete();
        }
    }
}