namespace SFA.Apprenticeships.CustomHosts.Azure.ServiceBus.Consumers
{
    using System;
    using Application.Candidate;
    using Domain.Interfaces.Messaging;

    public class AuditCreateCandidateRequestConsumer : IServiceBusSubscriber<CreateCandidateRequest>
    {
        public void Consume(CreateCandidateRequest message)
        {
            Console.WriteLine("AUDIT: {0}", message.CandidateId);
        }
    }
}
