namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System;
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
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

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Keyword).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();

            view.GetElementbyId("saved-searches-tab-control").Should().BeNull();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();

            //Shares parent with loc-within here.
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" active").Should().BeTrue();

            view.GetElementbyId("browse-button").Should().NotBeNull();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();

            view.GetElementbyId("reset-search-options-link").Should().NotBeNull();
            view.GetElementbyId("reset-search-options-link").Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("reset-search-options-link").Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
        }

        [Test]
        public void SearchMode_Category_BasicVisibilityTest()
        {
            var index = new Index();

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Category).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();

            view.GetElementbyId("saved-searches-tab-control").Should().BeNull();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeTrue();

            //Shares parent with loc-within here.
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();

            view.GetElementbyId("browse-button").Should().NotBeNull();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" active").Should().BeTrue();

            view.GetElementbyId("reset-search-options-link").Should().NotBeNull();
            view.GetElementbyId("reset-search-options-link").Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("reset-search-options-link").Attributes["class"].Value.Contains(" tab2").Should().BeTrue();
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

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Category).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Keywords").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("Location").ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            view.GetElementbyId("saved-searches-tab-control").Should().BeNull();

            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("loc-within").ParentNode.ParentNode.Attributes["class"].Value.Contains(" tab2").Should().BeFalse();

            //Shares parent with loc-within here.
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();

            view.GetElementbyId("search-button").Should().NotBeNull();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" tab1").Should().BeTrue();
            view.GetElementbyId("search-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();

            view.GetElementbyId("browse-button").Should().NotBeNull();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" tab2").Should().BeFalse();
            view.GetElementbyId("browse-button").Attributes["class"].Value.Contains(" active").Should().BeFalse();

            view.GetElementbyId("reset-search-options-link").Should().NotBeNull();
            view.GetElementbyId("reset-search-options-link").Attributes["class"].Value.Contains(" active").Should().BeTrue();
            view.GetElementbyId("reset-search-options-link").Attributes["class"].Value.Contains(" tab2").Should().BeFalse();
        }

        [Test]
        public void SearchMode_SavedSearches_AnonymousUserTest()
        {
            var index = new Index();
            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.SavedSearches).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("saved-searches-tab-control").Should().BeNull();
        }

        [TestCase(1)]
        [TestCase(5)]
        public void SearchMode_SavedSearches_CandidateWithSavedSearchesVisibilityTest(int savedSearchCount)
        {
            var index = new Index();
            var candidateId = Guid.NewGuid();
            var mockViewModel = new Fixture().Build<SavedSearchViewModel>().CreateMany(savedSearchCount);

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var searchViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            var link = view.GetElementbyId("saved-searches-tab-control");

            link.Should().NotBeNull();
            link.Attributes["class"].Value.Should().Contain(" active");

            var button = view.GetElementbyId("run-saved-search-button");

            button.Attributes["class"].Value.Should().Contain(" tab3");
            button.Attributes["class"].Value.Should().Contain(" active");

            view.GetElementbyId("reset-search-options-link").Should().NotBeNull();
            view.GetElementbyId("reset-search-options-link").Attributes["class"].Value.Contains(" active").Should().BeFalse();
            view.GetElementbyId("reset-search-options-link").Attributes["class"].Value.Contains(" tab3").Should().BeFalse();
        }

        [Test]
        public void SearchMode_SavedSearches_CandidateWithNoSavedSearchesVisibilityTest()
        {
            var index = new Index();
            var candidateId = Guid.NewGuid();
            var mockViewModel = new SavedSearchViewModel[] { };

            CandidateServiceProvider.Setup(mock => mock.GetSavedSearches(candidateId)).Returns(mockViewModel);

            var searchViewModel = Mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches).ViewModel;
            var view = index.RenderAsHtml(searchViewModel);

            view.GetElementbyId("saved-searches-tab-control").Should().BeNull();
        }
    }
}