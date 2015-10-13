namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.VacancyPosting;
    using Common.Configuration;
    using Converters;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using ViewModels.Frameworks;
    using ViewModels.Vacancy;

    public class VacancyPostingProvider : IVacancyPostingProvider
    {
        private readonly ILogService _logService;

        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IReferenceDataService _referenceDataService;
        private readonly IProviderService _providerService;

        private readonly string[] _blacklistedCategoryCodes;

        public VacancyPostingProvider(
            ILogService logService,
            IConfigurationService configurationService,
            IVacancyPostingService vacancyPostingService,
            IReferenceDataService referenceDataService,
            IProviderService providerService)
        {
            _logService = logService;
            _vacancyPostingService = vacancyPostingService;

            _referenceDataService = referenceDataService;
            _providerService = providerService;
            _blacklistedCategoryCodes = GetBlacklistedCategoryCodeNames(configurationService);
        }

        public NewVacancyViewModel GetNewVacancyViewModel(string ukprn, string providerSiteErn, string ern)
        {
            var providerSiteEmployerLink = _providerService.GetProviderSiteEmployerLink(providerSiteErn, ern);
            var sectors = GetSectorsAndFrameworks();

            return new NewVacancyViewModel
            {
                Ukprn = ukprn,
                ApprenticeshipLevel = ApprenticeshipLevel.Unknown, //Force a selection
                SectorsAndFrameworks = sectors,
                ProviderSiteEmployerLink = providerSiteEmployerLink.Convert()
            };
        }

        public NewVacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            _logService.Debug("Creating vacancy reference number");

            try
            {
                var vacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
                var providerSiteEmployerLink = _providerService.GetProviderSiteEmployerLink(newVacancyViewModel.ProviderSiteEmployerLink.ProviderSiteErn, newVacancyViewModel.ProviderSiteEmployerLink.Employer.Ern);

                var vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(new ApprenticeshipVacancy
                {
                    EntityId = Guid.NewGuid(),
                    VacancyReferenceNumber = vacancyReferenceNumber,
                    Ukprn = newVacancyViewModel.Ukprn,
                    FrameworkCodeName = newVacancyViewModel.FrameworkCodeName,
                    ApprenticeshipLevel = newVacancyViewModel.ApprenticeshipLevel,
                    ProviderSiteEmployerLink = providerSiteEmployerLink
                });

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

        public VacancyViewModel GetVacancy(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);
            var viewModel = vacancy.Convert();
            var providerSite = _providerService.GetProviderSite(vacancy.Ukprn, vacancy.ProviderSiteEmployerLink.ProviderSiteErn);
            viewModel.ProviderSite = providerSite.Convert();
            viewModel.ApprenticeshipLevels = GetApprenticeshipLevels();
            viewModel.FrameworkName = _referenceDataService.GetSubCategoryByCode(vacancy.FrameworkCodeName).FullName;
            return viewModel;
        }

        public VacancyViewModel SubmitVacancy(VacancyViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyReferenceNumber);

            vacancy.Title = viewModel.Title;
            vacancy.ShortDescription = viewModel.ShortDescription;
            vacancy.WorkingWeek = viewModel.WorkingWeek;
            vacancy.WeeklyWage = viewModel.WeeklyWage;
            vacancy.Duration = viewModel.Duration;
            vacancy.ClosingDate = viewModel.ClosingDate;
            vacancy.PossibleStartDate = viewModel.PossibleStartDate;
            vacancy.ApprenticeshipLevel = viewModel.ApprenticeshipLevel;
            vacancy.LongDescription = viewModel.LongDescription;
            vacancy.DesiredSkills = viewModel.DesiredSkills;
            vacancy.FutureProspects = viewModel.FutureProspects;
            vacancy.PersonalQualities = viewModel.PersonalQualities;
            vacancy.ThingsToConsider = viewModel.ThingsToConsider;
            vacancy.DesiredQualifications = viewModel.DesiredQualifications;
            vacancy.FirstQuestion = viewModel.FirstQuestion;
            vacancy.SecondQuestion = viewModel.SecondQuestion;

            vacancy = _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);

            viewModel = vacancy.Convert();
            viewModel.ApprenticeshipLevels = GetApprenticeshipLevels();
            viewModel.FrameworkName = _referenceDataService.GetSubCategoryByCode(vacancy.FrameworkCodeName).FullName;
            var providerSite = _providerService.GetProviderSite(vacancy.Ukprn, vacancy.ProviderSiteEmployerLink.ProviderSiteErn);
            viewModel.ProviderSite = providerSite.Convert();
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

        public List<SectorSelectItemViewModel> GetSectorsAndFrameworks()
        {
            var categories = _referenceDataService.GetCategories();

            return categories
                .Where(category => !_blacklistedCategoryCodes.Contains(category.CodeName))
                .Where(category => category.SubCategories?.Count > 0)
                .Select(category => new SectorSelectItemViewModel
                {
                    CodeName = category.CodeName,
                    FullName = category.FullName,
                    Frameworks = category.SubCategories.Select(subCategory => new FrameworkSelectItemViewModel
                    {
                        CodeName = subCategory.CodeName,
                        FullName = subCategory.FullName
                    }).ToList()
                }).ToList();
        }

        #region Helpers

        private static string[] GetBlacklistedCategoryCodeNames(IConfigurationService configurationService)
        {
            var blacklistedCategoryCodeNames = configurationService.Get<WebConfiguration>().BlacklistedCategoryCodes;
            
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
