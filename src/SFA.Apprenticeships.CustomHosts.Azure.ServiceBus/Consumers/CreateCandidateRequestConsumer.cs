namespace SFA.Apprenticeships.CustomHosts.Azure.ServiceBus.Consumers
{
    using System;
    using Application.Candidate;
    using Domain.Interfaces.Messaging;

    public class CreateCandidateRequestConsumer : IServiceBusSubscriber<CreateCandidateRequest>
    {
        public void Consume(CreateCandidateRequest message)
        {
            Console.WriteLine("CREATE: {0}", message.CandidateId);
        }
    }
}
