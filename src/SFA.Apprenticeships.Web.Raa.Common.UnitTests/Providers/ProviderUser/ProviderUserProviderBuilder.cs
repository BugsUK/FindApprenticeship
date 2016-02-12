namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.ProviderUser
{
    using Application.Interfaces.Providers;
    using Application.Interfaces.Users;
    using Moq;
    using Common.Providers;

    public class ProviderUserProviderBuilder
    {
        private Mock<IUserProfileService> _mockUserProfileService;
        private Mock<IProviderService> _mockProviderService;
        private Mock<IProviderUserAccountService> _mockProviderUserAccountService;

        public ProviderUserProviderBuilder()
        {
            _mockUserProfileService = new Mock<IUserProfileService>();
            _mockProviderService = new Mock<IProviderService>();
            _mockProviderUserAccountService = new Mock<IProviderUserAccountService>();
        }

        public ProviderUserProviderBuilder With(Mock<IUserProfileService> mockUserProfileService)
        {
            _mockUserProfileService = mockUserProfileService;
            return this;
        }

        public ProviderUserProviderBuilder With(Mock<IProviderService> mockProviderService)
        {
            _mockProviderService = mockProviderService;
            return this;
        }

        public ProviderUserProviderBuilder With(Mock<IProviderUserAccountService> mockProviderUserAccountService)
        {
            _mockProviderUserAccountService = mockProviderUserAccountService;
            return this;
        }

        public IProviderUserProvider Build()
        {
            var provider = new ProviderUserProvider(
                _mockUserProfileService.Object, _mockProviderService.Object, _mockProviderUserAccountService.Object);

            return provider;
        }
    }
}
