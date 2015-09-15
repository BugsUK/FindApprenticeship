using System.Collections.Generic;
using System.Security.Claims;
using Moq;
using NUnit.Framework;
using SFA.Apprenticeships.Web.Common.Constants;
using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;
using SFA.Apprenticeships.Web.Recruit.Constants.Messages;
using SFA.Apprenticeships.Web.Recruit.Mediators.Home;
using SFA.Apprenticeships.Web.Recruit.Providers;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Home
{
    [TestFixture]
    public class HomeMediatorTests
    {
        [Test]
        public void Authenticated_EmptyUsername()
        {
            var mediator = new HomeMediatorBuilder().Build();

            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertMessage(HomeMediatorCodes.Authorize.EmptyUsername, AuthorizeMessages.EmptyUsername, UserMessageLevel.Error);
        }

        [Test(Description = "If the provider doesn't have a provider identifier (missing \"ukprn claim\") then end the user's session and navigate to the landing page with a message")]
        public void Authenticated_MissingProviderIdentifier()
        {
            var mediator = new HomeMediatorBuilder().Build();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "User001"));
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertMessage(HomeMediatorCodes.Authorize.MissingProviderIdentifier, AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
        }

        [Test(Description = "If the provider doesn't have a provider identifier (missing \"ukprn claim\") then end the user's session and navigate to the landing page with a message")]
        public void Authenticated_EmptyProviderIdentifier()
        {
            var mediator = new HomeMediatorBuilder().Build();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "User001"));
            identity.AddClaim(new Claim(Constants.ClaimTypes.Ukprn, ""));
            identity.AddClaim(new Claim(ClaimTypes.Role, Constants.Roles.Faa));
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertMessage(HomeMediatorCodes.Authorize.MissingProviderIdentifier, AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
        }

        [Test(Description = "If the user doesn't have the service permission (missing \"service claim\") then end the user's session and return them to landing page with a message")]
        public void Authenticated_MissingServicePermission()
        {
            var mediator = new HomeMediatorBuilder().Build();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "User001"));
            identity.AddClaim(new Claim(Constants.ClaimTypes.Ukprn, "00001"));
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertMessage(HomeMediatorCodes.Authorize.MissingServicePermission, AuthorizeMessages.MissingServicePermission, UserMessageLevel.Warning);
        }

        [Test(Description = "If there is no provider profile then direct the user to the \"sites\" page with a message")]
        public void Authenticated_NoProviderProfile()
        {
            var mediator = new HomeMediatorBuilder().Build();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "User001"));
            identity.AddClaim(new Claim(Constants.ClaimTypes.Ukprn, "00001"));
            identity.AddClaim(new Claim(ClaimTypes.Role, Constants.Roles.Faa));
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertMessage(HomeMediatorCodes.Authorize.NoProviderProfile, AuthorizeMessages.NoProviderProfile, UserMessageLevel.Warning);
        }

        [Test(Description = "If there is not at least 1 site then direct the user to the \"sites\" page with a message")]
        public void Authenticated_FailedMinimumSitesCountCheck()
        {
            var providerProvider = new Mock<IProviderProvider>();
            providerProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(new ProviderViewModel());
            var mediator = new HomeMediatorBuilder().With(providerProvider).Build();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "User001"));
            identity.AddClaim(new Claim(Constants.ClaimTypes.Ukprn, "00001"));
            identity.AddClaim(new Claim(ClaimTypes.Role, Constants.Roles.Faa));
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertMessage(HomeMediatorCodes.Authorize.FailedMinimumSitesCountCheck, AuthorizeMessages.FailedMinimumSitesCountCheck, UserMessageLevel.Warning);
        }

        [Test(Description = "If there is no user profile then direct the user to the \"user profile\" page with a message")]
        public void Authenticated_NoUserProfile()
        {
            var providerProvider = new Mock<IProviderProvider>();
            var providerViewModel = new ProviderViewModel
            {
                ProviderSiteViewModels = new List<ProviderSiteViewModel> {new ProviderSiteViewModel()}
            };
            providerProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(providerViewModel);
            var mediator = new HomeMediatorBuilder().With(providerProvider).Build();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "User001"));
            identity.AddClaim(new Claim(Constants.ClaimTypes.Ukprn, "00001"));
            identity.AddClaim(new Claim(ClaimTypes.Role, Constants.Roles.Faa));
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertCode(HomeMediatorCodes.Authorize.Ok);
        }

        [Test(Description = "User has all claims, a complete provider profile and has verified their email address")]
        public void Authenticated_Ok()
        {
            var providerProvider = new Mock<IProviderProvider>();
            var providerViewModel = new ProviderViewModel
            {
                ProviderSiteViewModels = new List<ProviderSiteViewModel> {new ProviderSiteViewModel()}
            };
            providerProvider.Setup(p => p.GetProviderViewModel(It.IsAny<string>())).Returns(providerViewModel);
            var mediator = new HomeMediatorBuilder().With(providerProvider).Build();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "User001"));
            identity.AddClaim(new Claim(Constants.ClaimTypes.Ukprn, "00001"));
            identity.AddClaim(new Claim(ClaimTypes.Role, Constants.Roles.Faa));
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertCode(HomeMediatorCodes.Authorize.Ok);
        }
    }
}