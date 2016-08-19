namespace SFA.Apprenticeships.Web.Common.UnitTests.Providers
{
    using System;
    using System.Web;
    using Application.Interfaces.Candidates;
    using Builders;
    using Common.Providers;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class HelpCookieProviderTests
    {
        [Test]
        public void VisitorFirstVisit()
        {
            var provider = new HelpCookieProviderBuilder().Build();
            var httpResponse = new Mock<HttpResponseBase>();
            var responseCookies = new HttpCookieCollection();
            httpResponse.Setup(m => m.Cookies).Returns(responseCookies);
            var httpContext = new HttpContextBuilder().With(httpResponse).Build();

            var showSearchTour = provider.ShowSearchTour(httpContext, null);

            showSearchTour.Should().BeTrue();
            var cookie = responseCookies.Get(HelpCookieProvider.CookieName);
            cookie.Should().NotBeNull();
            var cookieValue = cookie[HelpCookieProvider.CookieKeys.ShowSearchTour.ToString()];
            cookieValue.Should().Be(Guid.Empty.ToString());
        }
        
        [Test]
        public void VisitorSubsequentVisit()
        {
            var provider = new HelpCookieProviderBuilder().Build();
            var httpRequest = new Mock<HttpRequestBase>();
            var requestCookies = new HttpCookieCollection();
            var existingCookie = new HttpCookie(HelpCookieProvider.CookieName);
            existingCookie[HelpCookieProvider.CookieKeys.ShowSearchTour.ToString()] = Guid.Empty.ToString();
            requestCookies.Add(existingCookie);
            httpRequest.Setup(m => m.Cookies).Returns(requestCookies);
            var httpResponse = new Mock<HttpResponseBase>();
            var responseCookies = new HttpCookieCollection();
            httpResponse.Setup(m => m.Cookies).Returns(responseCookies);
            var httpContext = new HttpContextBuilder().With(httpRequest).With(httpResponse).Build();

            var showSearchTour = provider.ShowSearchTour(httpContext, null);

            showSearchTour.Should().BeFalse();
            var cookie = responseCookies.Get(HelpCookieProvider.CookieName);
            cookie.Should().BeNull();
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void CandidateFirstVisit(bool showSearchTourPreference)
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var helpPreferences = new HelpPreferences
            {
                ShowSearchTour = showSearchTourPreference
            };
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).With(helpPreferences).Build);
            var provider = new HelpCookieProviderBuilder().With(candidateService).Build();
            var httpResponse = new Mock<HttpResponseBase>();
            var responseCookies = new HttpCookieCollection();
            httpResponse.Setup(m => m.Cookies).Returns(responseCookies);
            var httpContext = new HttpContextBuilder().With(httpResponse).Build();

            var showSearchTour = provider.ShowSearchTour(httpContext, candidateId);

            showSearchTour.Should().Be(showSearchTourPreference);
            var cookie = responseCookies.Get(HelpCookieProvider.CookieName);
            cookie.Should().NotBeNull();
            var cookieValue = cookie[HelpCookieProvider.CookieKeys.ShowSearchTour.ToString()];
            cookieValue.Should().Be(candidateId.ToString());
            candidateService.Verify(cs => cs.GetCandidate(It.IsAny<Guid>()), Times.Once);
        }
        
        [Test]
        public void CandidateSubsequentVisit()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var provider = new HelpCookieProviderBuilder().With(candidateService).Build();
            var httpRequest = new Mock<HttpRequestBase>();
            var requestCookies = new HttpCookieCollection();
            var existingCookie = new HttpCookie(HelpCookieProvider.CookieName);
            existingCookie[HelpCookieProvider.CookieKeys.ShowSearchTour.ToString()] = candidateId.ToString();
            requestCookies.Add(existingCookie);
            httpRequest.Setup(m => m.Cookies).Returns(requestCookies);
            var httpResponse = new Mock<HttpResponseBase>();
            var responseCookies = new HttpCookieCollection();
            httpResponse.Setup(m => m.Cookies).Returns(responseCookies);
            var httpContext = new HttpContextBuilder().With(httpRequest).With(httpResponse).Build();

            var showSearchTour = provider.ShowSearchTour(httpContext, candidateId);

            showSearchTour.Should().BeFalse();
            var cookie = responseCookies.Get(HelpCookieProvider.CookieName);
            cookie.Should().BeNull();
            candidateService.Verify(cs => cs.GetCandidate(It.IsAny<Guid>()), Times.Never);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void NewCandidateSubsequentVisit(bool showSearchTourPreference)
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var helpPreferences = new HelpPreferences
            {
                ShowSearchTour = showSearchTourPreference
            };
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).With(helpPreferences).Build);
            var provider = new HelpCookieProviderBuilder().With(candidateService).Build();
            var httpRequest = new Mock<HttpRequestBase>();
            var requestCookies = new HttpCookieCollection();
            var existingCookie = new HttpCookie(HelpCookieProvider.CookieName);
            existingCookie[HelpCookieProvider.CookieKeys.ShowSearchTour.ToString()] = Guid.NewGuid().ToString();
            requestCookies.Add(existingCookie);
            httpRequest.Setup(m => m.Cookies).Returns(requestCookies);
            var httpResponse = new Mock<HttpResponseBase>();
            var responseCookies = new HttpCookieCollection();
            httpResponse.Setup(m => m.Cookies).Returns(responseCookies);
            var httpContext = new HttpContextBuilder().With(httpRequest).With(httpResponse).Build();

            var showSearchTour = provider.ShowSearchTour(httpContext, candidateId);

            showSearchTour.Should().Be(showSearchTourPreference);
            var cookie = responseCookies.Get(HelpCookieProvider.CookieName);
            cookie.Should().NotBeNull();
            var cookieValue = cookie[HelpCookieProvider.CookieKeys.ShowSearchTour.ToString()];
            cookieValue.Should().Be(candidateId.ToString());
            candidateService.Verify(cs => cs.GetCandidate(It.IsAny<Guid>()), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CandidateSubsequentVisitAfterLoggingIn(bool showSearchTourPreference)
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var helpPreferences = new HelpPreferences
            {
                ShowSearchTour = showSearchTourPreference
            };
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).With(helpPreferences).Build);
            var provider = new HelpCookieProviderBuilder().With(candidateService).Build();
            var httpRequest = new Mock<HttpRequestBase>();
            var requestCookies = new HttpCookieCollection();
            var existingCookie = new HttpCookie(HelpCookieProvider.CookieName);
            existingCookie[HelpCookieProvider.CookieKeys.ShowSearchTour.ToString()] = Guid.Empty.ToString();
            requestCookies.Add(existingCookie);
            httpRequest.Setup(m => m.Cookies).Returns(requestCookies);
            var httpResponse = new Mock<HttpResponseBase>();
            var responseCookies = new HttpCookieCollection();
            httpResponse.Setup(m => m.Cookies).Returns(responseCookies);
            var httpContext = new HttpContextBuilder().With(httpRequest).With(httpResponse).Build();

            var showSearchTour = provider.ShowSearchTour(httpContext, candidateId);

            showSearchTour.Should().BeFalse();
            var cookie = responseCookies.Get(HelpCookieProvider.CookieName);
            cookie.Should().NotBeNull();
            var cookieValue = cookie[HelpCookieProvider.CookieKeys.ShowSearchTour.ToString()];
            cookieValue.Should().Be(candidateId.ToString());
            candidateService.Verify(cs => cs.GetCandidate(It.IsAny<Guid>()), Times.Once);
        }
    }
}