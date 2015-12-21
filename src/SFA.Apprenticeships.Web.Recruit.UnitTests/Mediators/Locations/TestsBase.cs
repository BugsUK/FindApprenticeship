namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Locations
{
    using Application.Location;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Provider;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.Validators.VacancyPosting;
    using Recruit.Mediators.VacancyPosting;
    using Recruit.Validators.VacancyPosting;

    public class TestsBase
    {
        protected Mock<IVacancyPostingProvider> VacancyPostingProvider;
        protected Mock<IProviderProvider> ProviderProvider;
        protected Mock<IEmployerProvider> EmployerProvider;

        [SetUp]
        public void SetUp()
        {
            VacancyPostingProvider = new Mock<IVacancyPostingProvider>();
            ProviderProvider = new Mock<IProviderProvider>();
            EmployerProvider = new Mock<IEmployerProvider>();
        }

        protected IVacancyPostingMediator GetMediator()
        {
            return new VacancyPostingMediator(
                VacancyPostingProvider.Object,
                ProviderProvider.Object,
                EmployerProvider.Object,
                new NewVacancyViewModelServerValidator(),
                new NewVacancyViewModelClientValidator(),
                new VacancySummaryViewModelServerValidator(),
                new VacancySummaryViewModelClientValidator(),
                new VacancyRequirementsProspectsViewModelServerValidator(),
                new VacancyRequirementsProspectsViewModelClientValidator(),
                new VacancyQuestionsViewModelServerValidator(),
                new VacancyQuestionsViewModelClientValidator(),
                new VacancyViewModelValidator(), 
                new ProviderSiteEmployerLinkViewModelValidator(),
                new EmployerSearchViewModelServerValidator(),
                new LocationSearchViewModelValidator(),
                new Mock<IAddressLookupProvider>().Object,
                new Mock<ILocationsProvider>().Object);
        }
    }
}
