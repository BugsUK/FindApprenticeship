namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Locations
{
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Employer;
    using Raa.Common.Validators.Provider;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.Validators.VacancyPosting;
    using Recruit.Mediators.VacancyPosting;

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
                new VacancyOwnerRelationshipViewModelValidator(),
                new EmployerSearchViewModelServerValidator(),
                new LocationSearchViewModelServerValidator(),
                new Mock<ILocationsProvider>().Object,
                new TrainingDetailsViewModelServerValidator(),
                new TrainingDetailsViewModelClientValidator());
        }
    }
}
