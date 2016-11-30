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
        private readonly string _unknownApiKey = Guid.NewGuid().ToString();
        private readonly string _validApiKey = Guid.NewGuid().ToString();
        private readonly string _validProviderApiKey = Guid.NewGuid().ToString();
        private readonly string _validEmployerApiKey = Guid.NewGuid().ToString();

        private readonly RaaApiUser _validApiUser = new RaaApiUser
        {
            PrimaryApiKey = Guid.NewGuid(),
            SecondaryApiKey = Guid.NewGuid(),
            ReferencedEntityId = 1,
            ReferencedEntityGuid = Guid.NewGuid()
        };

        private readonly RaaApiUser _validProviderApiUser = new RaaApiUser
        {
            PrimaryApiKey = Guid.NewGuid(),
            SecondaryApiKey = Guid.NewGuid(),
            UserType = RaaApiUserType.Provider,
            ReferencedEntityId = 2,
            ReferencedEntityGuid = Guid.NewGuid(),
            ReferencedEntitySurrogateId = 10033670
        };

        private readonly RaaApiUser _validEmployerApiUser = new RaaApiUser
        {
            PrimaryApiKey = Guid.NewGuid(),
            SecondaryApiKey = Guid.NewGuid(),
            UserType = RaaApiUserType.Employer,
            ReferencedEntityId = 3,
            ReferencedEntityGuid = Guid.NewGuid(),
            ReferencedEntitySurrogateId = 228616654
        };

        private Mock<IRaaApiUserRepository> _raaApiUserRepository;
        private IAuthenticationService _authenticationService;

        [SetUp]
        public void Setup()
        {
            _raaApiUserRepository = new Mock<IRaaApiUserRepository>();
            _raaApiUserRepository.Setup(r => r.GetUser(It.IsAny<string>())).Returns(RaaApiUser.UnknownApiUser);
            _raaApiUserRepository.Setup(r => r.GetUser(_validApiKey)).Returns(_validApiUser);
            _raaApiUserRepository.Setup(r => r.GetUser(_validProviderApiKey)).Returns(_validProviderApiUser);
            _raaApiUserRepository.Setup(r => r.GetUser(_validEmployerApiKey)).Returns(_validEmployerApiUser);

            _authenticationService = new ApiKeyAuthenticationService(_raaApiUserRepository.Object);
        }

        [Test]
        public void NoApiKeyTest()
        {
            var claims = new Dictionary<string, string>();
            var principal = _authenticationService.Authenticate(claims);

            var expectedName = $"RaaApiUser_MissingApiKey";
            principal.IsInRole(Roles.Provider).Should().BeFalse();
            principal.IsInRole(Roles.Employer).Should().BeFalse();
            principal.IsInRole(Roles.Admin).Should().BeFalse();
            var claimsIdentity = ValidatePrincipal(principal, false);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication).Should().BeFalse();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == expectedName).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData).Should().BeFalse();
            claimsIdentity.Name.Should().Be(expectedName);
            var raaApiUser = claimsIdentity.GetRaaApiUser();
            raaApiUser.Equals(RaaApiUser.UnknownApiUser).Should().BeTrue();
        }

        [Test]
        public void InvalidApiKeyTest()
        {
            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, InvalidApiKey }
            };
            var principal = _authenticationService.Authenticate(claims);

            var expectedName = $"RaaApiUser_InvalidApiKey_{InvalidApiKey}";
            principal.IsInRole(Roles.Provider).Should().BeFalse();
            principal.IsInRole(Roles.Employer).Should().BeFalse();
            principal.IsInRole(Roles.Admin).Should().BeFalse();
            var claimsIdentity = ValidatePrincipal(principal, false);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == InvalidApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == expectedName).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData).Should().BeFalse();
            claimsIdentity.Name.Should().Be(expectedName);
            var raaApiUser = claimsIdentity.GetRaaApiUser();
            raaApiUser.Equals(RaaApiUser.UnknownApiUser).Should().BeTrue();
        }

        [Test]
        public void UnknownApiKeyTest()
        {
            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, _unknownApiKey }
            };
            var principal = _authenticationService.Authenticate(claims);

            var expectedName = $"RaaApiUser_UnknownApiKey_{_unknownApiKey}";
            principal.IsInRole(Roles.Provider).Should().BeFalse();
            principal.IsInRole(Roles.Employer).Should().BeFalse();
            principal.IsInRole(Roles.Admin).Should().BeFalse();
            var claimsIdentity = ValidatePrincipal(principal, false);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == _unknownApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == expectedName).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData).Should().BeTrue();
            claimsIdentity.Name.Should().Be(expectedName);
            var raaApiUser = claimsIdentity.GetRaaApiUser();
            raaApiUser.Equals(RaaApiUser.UnknownApiUser).Should().BeTrue();
        }

        [Test]
        public void ValidApiKeyTest()
        {
            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, _validApiKey }
            };
            var principal = _authenticationService.Authenticate(claims);

            var expectedName = $"RaaApiUser_ApiKey_{_validApiKey}";
            principal.IsInRole(Roles.Provider).Should().BeFalse();
            principal.IsInRole(Roles.Employer).Should().BeFalse();
            principal.IsInRole(Roles.Admin).Should().BeFalse();
            var claimsIdentity = ValidatePrincipal(principal, true);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == _validApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == expectedName).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData).Should().BeTrue();
            claimsIdentity.Name.Should().Be(expectedName);
            var raaApiUser = claimsIdentity.GetRaaApiUser();
            raaApiUser.Equals(_validApiUser).Should().BeTrue();
        }

        [Test]
        public void ValidProviderApiKeyTest()
        {
            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, _validProviderApiKey }
            };
            var principal = _authenticationService.Authenticate(claims);

            var expectedName = $"RaaApiUser_ApiKey_{_validProviderApiKey}";
            principal.IsInRole(Roles.Provider).Should().BeTrue();
            principal.IsInRole(Roles.Employer).Should().BeFalse();
            principal.IsInRole(Roles.Admin).Should().BeFalse();
            var claimsIdentity = ValidatePrincipal(principal, true);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == _validProviderApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == expectedName).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData).Should().BeTrue();
            claimsIdentity.Name.Should().Be(expectedName);
            var raaApiUser = claimsIdentity.GetRaaApiUser();
            raaApiUser.Equals(_validProviderApiUser).Should().BeTrue();
        }

        [Test]
        public void ValidEmployerApiKeyTest()
        {
            var claims = new Dictionary<string, string>
            {
                { ApiKeyAuthenticationService.ApiKeyKey, _validEmployerApiKey }
            };
            var principal = _authenticationService.Authenticate(claims);

            var expectedName = $"RaaApiUser_ApiKey_{_validEmployerApiKey}";
            principal.IsInRole(Roles.Provider).Should().BeFalse();
            principal.IsInRole(Roles.Employer).Should().BeTrue();
            principal.IsInRole(Roles.Admin).Should().BeFalse();
            var claimsIdentity = ValidatePrincipal(principal, true);
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Authentication && c.Value == _validEmployerApiKey).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.Name && c.Value == expectedName).Should().BeTrue();
            claimsIdentity.HasClaim(c => c.Type == ClaimTypes.UserData).Should().BeTrue();
            claimsIdentity.Name.Should().Be(expectedName);
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