using System.Security.Claims;
using NUnit.Framework;
using SFA.Apprenticeships.Web.Common.Constants;
using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;
using SFA.Apprenticeships.Web.Recruit.Constants.Messages;
using SFA.Apprenticeships.Web.Recruit.Mediators.Home;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Home
{
    [TestFixture]
    public class HomeMediatorTests
    {
        [Test(Description = "If the provider doesn't have a provider identifier (missing \"ukprn claim\") then end the user's session and navigate to the landing page with a message")]
        public void Authenticated_MissingProviderIdentifier()
        {
            var mediator = new HomeMediatorBuilder().Build();

            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertMessage(HomeMediatorCodes.Authorize.MissingProviderIdentifier, AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
        }

        [Test(Description = "If the user doesn't have the service permission (missing \"service claim\") then end the user's session and return them to landing page with a message")]
        public void Authenticated_MissingServicePermission()
        {
            var mediator = new HomeMediatorBuilder().Build();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(Constants.ClaimTypes.Ukprn, "00001"));
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertMessage(HomeMediatorCodes.Authorize.MissingServicePermission, AuthorizeMessages.MissingServicePermission, UserMessageLevel.Warning);
        }

        [Test(Description = "User has all claims, a complete provider profile and has verified their email address")]
        public void Authenticated_Ok()
        {
            var mediator = new HomeMediatorBuilder().Build();

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(Constants.ClaimTypes.Ukprn, "00001"));
            identity.AddClaim(new Claim(ClaimTypes.Role, Constants.Roles.Faa));
            var principal = new ClaimsPrincipal(identity);

            var response = mediator.Authorize(principal);
            response.AssertCode(HomeMediatorCodes.Authorize.Ok);
        }
    }
}