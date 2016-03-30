using SFA.Apprenticeships.Web.Raa.Common.Providers;

namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.AgencyUser
{
    using Common.Providers;
    using Common.Providers.Azure.AccessControlService;
    using Manage.Mediators.AgencyUser;
    using Manage.Providers;
    using Moq;
    using SFA.Infrastructure.Interfaces;

    public class AgencyUserMediatorBuilder
    {
        private Mock<IAgencyUserProvider> _agencyUserProvider = new Mock<IAgencyUserProvider>(); 
        private readonly Mock<IAuthorizationErrorProvider> _authorizationErrorProvider = new Mock<IAuthorizationErrorProvider>();
        private Mock<IUserDataProvider> _userDataProvider = new Mock<IUserDataProvider>();
        private Mock<IVacancyQAProvider> _vacancyProvider = new Mock<IVacancyQAProvider>();
        private Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();

        public IAgencyUserMediator Build()
        {
            return new AgencyUserMediator(
                _agencyUserProvider.Object,
                _authorizationErrorProvider.Object,
                _userDataProvider.Object,
                _vacancyProvider.Object,
                _configurationService.Object);
        }

        public AgencyUserMediatorBuilder With(Mock<IAgencyUserProvider> agencyUserProvider)
        {
            _agencyUserProvider = agencyUserProvider;
            return this;
        }

        public AgencyUserMediatorBuilder With(Mock<IUserDataProvider> userDataProvider)
        {
            _userDataProvider = userDataProvider;
            return this;
        }

        internal AgencyUserMediatorBuilder With(Mock<IVacancyQAProvider> vacancyProvider)
        {
            _vacancyProvider = vacancyProvider;
            return this;
        }

        public AgencyUserMediatorBuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationService = configurationService;
            return this;
        }
    }
}