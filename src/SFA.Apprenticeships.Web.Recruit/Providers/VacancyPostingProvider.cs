namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.DateTime;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.VacancyPosting;
    using Common.Configuration;
    using Converters;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using ViewModels.Vacancy;

    public class VacancyPostingProvider : IVacancyPostingProvider
    {
        private readonly ILogService _logService;

        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IReferenceDataService _referenceDataService;
        private readonly IProviderService _providerService;
        private readonly IDateTimeService _dateTimeService;

        private readonly string[] _blacklistedCategoryCodes;

        public VacancyPostingProvider(
            ILogService logService,
            IConfigurationService configurationService,
            IVacancyPostingService vacancyPostingService,
            IReferenceDataService referenceDataService,
            IProviderService providerService, IDateTimeService dateTimeService)
        {
            _logService = logService;
            _vacancyPostingService = vacancyPostingService;

            _referenceDataService = referenceDataService;
            _providerService = providerService;
            _dateTimeService = dateTimeService;
            _blacklistedCategoryCodes = GetBlacklistedCategoryCodeNames(configurationService);
        }

        public NewVacancyViewModel GetNewVacancyViewModel(string ukprn, string providerSiteErn, string ern, Guid vacancyGuid)
        {
            var existingVacancy = _vacancyPostingService.GetVacancy(vacancyGuid);
            var sectors = GetSectorsAndFrameworks();

            if (existingVacancy != null)
            {
                var vacancyViewModel = existingVacancy.ConvertToNewVacancyViewModel();
                vacancyViewModel.SectorsAndFrameworks = sectors;
                return vacancyViewModel;
            }

            var providerSiteEmployerLink = _providerService.GetProviderSiteEmployerLink(providerSiteErn, ern);
            
            return new NewVacancyViewModel
            {
                Ukprn = ukprn,
                ApprenticeshipLevel = ApprenticeshipLevel.Unknown, //Force a selection
                SectorsAndFrameworks = sectors,
                ProviderSiteEmployerLink = providerSiteEmployerLink.Convert()
            };
        }

        public NewVacancyViewModel GetNewVacancyViewModel(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);
            var viewModel = vacancy.ConvertToNewVacancyViewModel();
            var sectors = GetSectorsAndFrameworks();
            viewModel.SectorsAndFrameworks = sectors;
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

                newVacancyViewModel.VacancyReferenceNumber = vacancy.VacancyReferenceNumber;

                return newVacancyViewModel;
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
                FrameworkCodeName = newVacancyViewModel.FrameworkCodeName,
                ApprenticeshipLevel = newVacancyViewModel.ApprenticeshipLevel,
                ProviderSiteEmployerLink = providerSiteEmployerLink,
                Status = ProviderVacancyStatuses.Draft,
                OfflineVacancy = newVacancyViewModel.OfflineVacancy,
                OfflineApplicationUrl = offlineApplicationUrl,
                OfflineApplicationInstructions = newVacancyViewModel.OfflineApplicationInstructions
            });
            return vacancy;
        }

        private NewVacancyViewModel UpdateExistingVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            var offlineApplicationUrl = !string.IsNullOrEmpty(newVacancyViewModel.OfflineApplicationUrl) ? new UriBuilder(newVacancyViewModel.OfflineApplicationUrl).Uri.ToString() : newVacancyViewModel.OfflineApplicationUrl;

            var vacancy = _vacancyPostingService.GetVacancy(newVacancyViewModel.VacancyReferenceNumber.Value);

            vacancy.Ukprn = newVacancyViewModel.Ukprn;
            vacancy.Title = newVacancyViewModel.Title;
            vacancy.ShortDescription = newVacancyViewModel.ShortDescription;
            vacancy.FrameworkCodeName = newVacancyViewModel.FrameworkCodeName;
            vacancy.ApprenticeshipLevel = newVacancyViewModel.ApprenticeshipLevel;
            vacancy.OfflineVacancy = newVacancyViewModel.OfflineVacancy;
            vacancy.OfflineApplicationUrl = offlineApplicationUrl;
            vacancy.OfflineApplicationInstructions = newVacancyViewModel.OfflineApplicationInstructions;

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            newVacancyViewModel = vacancy.ConvertToNewVacancyViewModel();

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
            var viewModel = vacancy.ConvertToVacancyViewModel();
            var providerSite = _providerService.GetProviderSite(vacancy.Ukprn, vacancy.ProviderSiteEmployerLink.ProviderSiteErn);
            viewModel.ProviderSite = providerSite.Convert();
            viewModel.ApprenticeshipLevels = GetApprenticeshipLevels();
            viewModel.FrameworkName = string.IsNullOrEmpty(vacancy.FrameworkCodeName) ? vacancy.FrameworkCodeName : _referenceDataService.GetSubCategoryByCode(vacancy.FrameworkCodeName).FullName;
            return viewModel;
        }

        public VacancyViewModel SubmitVacancy(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);

            vacancy.Status = ProviderVacancyStatuses.PendingQA;
            vacancy.DateSubmitted = _dateTimeService.UtcNow();

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            //TODO: should we return this VM or the one returned by GetVacancy?
            var viewModel = vacancy.ConvertToVacancyViewModel();

            return viewModel;
        }

        public List<SelectListItem> GetApprenticeshipLevels()
        {
            var levels =
                Enum.GetValues(typeof(ApprenticeshipLevel))
                    .Cast<ApprenticeshipLevel>()
                    .Where(al => al != ApprenticeshipLevel.Unknown)
                    .Select(al => new SelectListItem { Value = al.ToString(), Text = al.ToString() })
                    .ToList();

            return levels;
        }

        public List<SelectListItem> GetSectorsAndFrameworks()
        {
            var categories = _referenceDataService.GetCategories();

            var sectorsAndFrameworkItems = new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "Choose from the list of frameworks"}
            };

            foreach (var sector in categories.Where(category => !_blacklistedCategoryCodes.Contains(category.CodeName)))
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
    }
}
