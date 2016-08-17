namespace SFA.Apprenticeships.Application.UnitTests.Services.Provider
{
    using Apprenticeships.Application.Provider;
    using Domain.Raa.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Interfaces.Employers;
    using Interfaces.Providers;
    using Moq;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Apprenticeships.Application.UserAccount.Strategies.ProviderUserAccount;

    public class ProviderServiceBuilder
    {
        private IEmployerService _employerService;
        private IProviderReadRepository _providerReadRepository;
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;
        private IVacancyPartyReadRepository _vacancyPartyReadRepository;
        private readonly IVacancyPartyWriteRepository _vacancyPartyWriteRepository;
        private readonly ILogService _logService;
        private readonly ISubmitContactMessageStrategy _submitContactMessageStrategy;

        public ProviderServiceBuilder()
        {
            _employerService = new Mock<IEmployerService>().Object;
            _providerReadRepository = new Mock<IProviderReadRepository>().Object;
            _providerSiteReadRepository = new Mock<IProviderSiteReadRepository>().Object;
            _vacancyPartyReadRepository = new Mock<IVacancyPartyReadRepository>().Object;
            _vacancyPartyWriteRepository = new Mock<IVacancyPartyWriteRepository>().Object;
            _submitContactMessageStrategy=new Mock<ISubmitContactMessageStrategy>().Object;
            _logService = new Mock<ILogService>().Object;
        }

        public IProviderService Build()
        {
            var provider = new ProviderService(_providerReadRepository, _providerSiteReadRepository, 
                _vacancyPartyReadRepository, _vacancyPartyWriteRepository, _logService, 
                _employerService);
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

        public ProviderServiceBuilder With(IVacancyPartyReadRepository vacancyPartyReadRepository)
        {
            _vacancyPartyReadRepository = vacancyPartyReadRepository;
            return this;
        }
    }
}