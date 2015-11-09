using SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy;

namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using Manage.Mediators.Vacancy;
    using Manage.Providers;
    using Moq;

    public class VacancyMediatorBuilder
    {
        private Mock<IVacancyProvider> _vacancyProvider = new Mock<IVacancyProvider>();
        private VacancyViewModelValidator _vacancyViewModelValidator = new VacancyViewModelValidator(); 

        public IVacancyMediator Build()
        {
            return new VacancyMediator(_vacancyProvider.Object, _vacancyViewModelValidator);
        }

        public VacancyMediatorBuilder With(Mock<IVacancyProvider> provider)
        {
            _vacancyProvider = provider;
            return this;
        }
    }
}