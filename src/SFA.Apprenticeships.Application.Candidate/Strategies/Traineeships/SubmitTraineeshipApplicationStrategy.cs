namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Logging;
    using MessagingErrorCodes = Interfaces.Messaging.ErrorCodes;

    public class SubmitTraineeshipApplicationStrategy : ISubmitTraineeshipApplicationStrategy
    {
        private readonly ILogService _logger;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly ICommunicationService _communicationService;

        public SubmitTraineeshipApplicationStrategy(ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository, ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository, ICommunicationService communicationService, ILogService logger)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _communicationService = communicationService;
            _logger = logger;
        }

        public void SubmitApplication(Guid applicationId)
        {
            var traineeshipApplicationDetail = _traineeshipApplicationReadRepository.Get(applicationId, true);

            try
            {
                traineeshipApplicationDetail.SetStateSubmitting();
                traineeshipApplicationDetail.SetStateSubmitted();
                _traineeshipApplicationWriteRepository.Save(traineeshipApplicationDetail);
                NotifyCandidate(traineeshipApplicationDetail);
            }
            catch (Exception ex)
            {
                _logger.Debug("SubmitTraineeshipApplicationRequest could not be queued for ApplicationId={0}", applicationId);

                throw new CustomException("SubmitTraineeshipApplicationRequest could not be queued", ex,
                    MessagingErrorCodes.ApplicationQueuingError);
            }
        }

        private void NotifyCandidate(TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            _communicationService.SendMessageToCandidate(traineeshipApplicationDetail.CandidateId, MessageTypes.TraineeshipApplicationSubmitted,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.ApplicationId, traineeshipApplicationDetail.EntityId.ToString())
                });
        }
    }
}