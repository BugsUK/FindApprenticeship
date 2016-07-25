namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using System.Collections.Generic;
    using Common.Constants;
    using Common.UnitTests.Builders;
    using Common.UnitTests.Mediators;
    using Constants.Messages;
    using Domain.Entities.Raa;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.ProviderUser;
    using Recruit.Mediators.ProviderUser;

    [TestFixture]
    public class AuthorizeTests : TestBase
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [Description(
            "If the provider doesn't have a provider identifier (missing or empty \"ukprn claim\") then end the user's session and navigate to the landing page with a message"
            )]
        public void Authorize_MissingProviderIdentifier(string ukprn)
        {
            // Arrange.
            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder()
                .WithName("User001")
                .WithUkprn(ukprn)
                .Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.MissingProviderIdentifier,
                AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
        }

        [Test(
            Description =
                "If the user has not verified their email address then direct the user to the \"verify\" page with a message"
            )]
        public void Authorize_EmailAddressNotVerified()
        {
            // Arrange.
            var providerViewModel = new ProviderViewModelBuilder().With(new List<ProviderSiteViewModel>
            {
                new ProviderSiteViewModel()
            }).Build();

            MockProviderProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(providerViewModel);
            MockProviderUserProvider.Setup(p => p.GetUserProfileViewModel(It.IsAny<string>()))
                .Returns(new ProviderUserViewModel());

            var mediator = GetMediator();
            var principal =
                new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.EmailAddressNotVerified,
                AuthorizeMessages.EmailAddressNotVerified, UserMessageLevel.Info);
        }

        [Test]
        public void Authorize_EmptyUsername()
        {
            // Arrange.
            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.EmptyUsername, AuthorizeMessages.EmptyUsername,
                UserMessageLevel.Error);
        }

        [Test(Description = "If there is not at least 1 site then direct the user to the \"sites\" page with a message")
        ]
        public void Authorize_FailedMinimumSitesCountCheck()
        {
            // Arrange.
            MockProviderProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(new ProviderViewModelBuilder().Build());

            var mediator = GetMediator();
            var principal =
                new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.FailedMinimumSitesCountCheck,
                AuthorizeMessages.FailedMinimumSitesCountCheck, UserMessageLevel.Warning);
        }

        [Test(
            Description =
                "If the user doesn't have the service permission (missing \"service claim\") then end the user's session and return them to landing page with a message"
            )]
        public void Authorize_MissingServicePermission()
        {
            // Arrange.
            var mediator = GetMediator();
            var principal = new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.MissingServicePermission,
                AuthorizeMessages.MissingServicePermission, UserMessageLevel.Warning);
        }

        [Test(Description = "If there is no provider profile then direct the user to the \"sites\" page with a message")
        ]
        public void Authorize_NoProviderProfile()
        {
            // Arrange.
            var mediator = GetMediator();
            var principal =
                new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.NoProviderProfile,
                AuthorizeMessages.NoProviderProfile, UserMessageLevel.Info);
        }

        [Test(
            Description = "If there is no user profile then direct the user to the \"user profile\" page with a message"
            )]
        public void Authorize_NoUserProfile()
        {
            // Arrange.
            var providerViewModel = new ProviderViewModelBuilder().With(new List<ProviderSiteViewModel>
            {
                new ProviderSiteViewModel()
            }).Build();
            
            MockProviderProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(providerViewModel);
            MockProviderUserProvider.Setup(p => p.GetUserProfileViewModels(It.IsAny<string>()))
                .Returns(new List<ProviderUserViewModel> {new ProviderUserViewModel()});

            var mediator = GetMediator();
            var principal =
                new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertMessage(ProviderUserMediatorCodes.Authorize.NoUserProfile, AuthorizeMessages.NoUserProfile,
                UserMessageLevel.Info);
        }

        [Test(Description = "User has all claims, a complete provider profile and has verified their email address")]
        public void Authorize_Ok()
        {
            // Arrange.
            var providerViewModel = new ProviderViewModelBuilder().With(new List<ProviderSiteViewModel>
            {
                new ProviderSiteViewModel()
            }).Build();

            MockProviderProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(providerViewModel);
            MockProviderUserProvider.Setup(p => p.GetUserProfileViewModel(It.IsAny<string>()))
                .Returns(new ProviderUserViewModel {EmailAddressVerified = true});

            var mediator = GetMediator();
            var principal =
                new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertCode(ProviderUserMediatorCodes.Authorize.Ok);
        }

        [Test(Description = "User has all claims, a complete provider profile and has verified their email address")]
        public void Authorize_Ok_NonMigrated()
        {
            // Arrange.
            var providerViewModel = new ProviderViewModelBuilder().With(new List<ProviderSiteViewModel>
            {
                new ProviderSiteViewModel()
            }).Build();

            MockProviderProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(providerViewModel);
            MockProviderUserProvider.Setup(p => p.GetUserProfileViewModel(It.IsAny<string>()))
                .Returns(new ProviderUserViewModel { EmailAddressVerified = true });

            var mediator = GetMediator();
            var principal =
                new ClaimsPrincipalBuilder().WithName("User001").WithUkprn("00001").WithRole(Roles.Faa).Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertCode(ProviderUserMediatorCodes.Authorize.Ok);
        }

        [Test(
            Description =
                "User has all claims, a complete provider profile and has verified their email address, should set ProviderId"
            )]
        public void Authorize_Ok_ShouldSetProviderId()
        {
            // Arrange.
            var providerViewModel = new ProviderViewModelBuilder()
                .With(42)
                .With(new List<ProviderSiteViewModel>
                {
                    new ProviderSiteViewModel()
                }).Build();

            MockProviderProvider
                .Setup(mock => mock.GetProviderViewModel(It.IsAny<string>()))
                .Returns(providerViewModel);

            MockProviderUserProvider
                .Setup(mock => mock.GetUserProfileViewModel(It.IsAny<string>()))
                .Returns(new ProviderUserViewModel
                {
                    EmailAddressVerified = true
                });

            var mediator = GetMediator();

            var principal = new ClaimsPrincipalBuilder()
                .WithName("User001")
                .WithUkprn("00001")
                .WithRole(Roles.Faa)
                .Build();

            // Act.
            var response = mediator.Authorize(principal);

            // Assert.
            response.AssertCode(ProviderUserMediatorCodes.Authorize.Ok);
            response.ViewModel.ProviderId.Should().Be(providerViewModel.ProviderId);
        }
    }
}