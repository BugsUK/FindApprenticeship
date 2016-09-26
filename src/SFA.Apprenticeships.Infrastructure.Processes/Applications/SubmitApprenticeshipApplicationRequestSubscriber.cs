namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using System;
    using Application.Candidate;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Application.Interfaces;

    public class SubmitApprenticeshipApplicationRequestSubscriber : IServiceBusSubscriber<SubmitApprenticeshipApplicationRequest>
    {
        private readonly ILogService _logger;

        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;

        public SubmitApprenticeshipApplicationRequestSubscriber(
            ILogService logger,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository)
        {
            _logger = logger;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
        }

        [ServiceBusTopicSubscription(TopicName = "SubmitApprenticeshipApplication")]
        public ServiceBusMessageStates Consume(SubmitApprenticeshipApplicationRequest request)
        {
            return CreateApplication(request);
        }

        private ServiceBusMessageStates CreateApplication(SubmitApprenticeshipApplicationRequest request)
        {
            _logger.Debug("Creating apprenticeship application Id: {0}", request.ApplicationId);

            var applicationDetail = _apprenticeshipApplicationReadRepository.Get(request.ApplicationId, true);

            try
            {
                SetApplicationStateSubmitted(applicationDetail);
                return ServiceBusMessageStates.Complete;
            }
            catch (Exception e)
            {
                _logger.Error("Submit apprenticeship application with Id = {0} request async process failed, message will be requeued",
                    e, request.ApplicationId);

                return ServiceBusMessageStates.Requeue;
            }
        }

        private void SetApplicationStateSubmitted(ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            apprenticeshipApplication.SetStateSubmitted();
            _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);
        }
    }
}
