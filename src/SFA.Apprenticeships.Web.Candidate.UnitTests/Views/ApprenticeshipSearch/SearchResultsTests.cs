namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using Builders;
    using Common.Framework;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SearchResultsTests
    {
        [TestCase(5)]
        [TestCase(6)]
        public void NonNationalResultsPerPageDropDown(int totalLocalHits)
        {
            var viewModel = new ApprenticeshipSearchResponseViewModelBuilder().WithTotalLocalHits(totalLocalHits).Build();
            var result = new SearchResultsViewBuilder().With(viewModel).Render();

            var resultsPerPageDropDown = result.GetElementbyId("results-per-page");

            if (totalLocalHits > 5)
            {
                resultsPerPageDropDown.Should().NotBeNull();
            }
            else
            {
                resultsPerPageDropDown.Should().BeNull();
            }
        }

        [TestCase(5)]
        [TestCase(6)]
        public void NationalResultsPerPageDropDown(int totalNationalHits)
        {
            var viewModel = new ApprenticeshipSearchResponseViewModelBuilder().WithTotalNationalHits(totalNationalHits).Build();
            var result = new SearchResultsViewBuilder().With(viewModel).Render();

            var resultsPerPageDropDown = result.GetElementbyId("results-per-page");

            if (totalNationalHits > 5)
            {
                resultsPerPageDropDown.Should().NotBeNull();
            }
            else
            {
                resultsPerPageDropDown.Should().BeNull();
            }
        }

        [Test]
        public void PostedDate()
        {
            // Arrange.
            const int hits = 5;

            var vacancySearchViewModel = new ApprenticeshipSearchViewModelBuilder()
                .Build();

            var viewModel = new ApprenticeshipSearchResponseViewModelBuilder()
                .WithVacancySearch(vacancySearchViewModel)
                .WithTotalLocalHits(hits)
                .Build();

            // Act.
            var result = new SearchResultsViewBuilder().With(viewModel).Render();

            // Assert.
            viewModel.Vacancies.Count().Should().Be(hits);

            foreach (var vacancy in viewModel.Vacancies)
            {
                var id = string.Format("posted-date-{0}", vacancy.Id);

                var element = result.GetElementbyId(id);

                var friendlyPostedDate = vacancy.PostedDate.ToFriendlyDaysAgo();

                element.Should().NotBeNull();
                element.InnerText.Should().Contain(friendlyPostedDate);
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SaveVacancyLinks(bool isAuthenticated)
        {
            // Arrange.
            const int hits = 5;

            var vacancySearchViewModel = new ApprenticeshipSearchViewModelBuilder()
                .Build();

            var viewModel = new ApprenticeshipSearchResponseViewModelBuilder()
                .WithVacancySearch(vacancySearchViewModel)
                .WithTotalLocalHits(hits)
                .Build();

            var httpContext = CreateMockHttpContext(isAuthenticated);

            // Act.
            var result = new SearchResultsViewBuilder().With(viewModel).Render(httpContext);

            // Assert.
            viewModel.Vacancies.Count().Should().Be(hits);

            foreach (var vacancy in viewModel.Vacancies)
            {
                var idFormats = new[] { "save-vacancy-link-{0}", "resume-link-{0}", "applied-label-{0}" };

                foreach (var idFormat in idFormats)
                {
                    var id = string.Format(idFormat, vacancy.Id);

                    var element = result.GetElementbyId(id);

                    if (isAuthenticated)
                    {
                        element.Should().NotBeNull(id);
                    }
                    else
                    {
                        element.Should().BeNull(id);
                    }
                }
            }
        }

        #region Helpers

        private static HttpContextBase CreateMockHttpContext(bool isAuthenticated)
        {
            // HttpRequestBase.
            var mockRequest = new Mock<HttpRequestBase>(MockBehavior.Loose);

            mockRequest.Setup(m => m.IsLocal).Returns(false);
            mockRequest.Setup(m => m.ApplicationPath).Returns("/");
            mockRequest.Setup(m => m.ServerVariables).Returns(new NameValueCollection());
            mockRequest.Setup(m => m.RawUrl).Returns(string.Empty);
            mockRequest.Setup(m => m.Cookies).Returns(new HttpCookieCollection());
            mockRequest.Setup(m => m.IsAuthenticated).Returns(isAuthenticated);

            // HttpResponseBase.
            var mockResponse = new Mock<HttpResponseBase>(MockBehavior.Loose);

            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(virtualPath => virtualPath);
            mockResponse.Setup(m => m.Cookies).Returns(new HttpCookieCollection());

            // HttpContextBase.
            var mockHttpContext = new Mock<HttpContextBase>(MockBehavior.Loose);

            mockHttpContext.Setup(m => m.Items).Returns(new Hashtable());
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockHttpContext.Object;
        }

        #endregion
    }
}