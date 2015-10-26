namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Moq;
    using NUnit.Framework;
    using ViewModels.Provider;
    using ViewModels.Vacancy;

    [TestFixture]
    public class CreateVacancyTests : TestBase
    {
        [Test]
        public void ShouldStoreOfflineApplicationFields()
        {
            MockVacancyPostingService.Setup(s => s.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy());

            var provider = GetProvider();

            const bool offlineVacancy = true;
            const string offlineApplicationUrl = "A url";
            const string offlineApplicationInstructions = "Some instructions";

            provider.CreateVacancy(new NewVacancyViewModel
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel
                {
                  Employer  = new EmployerViewModel()
                },
                OfflineVacancy = offlineVacancy,
                OfflineApplicationUrl = offlineApplicationUrl,
                OfflineApplicationInstructions = offlineApplicationInstructions,
                ApprenticeshipLevel = ApprenticeshipLevel.Higher
            });

            MockVacancyPostingService.Verify(s => s.SaveApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(v => v.OfflineVacancy == offlineVacancy 
            && v.OfflineApplicationUrl == offlineApplicationUrl && v.OfflineApplicationInstructions == offlineApplicationInstructions)));
        }
    }
}