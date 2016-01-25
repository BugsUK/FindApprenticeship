namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using MessagingErrorCodes = Interfaces.Messaging.ErrorCodes;

    public class LegacySubmitApprenticeshipApplicationStrategy : ISubmitApprenticeshipApplicationStrategy
    {
        private readonly ILogService _logger;
        private readonly IServiceBus _serviceBus;

        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;

        private readonly ICommunicationService _communicationService;

        public LegacySubmitApprenticeshipApplicationStrategy(
            ILogService logger,
            IServiceBus serviceBus,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ICommunicationService communicationService)
        {
            _logger = logger;
            _serviceBus = serviceBus;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _communicationService = communicationService;
        }

        public void SubmitApplication(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId, true);

            applicationDetail.AssertState("Submit apprenticeshipApplication", ApplicationStatuses.Draft);

            try
            {
                applicationDetail.SetStateSubmitting();
                _apprenticeshipApplicationWriteRepository.Save(applicationDetail);

                PublishMessage(applicationDetail);
                NotifyCandidate(applicationDetail.CandidateId, applicationDetail.EntityId.ToString());
            }
            catch (Exception ex)
            {
                _logger.Debug("SubmitApplicationRequest could not be queued for ApplicationId={0}", applicationDetail.EntityId);

                throw new CustomException("SubmitApplicationRequest could not be queued", ex,
                    MessagingErrorCodes.ApplicationQueuingError);
            }
        }

        private void PublishMessage(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            try
            {
                var message = new SubmitApprenticeshipApplicationRequest
                {
                    ApplicationId = apprenticeshipApplicationDetail.EntityId
                };

                _serviceBus.PublishMessage(message);
            }
            catch
            {
                apprenticeshipApplicationDetail.RevertStateToDraft();
                _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplicationDetail);
                throw;
            }
        }

        private void NotifyCandidate(Guid candidateId, string applicationId)
        {
            _communicationService.SendMessageToCandidate(candidateId, MessageTypes.ApprenticeshipApplicationSubmitted,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.ApplicationId, applicationId)
                });
        }
    }
}