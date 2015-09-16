using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Security;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Web.Common.Providers;
using SFA.Apprenticeships.Web.Common.UnitTests.Builders;

namespace SFA.Apprenticeships.Web.Common.UnitTests.Providers
{
    [TestFixture]
    public class CookieAuthorizationDataProviderTests
    {
        private const string CookieName = "User.Authorization";

        [Test]
        public void AddClaim()
        {
            var provider = new CookieAuthorizationDataProvider();

            var claim = new Claim("TestType", "TestValue");

            var httpContext = new HttpContextBuilder().Build();

            provider.AddClaim(claim, httpContext, "provider@user.com");

            var cookie = httpContext.Response.Cookies.Get(CookieName);

            cookie.Should().NotBeNull();
            if (cookie != null)
            {
                cookie.Value.Should().NotBeNullOrEmpty();
                cookie.Value.Should().NotContain("TestType");
                cookie.Value.Should().NotContain("TestValue");
                var unprotectedValue = CookieAuthorizationDataProvider.Unprotect(cookie.Value);
                unprotectedValue.Should().Contain("TestType");
                unprotectedValue.Should().Contain("TestValue");
            }
        }

        [Test]
        public void AddClaims()
        {
            var provider = new CookieAuthorizationDataProvider();

            var claim1 = new Claim("TestType1", "TestValue1");
            var claim2 = new Claim("TestType2", "TestValue2");

            var httpContext = new HttpContextBuilder().Build();

            provider.AddClaim(claim1, httpContext, "provider@user.com");
            provider.AddClaim(claim2, httpContext, "provider@user.com");

            var cookie = httpContext.Response.Cookies.Get(CookieName);

            cookie.Should().NotBeNull();
            if (cookie != null)
            {
                var unprotectedValue = CookieAuthorizationDataProvider.Unprotect(cookie.Value);
                unprotectedValue.Should().Contain("TestType1");
                unprotectedValue.Should().Contain("TestValue1");
                unprotectedValue.Should().Contain("TestType2");
                unprotectedValue.Should().Contain("TestValue2");
            }
        }

        [Test]
        public void GetClaim()
        {
            var provider = new CookieAuthorizationDataProvider();

            var claim = new Claim("TestType", "TestValue");

            var httpContext = new HttpContextBuilder().Build();

            provider.AddClaim(claim, httpContext, "provider@user.com");

            var claims = provider.GetClaims(httpContext, "provider@user.com");

            claims.Length.Should().Be(1);
            claims[0].Type.Should().Be("TestType");
            claims[0].Value.Should().Be("TestValue");
        }

        [Test]
        public void GetClaims()
        {
            var provider = new CookieAuthorizationDataProvider();

            var claim1 = new Claim("TestType1", "TestValue1");
            var claim2 = new Claim("TestType2", "TestValue2");

            var httpContext = new HttpContextBuilder().Build();

            provider.AddClaim(claim1, httpContext, "provider@user.com");
            provider.AddClaim(claim2, httpContext, "provider@user.com");

            var claims = provider.GetClaims(httpContext, "provider@user.com");

            claims.Length.Should().Be(2);
            claims[0].Type.Should().Be("TestType1");
            claims[0].Value.Should().Be("TestValue1");
            claims[1].Type.Should().Be("TestType2");
            claims[1].Value.Should().Be("TestValue2");
        }
    }
}