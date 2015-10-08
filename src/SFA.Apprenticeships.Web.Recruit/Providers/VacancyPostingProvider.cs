namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Interfaces.VacancyPosting;
    using Common.Configuration;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using ViewModels.Frameworks;
    using ViewModels.Vacancy;

    public class VacancyPostingProvider : IVacancyPostingProvider
    {
        private readonly ILogService _logService;

        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IUserProfileService _userProfileService;
        private readonly IProviderService _providerService;
        private readonly IReferenceDataService _referenceDataService;

        private readonly string[] _blacklistedCategoryCodes;

        public VacancyPostingProvider(
            ILogService logService,
            IConfigurationService configurationService,
            IVacancyPostingService vacancyPostingService,
            IUserProfileService userProfileService,
            IProviderService providerService,
            IReferenceDataService referenceDataService)
        {
            _logService = logService;
            _vacancyPostingService = vacancyPostingService;

            _userProfileService = userProfileService;
            _providerService = providerService;
            _referenceDataService = referenceDataService;
            _blacklistedCategoryCodes = GetBlacklistedCategoryCodeNames(configurationService);
        }

        public NewVacancyViewModel GetNewVacancyViewModel(string username)
        {
            var userProfile = _userProfileService.GetProviderUser(username);
            var sectors = GetSectorsAndFrameworks();

            return new NewVacancyViewModel
            {
                ApprenticeshipLevel = ApprenticeshipLevel.Intermediate,
                ProviderSiteErn = userProfile.PreferredSiteErn,
                SectorsAndFrameworks = sectors
            };
        }

        public NewVacancyViewModel CreateVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            _logService.Debug("Creating vacancy reference number");

            try
            {
                var vacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();

                var vacancy = _vacancyPostingService.CreateApprenticeshipVacancy(new ApprenticeshipVacancy
                {
                    EntityId = Guid.NewGuid(),
                    VacancyReferenceNumber = vacancyReferenceNumber,
                    FrameworkCodeName = newVacancyViewModel.FrameworkCodeName,
                    ApprenticeshipLevel = newVacancyViewModel.ApprenticeshipLevel,
                    ProviderSiteErn = newVacancyViewModel.ProviderSiteErn
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
            return new VacancyViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                ApprenticeshipLevels = GetApprenticeshipLevels()
            };
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
