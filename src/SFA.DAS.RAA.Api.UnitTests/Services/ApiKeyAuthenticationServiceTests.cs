namespace SFA.DAS.RAA.Api.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;
    using Api.Services;
    using Apprenticeships.Domain.Entities.Raa.RaaApi;
    using Apprenticeships.Domain.Raa.Interfaces.Repositories;
    using Constants;
    using Extensions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ApiKeyAuthenticationServiceTests
    {
        private const string InvalidApiKey = "INVALID";
        private const string ValidApiKey = "VALID";
        private const string ValidProviderApiKey = "PROVIDER";
        private const string ValidEmployerApiKey = "EMPLOYER";

        private readonly RaaApiUser _validApiUser = new RaaApiUser
        {
            Name = ValidApiKey,
            Id = 1,
            Guid = Guid.NewGuid()
        };

        private readonly RaaApiUser _validProviderApiUser = new RaaApiUser
        {
            Name = ValidProviderApiKey,
            UserType = RaaApiUserType.Provider,
            Id = 2,
            Guid = Guid.NewGuid(),
            SurrogateId = "10033670"
        };

        private readonly RaaApiUser _validEmployerApiUser = new RaaApiUser
        {
            Name = ValidEmployerApiKey,
            UserType = RaaApiUserType.Employer,
            Id = 3,
            Guid = Guid.NewGuid(),
            SurrogateId = "228616654"
        };

        private Mock<IRaaApiUserRepository> _raaApiUserRepository;
        private IAuthenticationService _authenticationService;

        [SetUp]
        public void Setup()
        {
            _raaApiUserRepository = new Mock<IRaaApiUserRepository>();
            _raaApiUserRepository.Setup(r => r.GetUser(It.IsAny<string>())).Returns(RaaApiUser.UnknownApiUser);
            _raaApiUserRepository.Setup(r => r.GetUser(ValidApiKey)).Returns(_validApiUser);
            _raaApiUserRepository.Setup(r => r.GetUser(ValidProviderApiKey)).Returns(_validProviderApiUser);
            _raaApiUserRepository.Setup(r => r.GetUser(ValidEmployerApiKey)).Returns(_validEmployerApiUser);

            _authenticationService = new ApiKeyAuthenticationService(_raaApiUserRepository.Object);
        }

        [Test]
        public void NoApiKeyTest()
        {
            var principal = _authenticationService.Authenticate(new Dictionary<string, string>());

            ValidatePrincipal(principal, false);
        }

        [Test]
        public void InvalidApiKeyTest()
        {
            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, InvalidApiKey }
            };
            var principal = _authenticationService.Authenticate(claims);

            principal.IsInRole(Roles.Provider).Should().BeFalse();
            principal.IsInRole(Roles.Employer).Should().BeFalse();
            principal.IsInRole(Roles.Admin).Should().BeFalse();
            var claimsIdentity = ValidatePrincipal(principal, false);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == InvalidApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == RaaApiUser.UnknownApiUser.Name).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData).Should().BeTrue();
            claimsIdentity.Name.Should().Be(RaaApiUser.UnknownApiUser.Name);
            var raaApiUser = claimsIdentity.GetRaaApiUser();
            raaApiUser.Equals(RaaApiUser.UnknownApiUser).Should().BeTrue();
        }

        [Test]
        public void ValidApiKeyTest()
        {
            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, ValidApiKey }
            };
            var principal = _authenticationService.Authenticate(claims);

            principal.IsInRole(Roles.Provider).Should().BeFalse();
            principal.IsInRole(Roles.Employer).Should().BeFalse();
            principal.IsInRole(Roles.Admin).Should().BeFalse();
            var claimsIdentity = ValidatePrincipal(principal, true);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == ValidApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == ValidApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData).Should().BeTrue();
            claimsIdentity.Name.Should().Be(ValidApiKey);
            var raaApiUser = claimsIdentity.GetRaaApiUser();
            raaApiUser.Equals(_validApiUser).Should().BeTrue();
        }

        [Test]
        public void ValidProviderApiKeyTest()
        {
            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, ValidProviderApiKey }
            };
            var principal = _authenticationService.Authenticate(claims);

            principal.IsInRole(Roles.Provider).Should().BeTrue();
            principal.IsInRole(Roles.Employer).Should().BeFalse();
            principal.IsInRole(Roles.Admin).Should().BeFalse();
            var claimsIdentity = ValidatePrincipal(principal, true);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == ValidProviderApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == ValidProviderApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData).Should().BeTrue();
            claimsIdentity.Name.Should().Be(ValidProviderApiKey);
            var raaApiUser = claimsIdentity.GetRaaApiUser();
            raaApiUser.Equals(_validProviderApiUser).Should().BeTrue();
        }

        [Test]
        public void ValidEmployerApiKeyTest()
        {
            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, ValidEmployerApiKey }
            };
            var principal = _authenticationService.Authenticate(claims);

            principal.IsInRole(Roles.Provider).Should().BeFalse();
            principal.IsInRole(Roles.Employer).Should().BeTrue();
            principal.IsInRole(Roles.Admin).Should().BeFalse();
            var claimsIdentity = ValidatePrincipal(principal, true);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == ValidEmployerApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == ValidEmployerApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData).Should().BeTrue();
            claimsIdentity.Name.Should().Be(ValidEmployerApiKey);
            var raaApiUser = claimsIdentity.GetRaaApiUser();
            raaApiUser.Equals(_validEmployerApiUser).Should().BeTrue();
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
    }
}