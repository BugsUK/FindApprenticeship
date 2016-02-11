namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.ProviderUser
{
    using Application.Interfaces.Providers;
    using Application.Interfaces.Users;
    using Moq;
    using Common.Providers;

    public class ProviderUserProviderBuilder
    {
        private Mock<IUserProfileService> _userProfileService;
        private readonly Mock<IProviderService> _providerService;
        private readonly Mock<IProviderUserAccountService> _providerUserAccountService;

        public ProviderUserProviderBuilder()
        {
            _userProfileService = new Mock<IUserProfileService>();
            _providerService = new Mock<IProviderService>();
            _providerUserAccountService = new Mock<IProviderUserAccountService>();
        }

        public IProviderUserProvider Build()
        {
            var provider = new ProviderUserProvider(
                _userProfileService.Object, _providerService.Object, _providerUserAccountService.Object);
            return provider;
        }

        public ProviderUserProviderBuilder With(Mock<IUserProfileService> userProfileService)
        {
            _userProfileService = userProfileService;
            return this;
        }
    }
}
