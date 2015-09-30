namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Common.Configuration;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;
    using ViewModels.Frameworks;
    using ViewModels.Vacancy;

    public class VacancyPostingProvider : IVacancyPostingProvider
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IProviderService _providerService;
        private readonly IReferenceDataService _referenceDataService;

        private readonly string[] _blacklistedCategoryCodes;

        public VacancyPostingProvider(
            IConfigurationService configurationService,
            IUserProfileService userProfileService,
            IProviderService providerService,
            IReferenceDataService referenceDataService)
        {
            _userProfileService = userProfileService;
            _providerService = providerService;
            _referenceDataService = referenceDataService;
            _blacklistedCategoryCodes = GetBlacklistedCategoryCodeNames(configurationService);
        }

        public NewVacancyViewModel GetNewVacancyViewModel(string username)
        {
            var userProfile = _userProfileService.GetProviderUser(username);
            var providerSites = GetProviderSiteSelectList(userProfile.Ukprn);
            var sectors = GetSectorSelectList();

            return new NewVacancyViewModel
            {
                ApprenticeshipLevel = ApprenticeshipLevel.Intermediate,
                TrainingSiteErn = userProfile.PreferredSiteErn,
                Sectors = sectors,
                ProviderSites = providerSites
            };
        }

        #region Helper

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

        private List<SectorSelectItemViewModel> GetSectorSelectList()
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

        private List<SelectListItem> GetProviderSiteSelectList(string ukprn)
        {
            var providerSites = _providerService.GetProviderSites(ukprn);

            return providerSites.Select(providerSite => new SelectListItem
            {
                Value = providerSite.Ern,
                Text = providerSite.Name
            }).ToList();
        }

        #endregion
    }
}
