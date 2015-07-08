namespace SFA.Apprenticeships.Infrastructure.Processes.Candidates
{
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Messaging;

    public class SaveCandidateRequestSubscriber : IServiceBusSubscriber<SaveCandidateRequest>
    {
        private readonly ILogService _logService;

        public SaveCandidateRequestSubscriber(ILogService logService)
        {
            _logService = logService;
        }

        [ServiceBusTopicSubscription(TopicName = "save-candidate-request")]
        public ServiceBusMessageResult Consume(SaveCandidateRequest message)
        {
            _logService.Info("SaveCandidateRequest: {0}", message.CandidateId);

            return ServiceBusMessageResult.Complete();
        }
    }
}
