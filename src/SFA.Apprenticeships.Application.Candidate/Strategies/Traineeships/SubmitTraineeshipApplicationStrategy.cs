namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using Infrastructure.Interfaces;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using MessagingErrorCodes = Interfaces.Messaging.ErrorCodes;

    public class SubmitTraineeshipApplicationStrategy : ISubmitTraineeshipApplicationStrategy
    {
        private readonly ILogService _logger;
        private readonly IServiceBus _serviceBus;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly ICommunicationService _communicationService;

        public SubmitTraineeshipApplicationStrategy(
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository,
            ICommunicationService communicationService, ILogService logger, IServiceBus serviceBus)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _communicationService = communicationService;
            _logger = logger;
            _serviceBus = serviceBus;
        }

        public void SubmitApplication(Guid applicationId)
        {
            var traineeshipApplicationDetail = _traineeshipApplicationReadRepository.Get(applicationId, true);

            try
            {
                traineeshipApplicationDetail.SetStateSubmitting();
                traineeshipApplicationDetail.SetStateSubmitted();

                _traineeshipApplicationWriteRepository.Save(traineeshipApplicationDetail);
                
                PublishMessage(traineeshipApplicationDetail);
                NotifyCandidate(traineeshipApplicationDetail);
            }
            catch (Exception ex)
            {
                _logger.Debug("SubmitTraineeshipApplicationRequest could not be queued for ApplicationId={0}", applicationId);

                throw new CustomException("SubmitTraineeshipApplicationRequest could not be queued", ex,
                    MessagingErrorCodes.ApplicationQueuingError);
            }
        }

        private void PublishMessage(TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            try
            {
                var message = new SubmitTraineeshipApplicationRequest
                {
                    ApplicationId = traineeshipApplicationDetail.EntityId
                };

                _serviceBus.PublishMessage(message);
            }
            catch
            {
                // No need to delete the application if failed enqueing the applicatino submission?
                // Should we swallow the exception?
                // Compensate for failure to enqueue application submission by deleting the application.
                //_traineeshipApplicationWriteRepository.Delete(traineeshipApplicationDetail.EntityId);
                throw;
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