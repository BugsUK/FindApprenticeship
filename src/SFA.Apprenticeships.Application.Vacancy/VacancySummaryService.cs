namespace SFA.Apprenticeships.Application.Vacancy
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;

    public class VacancySummaryService : IVacancySummaryService
    {
        private readonly IVacancySummaryRepository _vacancySummaryRepository;

        public VacancySummaryService(IVacancySummaryRepository vacancySummaryRepository)
        {
            _vacancySummaryRepository = vacancySummaryRepository;
        }

        public IList<VacancySummary> GetSummariesForProvider(VacancySummaryQuery query, out int totalRecords)
        {
            return _vacancySummaryRepository.GetSummariesForProvider(query, out totalRecords);
        }

        public VacancyCounts GetLotteryCounts(VacancySummaryQuery query)
        {
            return _vacancySummaryRepository.GetLotteryCounts(query);
        }
    }
}