namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Application.Interfaces.DateTime;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.VacancyPosting;
    using Configuration;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using ViewModels.Vacancy;
    using Web.Common.Configuration;
    using Converters;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using ViewModels;
    using ViewModels.Provider;
    using ViewModels.ProviderUser;
    using Web.Common.ViewModels;


    public class VacancyProvider : IVacancyPostingProvider, IVacancyQAProvider
    {
        private readonly ILogService _logService;

        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IReferenceDataService _referenceDataService;
        private readonly IProviderService _providerService;
        private readonly IDateTimeService _dateTimeService;
        //TODO: Providers aren't really supposed to reference repositories directly, they are supposed to use services at least with the current architecture
        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
        private readonly IApprenticeshipVacancyWriteRepository _apprenticeshipVacancyWriteRepository;
        private readonly IConfigurationService _configurationService;
        private readonly IMapper _mapper;

        public VacancyProvider(ILogService logService, IConfigurationService configurationService, IVacancyPostingService vacancyPostingService, IReferenceDataService referenceDataService, IProviderService providerService, IDateTimeService dateTimeService, IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository, IApprenticeshipVacancyWriteRepository apprenticeshipVacancyWriteRepository, IMapper mapper)
        {
            _logService = logService;
            _vacancyPostingService = vacancyPostingService;
            _referenceDataService = referenceDataService;
            _providerService = providerService;
            _dateTimeService = dateTimeService;
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
            _apprenticeshipVacancyWriteRepository = apprenticeshipVacancyWriteRepository;
            _configurationService = configurationService;
            _mapper = mapper;
        }

        public NewVacancyViewModel GetNewVacancyViewModel(string ukprn, string providerSiteErn, string ern, Guid vacancyGuid)
        {
            var existingVacancy = _vacancyPostingService.GetVacancy(vacancyGuid);
            var sectors = GetSectorsAndFrameworks();
            var standards = GetStandards();

            if (existingVacancy != null)
            {
                var vacancyViewModel = _mapper.Map<ApprenticeshipVacancy, NewVacancyViewModel>(existingVacancy);
                vacancyViewModel.SectorsAndFrameworks = sectors;
                vacancyViewModel.Standards = standards;
                return vacancyViewModel;
            }

            var providerSiteEmployerLink = _providerService.GetProviderSiteEmployerLink(providerSiteErn, ern);
            
            return new NewVacancyViewModel
            {
                Ukprn = ukprn,
                ApprenticeshipLevel = ApprenticeshipLevel.Unknown, //Force a selection
                TrainingType = TrainingType.Unknown, //Force a selection
                SectorsAndFrameworks = sectors,
                Standards = standards,
                ProviderSiteEmployerLink = providerSiteEmployerLink.Convert()
            };
        }

        public NewVacancyViewModel GetNewVacancyViewModel(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);
            var viewModel = _mapper.Map<ApprenticeshipVacancy, NewVacancyViewModel>(vacancy);
            var sectors = GetSectorsAndFrameworks();
            var standards = GetStandards();
            viewModel.SectorsAndFrameworks = sectors;
            viewModel.Standards = standards;
            return viewModel;
        }

        /// <summary>
        /// This method will create a new Vacancy record if the model provided does not have a vacancy reference number.
        /// Otherwise, it updates the existing one.
        /// </summary>
        /// <param name="newVacancyViewModel"></param>
        /// <returns></returns>
        public NewVacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            if (VacancyExists(newVacancyViewModel))
            {
                return UpdateExistingVacancy(newVacancyViewModel);
            }

            _logService.Debug("Creating vacancy reference number");

            try
            {
                var vacancy = CreateNewVacancy(newVacancyViewModel);

                _logService.Debug("Created vacancy with reference number={0}", vacancy.VacancyReferenceNumber);

                return _mapper.Map<ApprenticeshipVacancy, NewVacancyViewModel>(vacancy);
            }
            catch (Exception e)
            {
                _logService.Error("Failed to create vacancy", e);
                throw;
            }
        }

        private ApprenticeshipVacancy CreateNewVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            var offlineApplicationUrl = !string.IsNullOrEmpty(newVacancyViewModel.OfflineApplicationUrl) ? new UriBuilder(newVacancyViewModel.OfflineApplicationUrl).Uri.ToString() : newVacancyViewModel.OfflineApplicationUrl;
            var vacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
            var providerSiteEmployerLink =
                _providerService.GetProviderSiteEmployerLink(newVacancyViewModel.ProviderSiteEmployerLink.ProviderSiteErn,
                    newVacancyViewModel.ProviderSiteEmployerLink.Employer.Ern);

            var vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(new ApprenticeshipVacancy
            {
                EntityId = newVacancyViewModel.VacancyGuid,
                VacancyReferenceNumber = vacancyReferenceNumber,
                Ukprn = newVacancyViewModel.Ukprn,
                Title = newVacancyViewModel.Title,
                ShortDescription = newVacancyViewModel.ShortDescription,
                TrainingType = newVacancyViewModel.TrainingType,
                FrameworkCodeName = GetFrameworkCodeName(newVacancyViewModel),
                StandardId = newVacancyViewModel.StandardId,
                ApprenticeshipLevel = GetApprenticeshipLevel(newVacancyViewModel),
                ProviderSiteEmployerLink = providerSiteEmployerLink,
                Status = ProviderVacancyStatuses.Draft,
                OfflineVacancy = newVacancyViewModel.OfflineVacancy,
                OfflineApplicationUrl = offlineApplicationUrl,
                OfflineApplicationInstructions = newVacancyViewModel.OfflineApplicationInstructions
            });

            return vacancy;
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

        private NewVacancyViewModel UpdateExistingVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            var offlineApplicationUrl = !string.IsNullOrEmpty(newVacancyViewModel.OfflineApplicationUrl) ? new UriBuilder(newVacancyViewModel.OfflineApplicationUrl).Uri.ToString() : newVacancyViewModel.OfflineApplicationUrl;

            var vacancy = _vacancyPostingService.GetVacancy(newVacancyViewModel.VacancyReferenceNumber.Value);

            vacancy.Ukprn = newVacancyViewModel.Ukprn;
            vacancy.Title = newVacancyViewModel.Title;
            vacancy.ShortDescription = newVacancyViewModel.ShortDescription;
            vacancy.TrainingType = newVacancyViewModel.TrainingType;
            vacancy.FrameworkCodeName = GetFrameworkCodeName(newVacancyViewModel);
            vacancy.StandardId = newVacancyViewModel.StandardId;
            vacancy.ApprenticeshipLevel = GetApprenticeshipLevel(newVacancyViewModel);
            vacancy.OfflineVacancy = newVacancyViewModel.OfflineVacancy;
            vacancy.OfflineApplicationUrl = offlineApplicationUrl;
            vacancy.OfflineApplicationInstructions = newVacancyViewModel.OfflineApplicationInstructions;

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            newVacancyViewModel = _mapper.Map<ApprenticeshipVacancy, NewVacancyViewModel>(vacancy);

            return newVacancyViewModel;
        }

        private static bool VacancyExists(NewVacancyViewModel newVacancyViewModel)
        {
            return newVacancyViewModel.VacancyReferenceNumber.HasValue && newVacancyViewModel.VacancyReferenceNumber > 0;
        }

        public VacancySummaryViewModel GetVacancySummaryViewModel(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToVacancySummaryViewModel();
            return viewModel;
        }

        public VacancySummaryViewModel UpdateVacancy(VacancySummaryViewModel viewModel)
        {
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

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancySummaryViewModel();
            return viewModel;
        }

        public VacancyRequirementsProspectsViewModel GetVacancyRequirementsProspectsViewModel(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToVacancyRequirementsProspectsViewModel();
            return viewModel;
        }

        public VacancyRequirementsProspectsViewModel UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyReferenceNumber);

            vacancy.DesiredSkills = viewModel.DesiredSkills;
            vacancy.FutureProspects = viewModel.FutureProspects;
            vacancy.PersonalQualities = viewModel.PersonalQualities;
            vacancy.ThingsToConsider = viewModel.ThingsToConsider;
            vacancy.DesiredQualifications = viewModel.DesiredQualifications;

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyRequirementsProspectsViewModel();
            return viewModel;
        }

        public VacancyQuestionsViewModel GetVacancyQuestionsViewModel(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToVacancyQuestionsViewModel();
            return viewModel;
        }

        public VacancyQuestionsViewModel UpdateVacancy(VacancyQuestionsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyReferenceNumber);

            vacancy.FirstQuestion = viewModel.FirstQuestion;
            vacancy.SecondQuestion = viewModel.SecondQuestion;

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyQuestionsViewModel();
            return viewModel;
        }

        public VacancyViewModel GetVacancy(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyViewModel>(vacancy);
            var providerSite = _providerService.GetProviderSite(vacancy.Ukprn, vacancy.ProviderSiteEmployerLink.ProviderSiteErn);
            viewModel.ProviderSite = providerSite.Convert();
            viewModel.FrameworkName = string.IsNullOrEmpty(vacancy.FrameworkCodeName) ? vacancy.FrameworkCodeName : _referenceDataService.GetSubCategoryByCode(vacancy.FrameworkCodeName).FullName;
            var standard = GetStandard(vacancy.StandardId);
            viewModel.StandardName = standard == null ? "" : standard.Name;
            return viewModel;
        }

        public VacancyViewModel SubmitVacancy(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);

            vacancy.Status = ProviderVacancyStatuses.PendingQA;
            vacancy.DateSubmitted = _dateTimeService.UtcNow();

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            //TODO: should we return this VM or the one returned by GetVacancy?
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyViewModel>(vacancy);

            return viewModel;
        }

        public List<SelectListItem> GetSectorsAndFrameworks()
        {
            var categories = _referenceDataService.GetCategories();

            var sectorsAndFrameworkItems = new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "Choose from the list of frameworks"}
            };

            var blacklistedCategoryCodes = GetBlacklistedCategoryCodeNames(_configurationService);

            foreach (var sector in categories.Where(category => !blacklistedCategoryCodes.Contains(category.CodeName)))
            {
                if (sector.SubCategories != null)
                {
                    var sectorGroup = new SelectListGroup {Name = sector.FullName};
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

        public List<StandardViewModel> GetStandards()
        {
            var sectors = _referenceDataService.GetSectors();

            return (from sector in sectors
                from standard in sector.Standards
                select standard.Convert(sector)).ToList();
        }

        public StandardViewModel GetStandard(int? standardId)
        {
            if (!standardId.HasValue) return null;

            var sectors = _referenceDataService.GetSectors().ToList();
            var standard = sectors.SelectMany(s => s.Standards).First(s => s.Id == standardId.Value);
            var sector = sectors.First(s => s.Id == standard.ApprenticeshipSectorId);
            return standard.Convert(sector);
        }

        #region Helpers

        private static string[] GetBlacklistedCategoryCodeNames(IConfigurationService configurationService)
        {
            var blacklistedCategoryCodeNames = configurationService.Get<CommonWebConfiguration>().BlacklistedCategoryCodes;
            
            if (string.IsNullOrWhiteSpace(blacklistedCategoryCodeNames))
            {
                return new string[] {};
            }

            return blacklistedCategoryCodeNames
                .Split(',')
                .Select(each => each.Trim())
                .ToArray();
        }

        #endregion

        public VacanciesSummaryViewModel GetVacanciesSummaryForProvider(string ukprn, string providerSiteErn,
            VacanciesSummarySearchViewModel vacanciesSummarySearch)
        {
            var isVacancySearch = !string.IsNullOrEmpty(vacanciesSummarySearch.SearchString);
            if (isVacancySearch)
            {
                //When searching the ﬁlters (lottery numbers) are ignored and the search applies to all vacancies
                vacanciesSummarySearch.FilterType = VacanciesSummaryFilterTypes.All;
            }

            //TODO: This filtering, aggregation and pagination should be done in the DAL once we've moved over to SQL Server
            var vacancies = _apprenticeshipVacancyReadRepository.GetForProvider(ukprn, providerSiteErn);

            var live = vacancies.Where(v => v.Status == ProviderVacancyStatuses.Live).ToList();
            var submitted = vacancies.Where(v => v.Status == ProviderVacancyStatuses.PendingQA || v.Status == ProviderVacancyStatuses.ReservedForQA).ToList();
            var rejected = vacancies.Where(v => v.Status == ProviderVacancyStatuses.RejectedByQA).ToList();
            //TODO: Agree on closing soon range and make configurable
            var closingSoon = vacancies.Where(v => v.Status == ProviderVacancyStatuses.Live && v.ClosingDate.HasValue && v.ClosingDate > _dateTimeService.UtcNow() && v.ClosingDate.Value.AddDays(-5) < _dateTimeService.UtcNow()).ToList();
            var closed = vacancies.Where(v => v.Status == ProviderVacancyStatuses.Live && v.ClosingDate.HasValue && v.ClosingDate < _dateTimeService.UtcNow()).ToList();
            //TODO: Does this include the one's in QA at the moment?
            var draft = vacancies.Where(v => v.Status == ProviderVacancyStatuses.Draft).ToList();

            switch (vacanciesSummarySearch.FilterType)
            {
                case VacanciesSummaryFilterTypes.Live:
                    vacancies = live;
                    break;
                case VacanciesSummaryFilterTypes.Submitted:
                    vacancies = submitted;
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

            vacanciesSummarySearch.PageSizes = GetPageSizes(vacanciesSummarySearch.PageSize);

            if (isVacancySearch)
            {
                vacancies = vacancies.Where(v => v.Title.IndexOf(vacanciesSummarySearch.SearchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            var vacancyPage = new PageableViewModel<VacancyViewModel>
            {
                Page = vacancies.OrderByDescending(v => v.DateCreated).Skip((vacanciesSummarySearch.CurrentPage - 1)*vacanciesSummarySearch.PageSize).Take(vacanciesSummarySearch.PageSize).Select(v => _mapper.Map<ApprenticeshipVacancy, VacancyViewModel>(v)).ToList(),
                ResultsCount = vacancies.Count,
                CurrentPage = vacanciesSummarySearch.CurrentPage,
                TotalNumberOfPages = vacancies.Count == 0 ? 1 : (int)Math.Ceiling((double)vacancies.Count/vacanciesSummarySearch.PageSize)
            };

            var vacanciesSummary = new VacanciesSummaryViewModel
            {
                VacanciesSummarySearch = vacanciesSummarySearch,
                LiveCount = live.Count,
                SubmittedCount = submitted.Count,
                RejectedCount = rejected.Count,
                ClosingSoonCount = closingSoon.Count,
                ClosedCount = closed.Count,
                DraftCount = draft.Count,
                Vacancies = vacancyPage
            };

            return vacanciesSummary; 
        }

        public ProviderSiteEmployerLinkViewModel CloneVacancy(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);

            //TODO: control vacancy doesn't exist

            vacancy.VacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
            vacancy.Title = $"(Copy of) {vacancy.Title}";
            vacancy.Status = ProviderVacancyStatuses.Draft;
            vacancy.DateCreated = _dateTimeService.UtcNow();
            vacancy.DateUpdated = null;
            vacancy.DateSubmitted = null;
            vacancy.DateStartedToQA = null;
            vacancy.DateQAApproved = null;
            vacancy.ClosingDate = null;
            vacancy.PossibleStartDate = null;
            //Comments
            vacancy.TitleComment = null;
            vacancy.ShortDescriptionComment = null;
            vacancy.DesiredSkillsComment = null;
            vacancy.FutureProspectsComment = null;
            vacancy.PersonalQualitiesComment = null;
            vacancy.ThingsToConsiderComment = null;
            vacancy.DesiredQualificationsComment = null;
            vacancy.OfflineApplicationUrlComment = null;
            vacancy.OfflineApplicationInstructionsComment = null;
            vacancy.ApprenticeshipLevelComment = null;
            vacancy.FrameworkCodeNameComment = null;
            vacancy.StandardIdComment = null;
            vacancy.WageComment = null;
            vacancy.ClosingDateComment = null;
            vacancy.DurationComment = null;
            vacancy.LongDescriptionComment = null;
            vacancy.PossibleStartDateComment = null;
            vacancy.WorkingWeekComment = null;
            vacancy.FirstQuestionComment = null;
            vacancy.SecondQuestionComment = null;

            vacancy.EntityId = Guid.NewGuid();

            _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            var result = vacancy.ProviderSiteEmployerLink.Convert();
            result.VacancyGuid = vacancy.EntityId;

            return result;
        }

        private List<SelectListItem> GetPageSizes(int pageSize)
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Value = "5", Text = "5 per page", Selected = pageSize == 5},
                new SelectListItem {Value = "10", Text = "10 per page", Selected = pageSize == 10},
                new SelectListItem {Value = "25", Text = "25 per page", Selected = pageSize == 25},
                new SelectListItem {Value = "50", Text = "50 per page", Selected = pageSize == 50}
            };
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

        private DashboardVacancySummaryViewModel ConvertToDashboardVacancySummaryViewModel(ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var provider = _providerService.GetProvider(apprenticeshipVacancy.Ukprn);

            return new DashboardVacancySummaryViewModel
            {
                ClosingDate = apprenticeshipVacancy.ClosingDate,
                DateSubmitted = apprenticeshipVacancy.DateSubmitted,
                ProviderName = provider.Name,
                Status = apprenticeshipVacancy.Status,
                Title = apprenticeshipVacancy.Title,
                VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber,
                DateStartedToQA = apprenticeshipVacancy.DateStartedToQA,
                QAUserName = apprenticeshipVacancy.QAUserName,
                CanBeReservedForQaByCurrentUser = CanBeReservedForQaByCurrentUser(apprenticeshipVacancy)
            };
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

        private static bool NoUserHasStartedToQATheVacancy(ApprenticeshipVacancy apprenticeshipVacancy)
        {
            return apprenticeshipVacancy.Status == ProviderVacancyStatuses.PendingQA && (string.IsNullOrWhiteSpace(apprenticeshipVacancy.QAUserName) || !apprenticeshipVacancy.DateStartedToQA.HasValue);
        }

        private bool CurrentUserHasStartedToQATheVacancy(ApprenticeshipVacancy apprenticeshipVacancy)
        {
            return apprenticeshipVacancy.Status == ProviderVacancyStatuses.ReservedForQA && apprenticeshipVacancy.QAUserName == Thread.CurrentPrincipal.Identity.Name;
        }

        private bool AUserHasLeftTheVacancyUnattended(ApprenticeshipVacancy apprenticeshipVacancy, int timeout)
        {
            return apprenticeshipVacancy.Status == ProviderVacancyStatuses.ReservedForQA && (_dateTimeService.UtcNow() - apprenticeshipVacancy.DateStartedToQA).Value.TotalMinutes > timeout;
        }

        public List<DashboardVacancySummaryViewModel> GetPendingQAVacancies()
        {
            return GetPendingQAVacanciesOverview().Where(vm => vm.CanBeReservedForQaByCurrentUser).ToList();
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
            vacancy.QAUserName = null;

            _apprenticeshipVacancyWriteRepository.Save(vacancy);
        }

        public VacancyViewModel ReserveVacancyForQA(long vacancyReferenceNumber)
        {
            var vacancy = _apprenticeshipVacancyWriteRepository.ReserveVacancyForQA(vacancyReferenceNumber);
            //TODO: Cope with null, interprit as already reserved etc.
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyViewModel>(vacancy);
            var providerSite = _providerService.GetProviderSite(vacancy.Ukprn, vacancy.ProviderSiteEmployerLink.ProviderSiteErn);
            viewModel.ProviderSite = providerSite.Convert();
            viewModel.FrameworkName = string.IsNullOrEmpty(vacancy.FrameworkCodeName) ? vacancy.FrameworkCodeName : _referenceDataService.GetSubCategoryByCode(vacancy.FrameworkCodeName).FullName;
            var standard = GetStandard(vacancy.StandardId);
            viewModel.StandardName = standard == null ? "" : standard.Name;
            return viewModel;
        }

        public VacancySummaryViewModel UpdateVacancyWithComments(VacancySummaryViewModel viewModel)
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

        public NewVacancyViewModel UpdateVacancyWithComments(NewVacancyViewModel viewModel)
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
            vacancy.StandardIdComment = viewModel.StandardIdComment;
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

            viewModel = _mapper.Map<ApprenticeshipVacancy, NewVacancyViewModel>(vacancy);
            var sectors = GetSectorsAndFrameworks();
            var standards = GetStandards();
            viewModel.SectorsAndFrameworks = sectors;
            viewModel.Standards = standards;
            return viewModel;
        }

        public VacancyRequirementsProspectsViewModel UpdateVacancyWithComments(VacancyRequirementsProspectsViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyReferenceNumber);

            vacancy.DesiredSkills = viewModel.DesiredSkills;
            vacancy.DesiredSkillsComment = viewModel.DesiredSkillsComment;
            vacancy.FutureProspects = viewModel.FutureProspects;
            vacancy.FutureProspectsComment = viewModel.FutureProspectsComment;
            vacancy.PersonalQualities = viewModel.PersonalQualities;
            vacancy.PersonalQualitiesComment = viewModel.PersonalQualitiesComment;
            vacancy.ThingsToConsider = viewModel.ThingsToConsider;
            vacancy.ThingsToConsiderComment = viewModel.ThingsToConsiderComment;
            vacancy.DesiredQualifications = viewModel.DesiredQualifications;
            vacancy.DesiredQualificationsComment = viewModel.DesiredQualificationsComment;

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            viewModel = vacancy.ConvertToVacancyRequirementsProspectsViewModel();
            return viewModel;
        }

        public VacancyQuestionsViewModel UpdateVacancyWithComments(VacancyQuestionsViewModel viewModel)
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
    }
}
