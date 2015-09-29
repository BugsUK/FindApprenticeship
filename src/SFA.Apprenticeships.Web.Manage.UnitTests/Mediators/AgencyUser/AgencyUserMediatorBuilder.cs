﻿namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.AgencyUser
{
    using Common.Providers;
    using Common.Providers.Azure.AccessControlService;
    using Manage.Mediators.AgencyUser;
    using Manage.Providers;
    using Moq;

    public class AgencyUserMediatorBuilder
    {
        private Mock<IAgencyUserProvider> _agencyUserProvider = new Mock<IAgencyUserProvider>(); 
        private readonly Mock<IAuthorizationErrorProvider> _authorizationErrorProvider = new Mock<IAuthorizationErrorProvider>();
        private Mock<IUserDataProvider> _userDataProvider = new Mock<IUserDataProvider>(); 

        public IAgencyUserMediator Build()
        {
            return new AgencyUserMediator(
                _agencyUserProvider.Object,
                _authorizationErrorProvider.Object,
                _userDataProvider.Object);
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
    }
}