using SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using System.Collections.Generic;
    using Common.Constants;
    using Common.UnitTests.Builders;
    using Common.UnitTests.Mediators;
    using Constants.Messages;
    using Moq;
    using NUnit.Framework;
    using Recruit.Mediators.ProviderUser;
    using Raa.Common.ViewModels.Provider;

    [TestFixture]
    public class AuthorizeTests : TestBase
    {
        [Test]
        public void Authorize_EmptyUsername()
        {
            // Arrange.
            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.EmptyUsername, AuthorizeMessages.EmptyUsername, UserMessageLevel.Error);
        }

        [Test(Description = "If the provider doesn't have a provider identifier (missing \"ukprn claim\") then end the user's session and navigate to the landing page with a message")]
        public void Authorize_MissingProviderIdentifier()
        {
            // Arrange.
            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.MissingProviderIdentifier, AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
        }

        [Test(Description = "If the provider doesn't have a provider identifier (missing \"ukprn claim\") then end the user's session and navigate to the landing page with a message")]
        public void Authorize_EmptyProviderIdentifier()
        {
            // Arrange.
            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("").WithRole(Constants.Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.MissingProviderIdentifier, AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
        }

        [Test(Description = "If the user doesn't have the service permission (missing \"service claim\") then end the user's session and return them to landing page with a message")]
        public void Authorize_MissingServicePermission()
        {
            // Arrange.
            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.MissingServicePermission, AuthorizeMessages.MissingServicePermission, UserMessageLevel.Warning);
        }

        [Test(Description = "If there is no provider profile then direct the user to the \"sites\" page with a message")]
        public void Authorize_NoProviderProfile()
        {
            // Arrange.
            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Constants.Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.NoProviderProfile, AuthorizeMessages.NoProviderProfile, UserMessageLevel.Info);
        }

        [Test(Description = "If there is not at least 1 site then direct the user to the \"sites\" page with a message")]
        public void Authorize_FailedMinimumSitesCountCheck()
        {
            // Arrange.
            MockProviderProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(new ProviderViewModel());

            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Constants.Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.FailedMinimumSitesCountCheck, AuthorizeMessages.FailedMinimumSitesCountCheck, UserMessageLevel.Warning);
        }

        [Test(Description = "If there is no user profile then direct the user to the \"user profile\" page with a message")]
        public void Authorize_NoUserProfile()
        {
            // Arrange.
            var providerViewModel = new ProviderViewModel
            {
                ProviderSiteViewModels = new List<ProviderSiteViewModel> {new ProviderSiteViewModel()}
            };

            MockProviderProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(providerViewModel);
            MockProviderUserProvider.Setup(p => p.GetUserProfileViewModels(It.IsAny<string>()))
                .Returns(new List<ProviderUserViewModel> {new ProviderUserViewModel()});

            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Constants.Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.NoUserProfile, AuthorizeMessages.NoUserProfile, UserMessageLevel.Info);
        }

        [Test(Description = "If the user has not verified their email address then direct the user to the \"verify\" page with a message")]
        public void Authorize_EmailAddressNotVerified()
        {
            // Arrange.
            var providerViewModel = new ProviderViewModel
            {
                ProviderSiteViewModels = new List<ProviderSiteViewModel>
                {
                    new ProviderSiteViewModel()
                }
            };

            MockProviderProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(providerViewModel);
            MockProviderUserProvider.Setup(p => p.GetUserProfileViewModel(It.IsAny<string>())).Returns(new ProviderUserViewModel());

            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Constants.Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.EmailAddressNotVerified, AuthorizeMessages.EmailAddressNotVerified, UserMessageLevel.Info);
        }

        [Test(Description = "User has all claims, a complete provider profile and has verified their email address")]
        public void Authorize_Ok()
        {
            // Arrange.
            var providerViewModel = new ProviderViewModel
            {
                ProviderSiteViewModels = new List<ProviderSiteViewModel> { new ProviderSiteViewModel() }
            };

            MockProviderProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(providerViewModel);
            MockProviderUserProvider.Setup(p => p.GetUserProfileViewModel(It.IsAny<string>())).Returns(new ProviderUserViewModel {EmailAddressVerified = true});

            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Constants.Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertCode(ProviderUserMediatorCodes.Authorize.Ok);
        }
    }
}