namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Vacancies.Entities;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class VacancyAboutToExpireSubscriber : IServiceBusSubscriber<VacancyAboutToExpire>
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IExpiringApprenticeshipApplicationDraftRepository _expiringDraftRepository;
        private readonly IMapper _mapper;

        public VacancyAboutToExpireSubscriber(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IExpiringApprenticeshipApplicationDraftRepository expiringDraftRepository,
            IMapper mapper)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _expiringDraftRepository = expiringDraftRepository;
            _mapper = mapper;
        }

        [ServiceBusTopicSubscription(TopicName = "vacancy-about-to-expire")]
        public ServiceBusMessageResult Consume(VacancyAboutToExpire vacancy)
        {
            // Get saved and draft applications for expiring vacancy
            var expiringApplications = _apprenticeshipApplicationReadRepository
                .GetApplicationSummaries(vacancy.Id)
                .Where(v => v.Status == ApplicationStatuses.Saved || v.Status == ApplicationStatuses.Draft)
                .ToList();

            if (!expiringApplications.Any())
            {
                return ServiceBusMessageResult.Complete();
            }

            // Map to expiring draft model
            var expiringDrafts = _mapper
                .Map<IEnumerable<ApprenticeshipApplicationSummary>, IEnumerable<ExpiringApprenticeshipApplicationDraft>>(expiringApplications)
                .ToList();

            // Get those already notified or marked for notification
            var alreadyExpiringDrafts = _expiringDraftRepository.GetExpiringApplications(vacancy.Id);

            // Get those that haven't been added to expiring repo
            var newExpiringDrafts = expiringDrafts
                .Where(e => !alreadyExpiringDrafts.Select(ev => ev.EntityId).Contains(e.EntityId))
                .ToList();

            // Write to repo
            newExpiringDrafts.ForEach(_expiringDraftRepository.Save);

            return ServiceBusMessageResult.Complete();
        }
    }
}
