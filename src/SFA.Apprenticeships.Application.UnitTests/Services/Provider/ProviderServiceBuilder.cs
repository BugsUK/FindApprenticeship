namespace SFA.Apprenticeships.Application.UnitTests.Services.Provider
{
    using Apprenticeships.Application.Provider;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces;
    using Interfaces.Employers;
    using Interfaces.Providers;
    using Moq;

    public class ProviderServiceBuilder
    {
        private IEmployerService _employerService;
        private IProviderReadRepository _providerReadRepository;
        private readonly IProviderWriteRepository _providerWriteRepository;
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;
        private readonly IProviderSiteWriteRepository _providerSiteWriteRepository;
        private IVacancyOwnerRelationshipReadRepository _vacancyOwnerRelationshipReadRepository;
        private readonly IVacancyOwnerRelationshipWriteRepository _vacancyOwnerRelationshipWriteRepository;
        private readonly ILogService _logService;

        public ProviderServiceBuilder()
        {
            _employerService = new Mock<IEmployerService>().Object;
            _providerReadRepository = new Mock<IProviderReadRepository>().Object;
            _providerWriteRepository = new Mock<IProviderWriteRepository>().Object;
            _providerSiteReadRepository = new Mock<IProviderSiteReadRepository>().Object;
            _providerSiteWriteRepository = new Mock<IProviderSiteWriteRepository>().Object;
            _vacancyOwnerRelationshipReadRepository = new Mock<IVacancyOwnerRelationshipReadRepository>().Object;
            _vacancyOwnerRelationshipWriteRepository = new Mock<IVacancyOwnerRelationshipWriteRepository>().Object;
            _logService = new Mock<ILogService>().Object;
        }

        public IProviderService Build()
        {
            var provider = new ProviderService(_providerReadRepository, _providerSiteReadRepository,
                _vacancyOwnerRelationshipReadRepository, _vacancyOwnerRelationshipWriteRepository, _logService,
                _employerService, _providerWriteRepository, _providerSiteWriteRepository);
            return provider;
        }

        public ProviderServiceBuilder With(IEmployerService employerService)
        {
            _employerService = employerService;
            return this;
        }

        public ProviderServiceBuilder With(IProviderReadRepository providerReadRepository)
        {
            _providerReadRepository = providerReadRepository;
            return this;
        }

        public ProviderServiceBuilder With(IVacancyOwnerRelationshipReadRepository vacancyOwnerRelationshipReadRepository)
        {
            _vacancyOwnerRelationshipReadRepository = vacancyOwnerRelationshipReadRepository;
            return this;
        }
    }
}