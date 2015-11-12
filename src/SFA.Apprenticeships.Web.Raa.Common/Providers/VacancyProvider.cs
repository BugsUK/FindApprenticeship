using System.Web.Mvc;
using SFA.Apprenticeships.Web.Common.Configuration;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Threading;
    using Application.Interfaces.VacancyPosting;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.DateTime;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Configuration;
    using Converters;
    using ViewModels;
    using ViewModels.Vacancy;
	using ViewModels.ProviderUser;
	using Web.Common.ViewModels;

    public class VacancyProvider : IVacancyProvider
    {
        //TODO: Providers aren't really supposed to reference repositories directly, they are supposed to use services at least with the current architecture
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

        public List<VacancyViewModel> GetVacanciesForProvider(string ukprn, string providerSiteErn)
        {
            var vacancies = _apprenticeshipVacancyReadRepository.GetForProvider(ukprn, providerSiteErn);

            return vacancies.Select(v => v.ConvertToVacancyViewModel()).ToList();
        }

        public VacanciesSummaryViewModel GetVacanciesSummaryForProvider(string ukprn, string providerSiteErn, VacanciesSummarySearchViewModel vacanciesSummarySearch)
        {
            //TODO: This filtering, aggregation and pagination should be done in the DAL once we've moved over to SQL Server
            var vacancies = _apprenticeshipVacancyReadRepository.GetForProvider(ukprn, providerSiteErn);

            var live = vacancies.Where(v => v.Status == ProviderVacancyStatuses.Live).ToList();
            //TODO: make approved timespan configurable
            var approved = vacancies.Where(v => v.Status == ProviderVacancyStatuses.Live && v.DateQAApproved.HasValue && v.DateQAApproved > _dateTimeService.UtcNow().AddHours(-24)).ToList();
            var rejected = vacancies.Where(v => v.Status == ProviderVacancyStatuses.RejectedByQA).ToList();
            //TODO: Agree on closing soon range and make configurable
            var closingSoon = vacancies.Where(v => v.Status == ProviderVacancyStatuses.Live && v.ClosingDate.HasValue && v.ClosingDate > _dateTimeService.UtcNow() && v.ClosingDate.Value.AddDays(-3) < _dateTimeService.UtcNow()).ToList();
            var closed = vacancies.Where(v => v.Status == ProviderVacancyStatuses.Live && v.ClosingDate.HasValue && v.ClosingDate < _dateTimeService.UtcNow()).ToList();
            //TODO: Does this include the one's in QA at the moment?
            var draft = vacancies.Where(v => v.Status == ProviderVacancyStatuses.Draft).ToList();

            switch (vacanciesSummarySearch.FilterType)
            {
                case VacanciesSummaryFilterTypes.Live:
                    vacancies = live;
                    break;
                case VacanciesSummaryFilterTypes.Approved:
                    vacancies = approved;
                    break;
                case VacanciesSummaryFilterTypes.Rejected:
                    vacancies = rejected;
                    break;
                case VacanciesSummaryFilterTypes.ClosingSoon:
                    vacancies = closingSoon;
                    break;
                case VacanciesSummaryFilterTypes.Closed:
                    vacancies = closed;
                    break;
                case VacanciesSummaryFilterTypes.Draft:
                    vacancies = draft;
                    break;
            }
            var vacancyPage = new PageableViewModel<VacancyViewModel>
            {
                Page = vacancies.Skip((vacanciesSummarySearch.CurrentPage - 1)*vacanciesSummarySearch.PageSize).Take(vacanciesSummarySearch.PageSize).Select(v => v.ConvertToVacancyViewModel()).ToList(),
                ResultsCount = vacancies.Count,
                CurrentPage = vacanciesSummarySearch.CurrentPage,
                TotalNumberOfPages = (vacancies.Count/vacanciesSummarySearch.PageSize) + 1
            };

            var vacanciesSummary = new VacanciesSummaryViewModel
            {
                VacanciesSummarySearch = vacanciesSummarySearch,
                LiveCount = live.Count,
                ApprovedCount = approved.Count,
                RejectedCount = rejected.Count,
                ClosingSoonCount = closingSoon.Count,
                ClosedCount = closed.Count,
                DraftCount = draft.Count,
                Vacancies = vacancyPage
            };

            return vacanciesSummary;
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
            vacancy.DateQAApproved = _dateTimeService.UtcNow();

            _apprenticeshipVacancyWriteRepository.Save(vacancy);
        }

        public void RejectVacancy(long vacancyReferenceNumber)
        {
            var vacancy = _apprenticeshipVacancyReadRepository.Get(vacancyReferenceNumber);
            vacancy.Status = ProviderVacancyStatuses.RejectedByQA;

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
            // TODO: merge with vacancypostingprovider? -> how we deal with comments. Add them as hidden fields in vacancy posting journey?
            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyReferenceNumber);
            
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

            vacancy.WageComment = viewModel.WageComment;
            vacancy.ClosingDateComment = viewModel.ClosingDateComment;
            vacancy.DurationComment = viewModel.DurationComment;
            vacancy.LongDescriptionComment = viewModel.LongDescriptionComment;
            vacancy.PossibleStartDateComment = viewModel.PossibleStartDateComment;
            vacancy.WorkingWeekComment = viewModel.WorkingWeekComment;

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancySummaryViewModel();
            return viewModel;
        }

        public NewVacancyViewModel UpdateVacancy(NewVacancyViewModel viewModel)
        {
            if (!viewModel.VacancyReferenceNumber.HasValue)
                throw new ArgumentNullException("viewModel.VacancyReferenceNumber", "VacancyReferenceNumber required for update");

            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyReferenceNumber.Value);

            var offlineApplicationUrl = !string.IsNullOrEmpty(viewModel.OfflineApplicationUrl) ? new UriBuilder(viewModel.OfflineApplicationUrl).Uri.ToString() : viewModel.OfflineApplicationUrl;

            //update properties
            vacancy.Ukprn = viewModel.Ukprn;
            vacancy.Title = viewModel.Title;
            vacancy.ShortDescription = viewModel.ShortDescription;
            vacancy.TrainingType = viewModel.TrainingType;
            vacancy.FrameworkCodeName = GetFrameworkCodeName(viewModel);
            vacancy.StandardId = viewModel.StandardId;
            vacancy.ApprenticeshipLevel = GetApprenticeshipLevel(viewModel);
            vacancy.OfflineVacancy = viewModel.OfflineVacancy;
            vacancy.OfflineApplicationUrl = offlineApplicationUrl;
            vacancy.OfflineApplicationInstructions = viewModel.OfflineApplicationInstructions;
            vacancy.ApprenticeshipLevelComment = viewModel.ApprenticeshipLevelComment;
            vacancy.FrameworkCodeNameComment = viewModel.FrameworkCodeNameComment;
            vacancy.OfflineApplicationInstructionsComment = viewModel.OfflineApplicationInstructionsComment;
            vacancy.OfflineApplicationUrlComment = viewModel.OfflineApplicationUrlComment;
            vacancy.ShortDescriptionComment = viewModel.ShortDescriptionComment;
            vacancy.TitleComment = viewModel.TitleComment;

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            viewModel = vacancy.ConvertToNewVacancyViewModel();
            var sectors = GetSectorsAndFrameworks();
            var standards = GetStandards();
            viewModel.SectorsAndFrameworks = sectors;
            viewModel.Standards = standards;
            return viewModel;
        }

        public VacancyRequirementsProspectsViewModel UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyReferenceNumber);

            vacancy.FirstQuestion = viewModel.FirstQuestion;
            vacancy.SecondQuestion = viewModel.SecondQuestion;
            vacancy.FirstQuestionComment = viewModel.FirstQuestionComment;
            vacancy.SecondQuestionComment = viewModel.SecondQuestionComment;

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyQuestionsViewModel();
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
                QAUserName = apprenticeshipVacancy.QAUserName,
                CanBeReservedForQaByCurrentUser = CanBeReservedForQaByCurrentUser(apprenticeshipVacancy)
            };
        }

        private string GetFrameworkCodeName(NewVacancyViewModel newVacancyViewModel)
        {
            return newVacancyViewModel.TrainingType == TrainingType.Standards ? null : newVacancyViewModel.FrameworkCodeName;
        }

        private ApprenticeshipLevel GetApprenticeshipLevel(NewVacancyViewModel newVacancyViewModel)
        {
            var apprenticeshipLevel = newVacancyViewModel.ApprenticeshipLevel;
            if (newVacancyViewModel.TrainingType == TrainingType.Standards)
            {
                var standard = GetStandard(newVacancyViewModel.StandardId);
                apprenticeshipLevel = standard?.ApprenticeshipLevel ?? ApprenticeshipLevel.Unknown;
            }
            return apprenticeshipLevel;
        }

        private List<SelectListItem> GetSectorsAndFrameworks()
        {
            var categories = _referenceDataService.GetCategories();
            var blacklistedCategoryCodes = GetBlacklistedCategoryCodeNames(_configurationService);

            var sectorsAndFrameworkItems = new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "Choose from the list of frameworks"}
            };

            foreach (var sector in categories.Where(category => !blacklistedCategoryCodes.Contains(category.CodeName)))
            {
                if (sector.SubCategories != null)
                {
                    var sectorGroup = new SelectListGroup { Name = sector.FullName };
                    foreach (var framework in sector.SubCategories)
                    {
                        sectorsAndFrameworkItems.Add(new SelectListItem
                        {
                            Group = sectorGroup,
                            Value = framework.CodeName,
                            Text = framework.FullName
                        });
                    }
                }
            }

            return sectorsAndFrameworkItems;
        }

        private List<StandardViewModel> GetStandards()
        {
            var sectors = _referenceDataService.GetSectors();

            return (from sector in sectors
                    from standard in sector.Standards
                    select standard.Convert(sector)).ToList();
        }

        private static string[] GetBlacklistedCategoryCodeNames(IConfigurationService configurationService)
        {
            var blacklistedCategoryCodeNames = configurationService.Get<CommonWebConfiguration>().BlacklistedCategoryCodes;

            if (string.IsNullOrWhiteSpace(blacklistedCategoryCodeNames))
            {
                return new string[] { };
            }

            return blacklistedCategoryCodeNames
                .Split(',')
                .Select(each => each.Trim())
                .ToArray();
        }
    }
}