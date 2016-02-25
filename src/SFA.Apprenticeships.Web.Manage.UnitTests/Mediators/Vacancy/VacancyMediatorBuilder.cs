namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using Manage.Mediators.Vacancy;
    using Moq;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Provider;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.Validators.VacancyPosting;

    public class VacancyMediatorBuilder
    {
        private readonly NewVacancyViewModelServerValidator _newVacancyViewModelServerValidator = new NewVacancyViewModelServerValidator();

        private Mock<IVacancyQAProvider> _vacancyProvider = new Mock<IVacancyQAProvider>();

        private readonly VacancySummaryViewModelServerValidator _vacancySummaryViewModelServerValidator = new VacancySummaryViewModelServerValidator();
        private readonly VacancyRequirementsProspectsViewModelServerValidator _vacancyRequirementsProspectsViewModelServerValidator = new VacancyRequirementsProspectsViewModelServerValidator();

        private readonly VacancyViewModelValidator _vacancyViewModelValidator = new VacancyViewModelValidator();
        private readonly VacancyQuestionsViewModelServerValidator _vacancyQuestionsViewModelServerValidator = new VacancyQuestionsViewModelServerValidator();
        private readonly Mock<IProviderQAProvider> _providerQaProvider = new Mock<IProviderQAProvider>();
        private readonly LocationSearchViewModelValidator _locationSearchViewModelValidator = new LocationSearchViewModelValidator();
        private readonly Mock<ILocationsProvider> _locationsProvider = new Mock<ILocationsProvider>();
        private readonly VacancyPartyViewModelValidator _vacancyPartyViewModelValidator = new VacancyPartyViewModelValidator();

        private readonly TrainingDetailsViewModelServerValidator _trainingDetailsViewModelServerValidator = new TrainingDetailsViewModelServerValidator();

        public IVacancyMediator Build()
        {   
            return new VacancyMediator(_vacancyProvider.Object, _vacancyViewModelValidator,
                _vacancySummaryViewModelServerValidator,
                _newVacancyViewModelServerValidator, _vacancyQuestionsViewModelServerValidator,
                _vacancyRequirementsProspectsViewModelServerValidator, _vacancyPartyViewModelValidator,
                _providerQaProvider.Object, _locationSearchViewModelValidator, _locationsProvider.Object, _trainingDetailsViewModelServerValidator);
        }

        public VacancyMediatorBuilder With(Mock<IVacancyQAProvider> provider)
        {
            _vacancyProvider = provider;
            return this;
        }
    }
}