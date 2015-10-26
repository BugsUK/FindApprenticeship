namespace SFA.Apprenticeships.Application.UnitTests.Services.Provider
{
    using Application.Provider;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;
    using Interfaces.Organisations;
    using Interfaces.Providers;
    using Moq;

    public class ProviderServiceBuilder
    {
        private IOrganisationService _organisationService;
        private IProviderReadRepository _providerReadRepository;
        private readonly IProviderWriteRepository _providerWriteRepository;
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;
        private readonly IProviderSiteWriteRepository _providerSiteWriteRepository;
        private IProviderSiteEmployerLinkReadRepository _providerSiteEmployerLinkReadRepository;
        private readonly IProviderSiteEmployerLinkWriteRepository _providerSiteEmployerLinkWriteRepository;
        private readonly ILogService _logService;

        public ProviderServiceBuilder()
        {
            _organisationService = new Mock<IOrganisationService>().Object;
            _providerReadRepository = new Mock<IProviderReadRepository>().Object;
            _providerWriteRepository = new Mock<IProviderWriteRepository>().Object;
            _providerSiteReadRepository = new Mock<IProviderSiteReadRepository>().Object;
            _providerSiteWriteRepository = new Mock<IProviderSiteWriteRepository>().Object;
            _providerSiteEmployerLinkReadRepository = new Mock<IProviderSiteEmployerLinkReadRepository>().Object;
            _providerSiteEmployerLinkWriteRepository = new Mock<IProviderSiteEmployerLinkWriteRepository>().Object;
            _logService = new Mock<ILogService>().Object;
        }

        public IProviderService Build()
        {
            var provider = new ProviderService(_organisationService, _providerReadRepository, _providerWriteRepository, _providerSiteReadRepository, _providerSiteWriteRepository, _providerSiteEmployerLinkReadRepository, _providerSiteEmployerLinkWriteRepository, _logService);
            return provider;
        }

        public ProviderServiceBuilder With(IOrganisationService organisationService)
        {
            _organisationService = organisationService;
            return this;
        }

        public ProviderServiceBuilder With(IProviderReadRepository providerReadRepository)
        {
            _providerReadRepository = providerReadRepository;
            return this;
        }

        public ProviderServiceBuilder With(IProviderSiteEmployerLinkReadRepository providerSiteEmployerLinkReadRepository)
        {
            _providerSiteEmployerLinkReadRepository = providerSiteEmployerLinkReadRepository;
            return this;
        }
    }
}