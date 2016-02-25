namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Application.ReferenceData;
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Raa.Interfaces.Repositories;
    using Mappers;

    /// <summary>
    /// TODO: This class will eventually use an RAA service for the data rather than referencing repositories directly.
    /// This service does not exist yet and so the simplest approach has been used for now
    /// </summary>
    public class VacancyIndexDataProvider : IVacancyIndexDataProvider
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IReferenceDataProvider _referenceDataProvider;
        private readonly ILogService _logService;

        public VacancyIndexDataProvider(IVacancyReadRepository vacancyReadRepository, IReferenceDataProvider referenceDataProvider, ILogService logService)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _referenceDataProvider = referenceDataProvider;
            _logService = logService;
        }

        public int GetVacancyPageCount()
        {
            //We're returning all live vacancies as a single page for now to simplify things.
            //TODO: Implement robust paging as a stretch goal
            return 1;
        }

        public VacancySummaries GetVacancySummaries(int pageNumber)
        {
            var vacancies = _vacancyReadRepository.GetWithStatus(VacancyStatus.Live);
            var categories = _referenceDataProvider.GetCategories();
            var vacancySummaries = vacancies.Select(v => ApprenticeshipSummaryMapper.GetApprenticeshipSummary(v, categories, _logService));
            return new VacancySummaries(vacancySummaries, new List<TraineeshipSummary>());
        }
    }
}