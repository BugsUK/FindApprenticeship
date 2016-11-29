namespace SFA.DAS.RAA.Api.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using Api.Services;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ApiKeyAuthenticationServiceTests
    {
        private const string InvalidApiKey = "INVALID";
        private const string ValidApiKey = "VALID";
        private const string ValidProviderApiKey = "PROVIDER";
        private const string ValidEmployerApiKey = "EMPLOYER";

        [Test]
        public void NoApiKeyTest()
        {
            var service = new ApiKeyAuthenticationService();

            var principal = service.Authenticate(new Dictionary<string, string>());

            var claimsIdentity = ValidatePrincipal(principal, false);
            claimsIdentity.Claims.Count().Should().Be(0);
        }

        private static ClaimsIdentity ValidatePrincipal(IPrincipal principal, bool expectedIsAuthenticated)
        {
            principal.Should().NotBeNull();
            principal.Should().BeOfType<ClaimsPrincipal>();
            var claimsPrincipal = (ClaimsPrincipal) principal;
            var identity = claimsPrincipal.Identity;
            identity.Should().NotBeNull();
            identity.Should().BeOfType<ClaimsIdentity>();
            var claimsIdentity = (ClaimsIdentity) identity;
            claimsIdentity.IsAuthenticated.Should().Be(expectedIsAuthenticated);
            return claimsIdentity;
        }

        [Test]
        public void InvalidApiKeyTest()
        {
            var service = new ApiKeyAuthenticationService();

            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, InvalidApiKey }
            };
            var principal = service.Authenticate(claims);

            var claimsIdentity = ValidatePrincipal(principal, false);
            claimsIdentity.Claims.Count().Should().Be(1);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == InvalidApiKey).Should().BeTrue();
        }

        [Test]
        public void ValidApiKeyTest()
        {
            var service = new ApiKeyAuthenticationService();

            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, ValidApiKey }
            };
            var principal = service.Authenticate(claims);

            var claimsIdentity = ValidatePrincipal(principal, true);
            claimsIdentity.Claims.Count().Should().Be(1);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == InvalidApiKey).Should().BeTrue();
        }
    }
}