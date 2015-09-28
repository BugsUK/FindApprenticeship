namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.AgencyUser
{
    using Common.Providers.Azure.AccessControlService;
    using Manage.Mediators.AgencyUser;
    using Manage.Providers;
    using Moq;

    public class AgencyUserMediatorBuilder
    {
        private Mock<IAgencyUserProvider> _agencyUserProvider = new Mock<IAgencyUserProvider>(); 
        private readonly Mock<IAuthorizationErrorProvider> _authorizationErrorProvider = new Mock<IAuthorizationErrorProvider>();

        public IAgencyUserMediator Build()
        {
            return new AgencyUserMediator(
                _agencyUserProvider.Object,
                _authorizationErrorProvider.Object);
        }

        public AgencyUserMediatorBuilder With(Mock<IAgencyUserProvider> agencyUserProvider)
        {
            _agencyUserProvider = agencyUserProvider;
            return this;
        }
    }
}