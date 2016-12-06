namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web;
    using Candidate.ViewModels.Account;
    using Candidate.Views.ApprenticeshipSearch;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;

    [TestFixture]
    public class IndexTests : MediatorTestsBase
    {
        [Test]
        public void SearchMode_Keyword_BasicVisibilityTest()
        {
            var index = new Index();

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Keyword, false).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeTrue();

            view.GetElementbyId("saved-searches-tab-control").Should().BeNull();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeTrue();

            //Shares parent with loc-within here.
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" active").Should().BeTrue();

            view.GetElementbyId("browse-button").Should().NotBeNull();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" browsetab").Should().BeTrue();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();

            view.GetElementbyId("reset-search-options-link").Should().NotBeNull();
            view.GetElementbyId("reset-search-options-link").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("reset-search-options-link").ParentNode.Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
        }

        [Test]
        public void SearchMode_Category_BasicVisibilityTest()
        {
            var index = new Index();

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Category, false).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeTrue();

            view.GetElementbyId("saved-searches-tab-control").Should().BeNull();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeTrue();

            //Shares parent with loc-within here.
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();

            view.GetElementbyId("browse-button").Should().NotBeNull();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" browsetab").Should().BeTrue();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" active").Should().BeTrue();

            view.GetElementbyId("reset-search-options-link").Should().NotBeNull();
            view.GetElementbyId("reset-search-options-link").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("reset-search-options-link").ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeTrue();
        }

        /// <summary>
        /// Form fields should no longer be visible and a message should be present
        /// to tell users loading of categories prevents filering by them
        /// </summary>
        [Test]
        public void SearchMode_Category_NullCategoriesVisibilityTest()
        {
            ReferenceDataService.Setup(rds => rds.GetCategories());
            var index = new Index();

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Category, false).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeFalse();

            view.GetElementbyId("saved-searches-tab-control").Should().BeNull();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeFalse();

            //Shares parent with loc-within here.
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" searchtab").Should().BeTrue();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();

            view.GetElementbyId("browse-button").Should().NotBeNull();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" browsetab").Should().BeFalse();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();

            view.GetElementbyId("reset-search-options-link").Should().NotBeNull();
            view.GetElementbyId("reset-search-options-link").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("reset-search-options-link").ParentNode.Attributes["class"].Value.Contains(" browsetab").Should().BeFalse();
        }

        [Test]
        public void SearchMode_SavedSearches_AnonymousUserTest()
        {
            var index = new Index();
            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.SavedSearches, false).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("saved-searches-tab-control").Should().BeNull();
        }

        [TestCase(1)]
        [TestCase(5)]
        public void SearchMode_SavedSearches_AuthenticatedUserWithSavedSearchesTest(int savedSearchCount)
        {
            var index = new Index();
            var candidateId = Guid.NewGuid();
            var mockViewModel = new Fixture().Build<SavedSearchViewModel>().CreateMany(savedSearchCount);

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            CandidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var searchViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false).ViewModel;
            var view = index.RenderAsHtml(CreateMockHttpContext(true), searchViewModel);

            var link = view.GetElementbyId("saved-searches-tab-control");

            link.Should().NotBeNull();
            link.Attributes["class"].Value.Should().Contain(" active");

            var button = view.GetElementbyId("run-saved-search-button");

            button.Attributes["class"].Value.Should().Contain(" savedsearchtab");
            button.Attributes["class"].Value.Should().Contain(" active");

            view.GetElementbyId("reset-search-options-link").Should().NotBeNull();
            view.GetElementbyId("reset-search-options-link").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("reset-search-options-link").ParentNode.Attributes["class"].Value.Contains(" savedsearchtab").Should().BeFalse();
            view.GetElementbyId("saved-searches-settings-link").Should().NotBeNull();
        }

        [Test]
        public void SearchMode_SavedSearches_AuthenticatedUserWithNoSavedSearchesTest()
        {
            var index = new Index();
            var candidateId = Guid.NewGuid();
            var mockViewModel = new List<SavedSearchViewModel>();

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            CandidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var searchViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false).ViewModel;
            var view = index.RenderAsHtml(CreateMockHttpContext(true), searchViewModel);

            var link = view.GetElementbyId("saved-searches-tab-control");

            link.Should().NotBeNull();
            link.Attributes["class"].Value.Should().Contain(" active");

            view.GetElementbyId("run-saved-search-button").Should().BeNull();
            view.GetElementbyId("saved-searches-settings-link").Should().BeNull();
        }

        // TODO: AG: DEBT: refactor this function out of each test class.
        private static HttpContextBase CreateMockHttpContext(bool isAuthenticated)
        {
            // Response.
            var mockRequest = new Mock<HttpRequestBase>(MockBehavior.Loose);

            mockRequest.Setup(m => m.IsLocal).Returns(false);
            mockRequest.Setup(m => m.ApplicationPath).Returns("/");
            mockRequest.Setup(m => m.ServerVariables).Returns(new NameValueCollection());
            mockRequest.Setup(m => m.RawUrl).Returns(string.Empty);
            mockRequest.Setup(m => m.Cookies).Returns(new HttpCookieCollection());
            mockRequest.Setup(m => m.IsAuthenticated).Returns(isAuthenticated);

            // Request.
            var mockResponse = new Mock<HttpResponseBase>(MockBehavior.Loose);

            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(virtualPath => virtualPath);
            mockResponse.Setup(m => m.Cookies).Returns(new HttpCookieCollection());

            // HttpContext.
            var mockHttpContext = new Mock<HttpContextBase>(MockBehavior.Loose);

            mockHttpContext.Setup(m => m.Items).Returns(new Hashtable());
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockHttpContext.Object;
        }
    }
}