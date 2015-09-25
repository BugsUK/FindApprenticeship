namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.AgencyUser
{
    using Manage.Mediators.AgencyUser;
    using Manage.Providers;
    using Moq;

    public class AgencyUserMediatorBuilder
    {
        private Mock<IAgencyUserProvider> _agencyUserProvider = new Mock<IAgencyUserProvider>(); 

        public IAgencyUserMediator Build()
        {
            var mediator = new AgencyUserMediator(_agencyUserProvider.Object);
            return mediator;
        }

        public AgencyUserMediatorBuilder With(Mock<IAgencyUserProvider> agencyUserProvider)
        {
            _agencyUserProvider = agencyUserProvider;
            return this;
        }
    }
}