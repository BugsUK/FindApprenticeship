namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Interfaces.Repositories;
    using Interfaces.Vacancies;

    public class OfflineVacancyService : IOfflineVacancyService
    {
        private readonly IOfflineApprenticeshipVacancyRepository _offlineApprenticeshipVacancyRepository;

        public OfflineVacancyService(IOfflineApprenticeshipVacancyRepository offlineApprenticeshipVacancyRepository)
        {
            _offlineApprenticeshipVacancyRepository = offlineApprenticeshipVacancyRepository;
        }

        public void IncrementOfflineApplicationClickThrough(long vacancyReferenceNumber)
        {
            _offlineApprenticeshipVacancyRepository.IncrementOfflineApplicationClickThrough(vacancyReferenceNumber);
        }
    }
}