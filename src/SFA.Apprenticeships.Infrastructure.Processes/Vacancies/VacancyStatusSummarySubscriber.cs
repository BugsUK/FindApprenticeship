namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using Application.Applications;
    using Application.Applications.Entities;
    using Configuration;
    using SFA.Infrastructure.Interfaces.Caching;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Messaging;
    using Extensions;

    public class VacancyStatusSummarySubscriber : IServiceBusSubscriber<VacancyStatusSummary>
    {
        private readonly bool _enableVacancyStatusPropagation;
        private readonly ICacheService _cacheService;
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;

        public VacancyStatusSummarySubscriber(
            IConfigurationService configurationService,
            ICacheService cacheService,
            IApplicationStatusProcessor applicationStatusProcessor)
        {
            _cacheService = cacheService;
            _applicationStatusProcessor = applicationStatusProcessor;
            _enableVacancyStatusPropagation = configurationService.Get<ProcessConfiguration>().EnableVacancyStatusPropagation;
        }

        [ServiceBusTopicSubscription(TopicName = "UpdateApprenticeshipVacancyStatus")]
        public ServiceBusMessageStates Consume(VacancyStatusSummary message)
        {
            // TODO: AG: ASB: refactor logic and unit test.
            if (!_enableVacancyStatusPropagation) return ServiceBusMessageStates.Complete;

            var cachedSummaryUpdate = _cacheService.Get<VacancyStatusSummary>(message.CacheKey());

            if (cachedSummaryUpdate != null)
            {
                // This vacancy has already been processed so return to prevent endless reprocessing.
                return ServiceBusMessageStates.Complete;
            }

            _cacheService.PutObject(message.CacheKey(), message, message.CacheDuration());
            _applicationStatusProcessor.ProcessApplicationStatuses(message);

            return ServiceBusMessageStates.Complete;
        }
    }
}
