namespace SFA.Apprenticeships.Application.UnitTests.Services.Provider
{
    using Application.Provider;
    using Domain.Raa.Interfaces.Repositories;
    using Infrastructure.Interfaces;
    using Interfaces.Employers;
    using Interfaces.Organisations;
    using Interfaces.Providers;
    using Moq;

    public class ProviderServiceBuilder
    {
        private IOrganisationService _organisationService;
        private IEmployerService _employerService;
        private IProviderReadRepository _providerReadRepository;
        private readonly IProviderWriteRepository _providerWriteRepository;
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;
        private readonly IProviderSiteWriteRepository _providerSiteWriteRepository;
        private IVacancyPartyReadRepository _vacancyPartyReadRepository;
        private readonly IVacancyPartyWriteRepository _vacancyPartyWriteRepository;
        private readonly ILogService _logService;

        public ProviderServiceBuilder()
        {
            _organisationService = new Mock<IOrganisationService>().Object;
            _employerService = new Mock<IEmployerService>().Object;
            _providerReadRepository = new Mock<IProviderReadRepository>().Object;
            _providerWriteRepository = new Mock<IProviderWriteRepository>().Object;
            _providerSiteReadRepository = new Mock<IProviderSiteReadRepository>().Object;
            _providerSiteWriteRepository = new Mock<IProviderSiteWriteRepository>().Object;
            _vacancyPartyReadRepository = new Mock<IVacancyPartyReadRepository>().Object;
            _vacancyPartyWriteRepository = new Mock<IVacancyPartyWriteRepository>().Object;
            _logService = new Mock<ILogService>().Object;
        }

        public IProviderService Build()
        {
            var provider = new ProviderService(_organisationService, _providerReadRepository, _providerWriteRepository, _providerSiteReadRepository, _providerSiteWriteRepository, _vacancyPartyReadRepository, _vacancyPartyWriteRepository, _logService, _employerService);
            return provider;
        }

        public ProviderServiceBuilder With(IOrganisationService organisationService)
        {
            _organisationService = organisationService;
            return this;
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

        public ProviderServiceBuilder With(IVacancyPartyReadRepository vacancyPartyReadRepository)
        {
            _vacancyPartyReadRepository = vacancyPartyReadRepository;
            return this;
        }
    }
}