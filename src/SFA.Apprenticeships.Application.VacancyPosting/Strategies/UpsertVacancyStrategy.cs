namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces;
    using System;

    public class UpsertVacancyStrategy : IUpsertVacancyStrategy
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IAuthoriseCurrentUserStrategy _authoriseCurrentUserStrategy;
        private readonly IPublishVacancySummaryUpdateStrategy _publishVacancySummaryUpdateStrategy;

        public UpsertVacancyStrategy(
            ICurrentUserService currentUserService,
            IProviderUserReadRepository providerUserReadRepository,
            IVacancyReadRepository vacancyReadRepository,
            IAuthoriseCurrentUserStrategy authoriseCurrentUserStrategy,
            IPublishVacancySummaryUpdateStrategy publishVacancySummaryUpdateStrategy)
        {
            _currentUserService = currentUserService;
            _providerUserReadRepository = providerUserReadRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _authoriseCurrentUserStrategy = authoriseCurrentUserStrategy;
            _publishVacancySummaryUpdateStrategy = publishVacancySummaryUpdateStrategy;
        }

        public Vacancy UpsertVacancy(Vacancy vacancy, Func<Vacancy, Vacancy> operation)
        {
            Condition.Requires(vacancy);

            _authoriseCurrentUserStrategy.AuthoriseCurrentUser(vacancy);

            if (_currentUserService.IsInRole(Roles.Faa))
            {
                var username = _currentUserService.CurrentUserName;
                var lastEditedBy = _providerUserReadRepository.GetByUsername(username);
                if (lastEditedBy != null)
                {
                    vacancy.LastEditedById = lastEditedBy.ProviderUserId;
                }
            }

            vacancy = operation(vacancy);

            _publishVacancySummaryUpdateStrategy.PublishVacancySummaryUpdate(vacancy);

            return _vacancyReadRepository.Get(vacancy.VacancyId);
        }

        public Vacancy UpsertVacancyForAdmin(Vacancy vacancy, Func<Vacancy, Vacancy> operation)
        {
            Condition.Requires(vacancy);
            if (_currentUserService.IsInRole(Roles.Faa))
            {
                var username = _currentUserService.CurrentUserName;
                var lastEditedBy = _providerUserReadRepository.GetByUsername(username);
                if (lastEditedBy != null)
                {
                    vacancy.LastEditedById = lastEditedBy.ProviderUserId;
                }
            }
            vacancy = operation(vacancy);
            _publishVacancySummaryUpdateStrategy.PublishVacancySummaryUpdate(vacancy);
            return _vacancyReadRepository.Get(vacancy.VacancyId);
        }
    }
}