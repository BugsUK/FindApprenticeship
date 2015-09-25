namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.AgencyUser
{
    using Application.Interfaces.Users;
    using Manage.Providers;
    using Moq;

    public class AgencyUserProviderBuilder
    {
        private Mock<IUserProfileService> _userProfileService;

        public IAgencyUserProvider Build()
        {
            var provider = new AgencyUserProvider(_userProfileService.Object);
            return provider;
        }

        public AgencyUserProviderBuilder With(Mock<IUserProfileService> userProfileService)
        {
            _userProfileService = userProfileService;
            return this;
        }
    }
}