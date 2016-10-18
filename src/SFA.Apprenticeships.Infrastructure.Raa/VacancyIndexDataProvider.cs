namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using System;
    using System.Linq;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.ReferenceData;
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Mappers;
    using Application.Interfaces;

    /// <summary>
    /// TODO: This class will eventually use an RAA service for the data rather than referencing repositories directly.
    /// This service does not exist yet and so the simplest approach has been used for now
    /// </summary>
    public class VacancyIndexDataProvider : IVacancyIndexDataProvider
    {
        private const int PageSize = 500;
        private readonly VacancyStatus[] _desiredStatuses = {VacancyStatus.Live};

        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IProviderService _providerService;
        private readonly IEmployerService _employerService;
        private readonly IReferenceDataProvider _referenceDataProvider;
        private readonly ILogService _logService;

        public VacancyIndexDataProvider(IVacancyReadRepository vacancyReadRepository, IProviderService providerService, IEmployerService employerService, IReferenceDataProvider referenceDataProvider, ILogService logService)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _providerService = providerService;
            _employerService = employerService;
            _referenceDataProvider = referenceDataProvider;
            _logService = logService;
        }

        public int GetVacancyPageCount()
        {
            var count = _vacancyReadRepository.CountWithStatus(_desiredStatuses);
            var pageCount = count/PageSize;
            if (count%PageSize != 0)
            {
                pageCount++;
            }
            return pageCount;
        }

        public VacancySummaries GetVacancySummaries(int pageNumber)
        {
            //Page number coming in increments from 1 rather than 0, the repo expects pages to start at 0 so take one from the passed in value
            var vacancies = _vacancyReadRepository.GetWithStatus(PageSize, pageNumber - 1, false, _desiredStatuses);
            var vacancyParties = _providerService.GetVacancyOwnerRelationships(vacancies.Select(v => v.VacancyOwnerRelationshipId).Distinct(), false);
            var employers = _employerService.GetEmployers(vacancyParties.Values.Select(v => v.EmployerId).Distinct()).ToDictionary(e => e.EmployerId, e => e);
            var providers = _providerService.GetProviders(vacancies.Select(v => v.ContractOwnerId).Distinct()).ToDictionary(p => p.ProviderId, p => p);
            var categories = _referenceDataProvider.GetCategories().ToList();
            //TODO: workaround to have the indexing partially working. Should be done properly
            var apprenticeshipSummaries =
                vacancies.Where(v => v.VacancyType == VacancyType.Apprenticeship).Select(
                    v =>
                    {
                        try
                        {
                            return ApprenticeshipSummaryMapper.GetApprenticeshipSummary(v,
                                employers[vacancyParties[v.VacancyOwnerRelationshipId].EmployerId],
                                providers[v.ContractOwnerId],
                                categories, _logService);
                        }
                        catch (Exception ex)
                        {
                            _logService.Error($"Error indexing the apprenticeship vacancy with ID={v.VacancyId}", ex);
                            return null;
                        }
                    });

            var traineeshipSummaries =
                vacancies.Where(v => v.VacancyType == VacancyType.Traineeship).Select(
                    v =>
                    {
                        try
                        {
                            return TraineeshipSummaryMapper.GetTraineeshipSummary(v,
                                employers[vacancyParties[v.VacancyOwnerRelationshipId].EmployerId],
                                providers[v.ContractOwnerId],
                                categories, _logService);
                        }
                        catch (Exception ex)
                        {
                            _logService.Error($"Error indexing the traineeship vacancy with ID={v.VacancyId}", ex);
                            return null;
                        }
                    });

            return new VacancySummaries(apprenticeshipSummaries.Where(s => s != null), traineeshipSummaries.Where(s => s != null));
        }
    }
}