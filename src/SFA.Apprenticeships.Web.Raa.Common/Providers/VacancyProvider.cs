using System.Collections.Generic;
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
    using System;
    using System.Threading;
    using Application.Interfaces.VacancyPosting;

    public class VacancyProvider : IVacancyProvider
    {
        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
        private readonly IApprenticeshipVacancyWriteRepository _apprenticeshipVacancyWriteRepository;
        private readonly IProviderService _providerService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IReferenceDataService _referenceDataService;
        private readonly IConfigurationService _configurationService;
        private readonly IVacancyPostingService _vacancyPostingService;

        public VacancyProvider(IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository,
                IApprenticeshipVacancyWriteRepository apprenticeshipVacancyWriteRepository,
                IProviderService providerService, IDateTimeService dateTimeService,
                IReferenceDataService referenceDataService,
                IConfigurationService configurationService,
                IVacancyPostingService vacancyPostingService)
        {
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
            _apprenticeshipVacancyWriteRepository = apprenticeshipVacancyWriteRepository;
            _providerService = providerService;
            _dateTimeService = dateTimeService;
            _referenceDataService = referenceDataService;
            _configurationService = configurationService;
            _vacancyPostingService = vacancyPostingService;
        }

        public List<VacancyViewModel> GetVacanciesForProvider(string ukprn)
        {
            var vacancies = _apprenticeshipVacancyReadRepository.GetForProvider(ukprn,
                new List<ProviderVacancyStatuses> {ProviderVacancyStatuses.Draft});

            return vacancies.Select(v => v.ConvertToVacancyViewModel()).ToList();
        }

        public List<DashboardVacancySummaryViewModel> GetPendingQAVacanciesOverview()
        {
            var vacancies =
                _apprenticeshipVacancyReadRepository.GetWithStatus(new List<ProviderVacancyStatuses>
                {
                        ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA
                });

            return vacancies.Select(ConvertToDashboardVacancySummaryViewModel).ToList();
        }

        public List<DashboardVacancySummaryViewModel> GetPendingQAVacancies()
        {
            return GetPendingQAVacanciesOverview().Where(vm => vm.CanBeReservedForQaByCurrentUser).ToList();
        }

        private bool CanBeReservedForQaByCurrentUser(ApprenticeshipVacancy apprenticeshipVacancy)
        {
            if (NoUserHasStartedToQATheVacancy(apprenticeshipVacancy))
            {
                return true;
            }

            if (CurrentUserHasStartedToQATheVacancy(apprenticeshipVacancy))
            {
                return true;
            }

            var timeout = _configurationService.Get<ManageWebConfiguration>().QAVacancyTimeout; //In minutes
            if (AUserHasLeftTheVacancyUnattended(apprenticeshipVacancy, timeout))
            {
                return true;
            }

            return false;
        }

        private bool AUserHasLeftTheVacancyUnattended(ApprenticeshipVacancy apprenticeshipVacancy, int timeout)
        {
            return apprenticeshipVacancy.Status == ProviderVacancyStatuses.ReservedForQA && (_dateTimeService.UtcNow() - apprenticeshipVacancy.DateStartedToQA).Value.TotalMinutes > timeout;
        }

        private static bool NoUserHasStartedToQATheVacancy(ApprenticeshipVacancy apprenticeshipVacancy)
        {
            return apprenticeshipVacancy.Status == ProviderVacancyStatuses.PendingQA && (string.IsNullOrWhiteSpace(apprenticeshipVacancy.QAUserName) || !apprenticeshipVacancy.DateStartedToQA.HasValue);
        }

        private bool CurrentUserHasStartedToQATheVacancy(ApprenticeshipVacancy apprenticeshipVacancy)
        {
            return apprenticeshipVacancy.Status == ProviderVacancyStatuses.ReservedForQA && apprenticeshipVacancy.QAUserName == Thread.CurrentPrincipal.Identity.Name;
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

        public VacancyViewModel ReserveVacancyForQA(long vacancyReferenceNumber)
        {
            var username = Thread.CurrentPrincipal.Identity.Name;
            var vacancy = _apprenticeshipVacancyWriteRepository.ReserveVacancyForQA(vacancyReferenceNumber, username);
            //TODO: Cope with null, interprit as already reserved etc.
            var viewModel = vacancy.ConvertToVacancyViewModel();
            var providerSite = _providerService.GetProviderSite(vacancy.Ukprn, vacancy.ProviderSiteEmployerLink.ProviderSiteErn);
            viewModel.ProviderSite = providerSite.Convert();
            viewModel.FrameworkName = string.IsNullOrEmpty(vacancy.FrameworkCodeName) ? vacancy.FrameworkCodeName : _referenceDataService.GetSubCategoryByCode(vacancy.FrameworkCodeName).FullName;
            var standard = GetStandard(vacancy.StandardId);
            viewModel.StandardName = standard == null ? "" : standard.Name;
            return viewModel;
        }

        public VacancySummaryViewModel UpdateVacancy(VacancySummaryViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyReferenceNumber);

            //TODO: add comment stuff
            vacancy.WorkingWeek = viewModel.WorkingWeek;
            vacancy.HoursPerWeek = viewModel.HoursPerWeek;
            vacancy.WageType = viewModel.WageType;
            vacancy.Wage = viewModel.Wage;
            vacancy.WageUnit = viewModel.WageUnit;
            vacancy.DurationType = viewModel.DurationType;
            vacancy.Duration = viewModel.Duration.HasValue ? (int?)Math.Round(viewModel.Duration.Value) : null;

            if (viewModel.ClosingDate.HasValue)
            {
                vacancy.ClosingDate = viewModel.ClosingDate?.Date;
            }
            if (viewModel.PossibleStartDate.HasValue)
            {
                vacancy.PossibleStartDate = viewModel.PossibleStartDate?.Date;
            }

            vacancy.LongDescription = viewModel.LongDescription;

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancySummaryViewModel();
            return viewModel;
        }

        public NewVacancyViewModel UpdateVacancy(NewVacancyViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public VacancyRequirementsProspectsViewModel UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel)
        {
            throw new NotImplementedException();
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
                QAUserName = apprenticeshipVacancy.QAUserName,
                CanBeReservedForQaByCurrentUser = CanBeReservedForQaByCurrentUser(apprenticeshipVacancy)
            };
        }
    }
}