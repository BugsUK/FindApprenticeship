namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.VacancyProvider
{
    using Application.Interfaces.Providers;
    using Domain.Interfaces.Repositories;
    using Manage.Providers.SFA.Apprenticeships.Web.Recruit.Providers;
    using Moq;

    public class VacancyProviderBuilder
    {
        private Mock<IApprenticeshipVacancyReadRepository> _apprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();
        private Mock<IApprenticeshipVacancyWriteRepository> _apprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();
        private Mock<IProviderService> _providerService = new Mock<IProviderService>(); 

        public VacancyProvider Build()
        {
            return new VacancyProvider(_apprenticeshipVacancyReadRepository.Object, _apprenticeshipVacancyWriteRepository.Object, _providerService.Object);
        }

        public VacancyProviderBuilder With(
            Mock<IApprenticeshipVacancyWriteRepository> apprenticeshipVacancyWriteRepository)
        {
            _apprenticeshipVacancyWriteRepository = apprenticeshipVacancyWriteRepository;
            return this;
        }

        public VacancyProviderBuilder With(
            Mock<IApprenticeshipVacancyReadRepository> apprenticeshipVacancyReadRepository)
        {
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
            return this;
        }

        public VacancyProviderBuilder With(
            Mock<IProviderService> providerService)
        {
            _providerService = providerService;
            return this;
        }
    }
}