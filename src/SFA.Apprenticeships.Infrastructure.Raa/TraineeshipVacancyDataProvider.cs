﻿namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Vacancies;
    using Application.ReferenceData;
    using Application.Vacancy;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Raa.Interfaces.Repositories;
    using Mappers;

    using SFA.Apprenticeships.Application.Interfaces;
    using ErrorCodes = Application.Interfaces.Vacancies.ErrorCodes;

    public class TraineeshipVacancyDataProvider : IVacancyDataProvider<TraineeshipVacancyDetail>
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IProviderService _providerService;
        private readonly IEmployerService _employerService;
        private readonly IReferenceDataProvider _referenceDataProvider;
        private readonly ILogService _logService;

        public TraineeshipVacancyDataProvider(IVacancyReadRepository vacancyReadRepository, IProviderService providerService, IEmployerService employerService, IReferenceDataProvider referenceDataProvider, ILogService logService)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _providerService = providerService;
            _employerService = employerService;
            _referenceDataProvider = referenceDataProvider;
            _logService = logService;
        }

        public TraineeshipVacancyDetail GetVacancyDetails(int vacancyId, bool errorIfNotFound = false)
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

            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false); // Some current vacancies have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, false);

            var providerSite = _providerService.GetProviderSite(vacancyOwnerRelationship.ProviderSiteId);
            if (providerSite == null)
                throw new System.Exception($"Could not find VacancyOwnerRelationship for ProviderSiteId={vacancyOwnerRelationship.ProviderSiteId}");

            var provider = _providerService.GetProvider(vacancy.ContractOwnerId);
            var categories = _referenceDataProvider.GetCategories();
            return TraineeshipVacancyDetailMapper.GetTraineeshipVacancyDetail(vacancy, employer, provider, providerSite, categories, _logService);
        }

        public int GetVacancyId(int vacancyReferenceNumber)
        {
            return _vacancyReadRepository.GetVacancyIdByReferenceNumber(vacancyReferenceNumber);
        }
    }
}