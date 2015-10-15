namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Moq;
    using NUnit.Framework;
    using Recruit.Mediators.VacancyPosting;
    using Recruit.Providers;
    using Recruit.Validators.Provider;
    using Recruit.Validators.Vacancy;

    public class TestsBase
    {
        protected Mock<IVacancyPostingProvider> VacancyPostingProvider;
        protected Mock<IProviderProvider> ProviderProvider;
        protected Mock<IEmployerProvider> EmployerProvider;

        [SetUp]
        public void SetUp()
        {
            VacancyPostingProvider = new Mock<IVacancyPostingProvider>();
        }

        protected IVacancyPostingMediator GetMediator()
        {
            return new VacancyPostingMediator(
                VacancyPostingProvider.Object,
                ProviderProvider.Object,
                EmployerProvider.Object,
                new NewVacancyViewModelServerValidator(),
                new VacancySummaryViewModelServerValidator(), 
                new VacancyRequirementsProspectsViewModelServerValidator(), 
                new VacancyQuestionsViewModelServerValidator(), 
                new VacancyViewModelValidator(),
                new ProviderSiteEmployerLinkViewModelValidator());
        }
    }
}
