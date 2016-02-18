namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Vacancies;
    using Application.ReferenceData;
    using Application.Vacancy;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Raa.Interfaces.Repositories;
    using Mappers;

    public class ApprenticeshipVacancyDataProvider : IVacancyDataProvider<ApprenticeshipVacancyDetail>
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IReferenceDataProvider _referenceDataProvider;
        private readonly ILogService _logService;

        public ApprenticeshipVacancyDataProvider(IVacancyReadRepository vacancyReadRepository, IReferenceDataProvider referenceDataProvider, ILogService logService)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _referenceDataProvider = referenceDataProvider;
            _logService = logService;
        }

        public ApprenticeshipVacancyDetail GetVacancyDetails(int vacancyId, bool errorIfNotFound = false)
        {
            var vacancy = _vacancyReadRepository.Get(vacancyId);

            if (vacancy == null)
            {
                if (errorIfNotFound)
                {
                    throw new DomainException(ErrorCodes.VacancyNotFoundError, new { vacancyId });
                }
                return null;
            }

            var categories = _referenceDataProvider.GetCategories();
            return ApprenticeshipVacancyDetailMapper.GetApprenticeshipVacancyDetail(vacancy, categories, _logService);
        }
    }
}