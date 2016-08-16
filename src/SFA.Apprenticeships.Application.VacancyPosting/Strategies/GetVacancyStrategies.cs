namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;

    public class GetVacancyStrategies : IGetVacancyStrategies
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IAuthoriseCurrentUserStrategy _authoriseCurrentUserStrategy;

        public GetVacancyStrategies(IVacancyReadRepository vacancyReadRepository, IAuthoriseCurrentUserStrategy authoriseCurrentUserStrategy)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _authoriseCurrentUserStrategy = authoriseCurrentUserStrategy;
        }

        public Vacancy GetVacancyByReferenceNumber(int vacancyReferenceNumber)
        {
            return _authoriseCurrentUserStrategy.AuthoriseCurrentUser(_vacancyReadRepository.GetByReferenceNumber(vacancyReferenceNumber));
        }

        public Vacancy GetVacancyByGuid(Guid vacancyGuid)
        {
            return _authoriseCurrentUserStrategy.AuthoriseCurrentUser(_vacancyReadRepository.GetByVacancyGuid(vacancyGuid));
        }

        public Vacancy GetVacancyById(int vacancyId)
        {
            return _authoriseCurrentUserStrategy.AuthoriseCurrentUser(_vacancyReadRepository.Get(vacancyId));
        }
    }
}