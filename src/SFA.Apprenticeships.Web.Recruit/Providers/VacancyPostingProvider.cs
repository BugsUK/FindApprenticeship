namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Linq;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Domain.Entities.Vacancies.Apprenticeships;
    using ViewModels.Vacancy;

    public class VacancyPostingProvider : IVacancyPostingProvider
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IReferenceDataService _referenceDataService;

        public VacancyPostingProvider(
            IUserProfileService userProfileService,
            IReferenceDataService referenceDataService)
        {
            _userProfileService = userProfileService;
            _referenceDataService = referenceDataService;
        }

        public NewVacancyViewModel GetNewVacancy(string username)
        {
            var userProfile = _userProfileService.GetProviderUser(username);
            var categories = _referenceDataService.GetCategories();

            return new NewVacancyViewModel
            {
                ApprenticeshipLevel = ApprenticeshipLevel.Intermediate,
                SiteUrn = userProfile.PreferredSiteErn,
                Categories = categories.ToArray()
            };
        }
    }
}
