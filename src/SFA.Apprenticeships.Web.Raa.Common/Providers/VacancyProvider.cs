﻿using System.Collections.Generic;
using System.Linq;
using SFA.Apprenticeships.Application.Interfaces.DateTime;
using SFA.Apprenticeships.Application.Interfaces.Providers;
using SFA.Apprenticeships.Application.Interfaces.ReferenceData;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
using SFA.Apprenticeships.Domain.Interfaces.Configuration;
using SFA.Apprenticeships.Domain.Interfaces.Repositories;
using SFA.Apprenticeships.Web.Raa.Common.Configuration;
using SFA.Apprenticeships.Web.Raa.Common.Converters;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    public class VacancyProvider : IVacancyProvider
    {
        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
        private readonly IApprenticeshipVacancyWriteRepository _apprenticeshipVacancyWriteRepository;
        private readonly IProviderService _providerService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IReferenceDataService _referenceDataService;
        private readonly IConfigurationService _configurationService;

        public VacancyProvider(IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository,
                IApprenticeshipVacancyWriteRepository apprenticeshipVacancyWriteRepository,
                IProviderService providerService, IDateTimeService dateTimeService,
                IReferenceDataService referenceDataService,
                IConfigurationService configurationService)
        {
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
            _apprenticeshipVacancyWriteRepository = apprenticeshipVacancyWriteRepository;
            _providerService = providerService;
            _dateTimeService = dateTimeService;
            _referenceDataService = referenceDataService;
            _configurationService = configurationService;
        }

        public List<VacancyViewModel> GetVacanciesForProvider(string ukprn)
        {
            var vacancies = _apprenticeshipVacancyReadRepository.GetForProvider(ukprn,
                new List<ProviderVacancyStatuses> {ProviderVacancyStatuses.Draft});

            return vacancies.Select(v => v.ConvertToVacancyViewModel()).ToList();
        }

        public List<DashboardVacancySummaryViewModel> GetPendingQAVacancies()
        {
            var vacancies =
                _apprenticeshipVacancyReadRepository.GetWithStatus(new List<ProviderVacancyStatuses>
                {
                        ProviderVacancyStatuses.PendingQA
                });

            return vacancies.Where(VacancyReadyToQA).Select(ConvertToDashboardVacancySummaryViewModel).ToList();
        }

        private bool VacancyReadyToQA(ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var timeout = _configurationService.Get<ManageWebConfiguration>().QAVacancyTimeout; //In minutes
            if (NoUserHasStartedToQATheVacancy(apprenticeshipVacancy))
            {
                return true;
            }

            if (AUserHasLeftTheVacancyUnattended(apprenticeshipVacancy, timeout))
            {
                return true;
            }

            return false;
        }

        private bool AUserHasLeftTheVacancyUnattended(ApprenticeshipVacancy apprenticeshipVacancy, int timeout)
        {
            return (_dateTimeService.UtcNow() - apprenticeshipVacancy.DateStartedToQA).Value.TotalMinutes > timeout;
        }

        private static bool NoUserHasStartedToQATheVacancy(ApprenticeshipVacancy apprenticeshipVacancy)
        {
            return string.IsNullOrWhiteSpace(apprenticeshipVacancy.QAUserName) || !apprenticeshipVacancy.DateStartedToQA.HasValue;
        }

        public void ApproveVacancy(long vacancyReferenceNumber)
        {
            var vacancy = _apprenticeshipVacancyReadRepository.Get(vacancyReferenceNumber);
            vacancy.Status = ProviderVacancyStatuses.Live;

            _apprenticeshipVacancyWriteRepository.Save(vacancy);
        }

        public void RejectVacancy(long vacancyReferenceNumber)
        {
            var vacancy = _apprenticeshipVacancyReadRepository.Get(vacancyReferenceNumber);
            vacancy.Status = ProviderVacancyStatuses.Draft;

            _apprenticeshipVacancyWriteRepository.Save(vacancy);
        }

        public VacancyViewModel GetVacancy(long vacancyReferenceNumber)
        {
            var vacancy = _apprenticeshipVacancyReadRepository.Get(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToVacancyViewModel();
            var providerSite = _providerService.GetProviderSite(vacancy.Ukprn, vacancy.ProviderSiteEmployerLink.ProviderSiteErn);
            viewModel.ProviderSite = providerSite.Convert();
            viewModel.FrameworkName = string.IsNullOrEmpty(vacancy.FrameworkCodeName) ? vacancy.FrameworkCodeName : _referenceDataService.GetSubCategoryByCode(vacancy.FrameworkCodeName).FullName;
            var standard = GetStandard(vacancy.StandardId);
            viewModel.StandardName = standard == null ? "" : standard.Name;
            return viewModel;
        }

        private StandardViewModel GetStandard(int? standardId)
        {
            if (!standardId.HasValue) return null;

            var sectors = _referenceDataService.GetSectors().ToList();
            var standard = sectors.SelectMany(s => s.Standards).First(s => s.Id == standardId.Value);
            var sector = sectors.First(s => s.Id == standard.ApprenticeshipSectorId);
            return standard.Convert(sector);
        }

        private DashboardVacancySummaryViewModel ConvertToDashboardVacancySummaryViewModel(ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var provider = _providerService.GetProvider(apprenticeshipVacancy.Ukprn);

            return new DashboardVacancySummaryViewModel
            {
                ClosingDate = apprenticeshipVacancy.ClosingDate.Value,
                DateSubmitted = apprenticeshipVacancy.DateSubmitted.Value,
                ProviderName = provider.Name,
                Status = apprenticeshipVacancy.Status,
                Title = apprenticeshipVacancy.Title,
                VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber,
                DateStartedToQA = apprenticeshipVacancy.DateStartedToQA,
                QAUserName = apprenticeshipVacancy.QAUserName
            };
        }
    }
}