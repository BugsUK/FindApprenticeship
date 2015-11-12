using SFA.Apprenticeships.Web.Raa.Common.Providers;
using SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy;

namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using Manage.Mediators.Vacancy;
    using Manage.Providers;
    using Moq;

    public class VacancyMediatorBuilder
    {
        private Mock<IVacancyProvider> _vacancyProvider = new Mock<IVacancyProvider>();
        private Mock<IVacancyPostingProvider> _vacancyPostingProvider = new Mock<IVacancyPostingProvider>();
        private VacancyViewModelValidator _vacancyViewModelValidator = new VacancyViewModelValidator();
        private VacancySummaryViewModelServerValidator _vacancySummaryViewModelServerValidator = new VacancySummaryViewModelServerValidator();
        private NewVacancyViewModelServerValidator _newVacancyViewModelServerValidator = new NewVacancyViewModelServerValidator();

        public IVacancyMediator Build()
        {
            return new VacancyMediator(_vacancyProvider.Object, _vacancyPostingProvider.Object, _vacancyViewModelValidator, _vacancySummaryViewModelServerValidator, _newVacancyViewModelServerValidator);
        }

        public VacancyMediatorBuilder With(Mock<IVacancyProvider> provider)
        {
            _vacancyProvider = provider;
            return this;
        }
    }
}