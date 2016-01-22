﻿namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.ProviderUser
{
    using Application.Interfaces.Users;
    using Moq;
    using Common.Providers;

    public class ProviderUserProviderBuilder
    {
        private Mock<IUserProfileService> _userProfileService = new Mock<IUserProfileService>();
        private readonly Mock<IProviderUserAccountService> _providerUserAccountService = new Mock<IProviderUserAccountService>();

        public IProviderUserProvider Build()
        {
            var provider = new ProviderUserProvider(_userProfileService.Object, _providerUserAccountService.Object);
            return provider;
        }

        public ProviderUserProviderBuilder With(Mock<IUserProfileService> userProfileService)
        {
            _userProfileService = userProfileService;
            return this;
        }
    }
}