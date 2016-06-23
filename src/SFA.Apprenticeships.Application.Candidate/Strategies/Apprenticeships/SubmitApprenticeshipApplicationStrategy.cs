namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Infrastructure.Interfaces;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using MessagingErrorCodes = Interfaces.Messaging.ErrorCodes;

    public class SubmitApprenticeshipApplicationStrategy : ISubmitApprenticeshipApplicationStrategy
    {
        private readonly ILogService _logger;
        private readonly IServiceBus _serviceBus;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ICommunicationService _communicationService;

        public SubmitApprenticeshipApplicationStrategy(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ICommunicationService communicationService, ILogService logger, IServiceBus serviceBus)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _communicationService = communicationService;
            _logger = logger;
            _serviceBus = serviceBus;
        }

        public void SubmitApplication(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId, true);

            applicationDetail.AssertState("Submit apprenticeshipApplication", ApplicationStatuses.Draft);

            try
            {
                applicationDetail.SetStateSubmitting();
                applicationDetail.SetStateSubmitted();

                _apprenticeshipApplicationWriteRepository.Save(applicationDetail);

                PublishMessage(applicationDetail);
                NotifyCandidate(applicationDetail.CandidateId, applicationDetail.EntityId.ToString());
            }
            catch (Exception ex)
            {
                _logger.Debug("SubmitApplicationRequest could not be queued for ApplicationId={0}", applicationDetail.EntityId);

                throw new CustomException("SubmitApplicationRequest could not be queued", ex, MessagingErrorCodes.ApplicationQueuingError);
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
                // No need to revert to draft in Raa if something fails in publishing the message?
                // Should we shallow the exception?

                //apprenticeshipApplicationDetail.RevertStateToDraft();
                //_apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplicationDetail);
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