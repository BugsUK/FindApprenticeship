namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
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
            var vacancies = _vacancyReadRepository.GetWithStatus(PageSize, pageNumber - 1, _desiredStatuses);
            var vacancyParties = _providerService.GetVacancyParties(vacancies.Select(v => v.OwnerPartyId).Distinct()).ToDictionary(vp => vp.VacancyPartyId, vp => vp);
            var employers = _employerService.GetEmployers(vacancyParties.Values.Select(v => v.EmployerId).Distinct()).ToDictionary(e => e.EmployerId, e => e);
            var providerSites = _providerService.GetProviderSites(vacancyParties.Values.Select(v => v.ProviderSiteId).Distinct()).ToDictionary(ps => ps.ProviderSiteId, ps => ps);
            var providers = _providerService.GetProviders(providerSites.Values.Select(v => v.ProviderId).Distinct()).ToDictionary(p => p.ProviderId, p => p);
            var categories = _referenceDataProvider.GetCategories().ToList();
            var apprenticeshipSummaries =
                vacancies.Where(v => v.VacancyType == VacancyType.Apprenticeship).Select(
                    v =>
                        ApprenticeshipSummaryMapper.GetApprenticeshipSummary(v,
                            employers[vacancyParties[v.OwnerPartyId].EmployerId],
                            providers[providerSites[vacancyParties[v.OwnerPartyId].ProviderSiteId].ProviderId],
                            categories, _logService));
            var traineeshipSummaries =
                vacancies.Where(v => v.VacancyType == VacancyType.Traineeship).Select(
                    v =>
                        TraineeshipSummaryMapper.GetTraineeshipSummary(v,
                            employers[vacancyParties[v.OwnerPartyId].EmployerId],
                            providers[providerSites[vacancyParties[v.OwnerPartyId].ProviderSiteId].ProviderId],
                            categories, _logService));
            return new VacancySummaries(apprenticeshipSummaries.Where(s => s != null), traineeshipSummaries.Where(s => s != null));
        }
    }
}