namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using Candidate.Views.ApprenticeshipSearch;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class ResultsSearchUpdateTests : MediatorTestsBase
    {
        [Test]
        public void SearchModeKeywordBasicVisibilityTest()
        {
            var searchUpdate = new searchUpdate();

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Keyword, false).ViewModel;
            var searchResultsViewModel = Mediator.Results(null, searchViewModel).ViewModel;
            var view = searchUpdate.RenderAsHtml(searchResultsViewModel.VacancySearch);

            view.GetElementbyId("validation-summary").Should().BeNull();
            view.GetElementbyId("Keywords").Should().NotBeNull();
            view.GetElementbyId("Location").Should().NotBeNull();            
            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();
            view.GetElementbyId("search-button").Should().NotBeNull();

            var createNewSearchLink = view.GetElementbyId("start-again-link");

            createNewSearchLink.Should().NotBeNull();
            createNewSearchLink.OuterHtml.Should().Contain("SearchMode=Keyword");
        }

        [Test]
        public void SearchModeCategoryBasicVisibilityTest()
        {
            var searchUpdate = new searchUpdate();

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Category, false).ViewModel;
            var searchResultsViewModel = Mediator.Results(null, searchViewModel).ViewModel;
            var view = searchUpdate.RenderAsHtml(searchResultsViewModel.VacancySearch);

            view.GetElementbyId("validation-summary").Should().BeNull();
            view.GetElementbyId("Keywords").Should().BeNull();
            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("loc-within").Should().NotBeNull();            
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();
            view.GetElementbyId("search-button").Should().NotBeNull();

            var createNewSearchLink = view.GetElementbyId("start-again-link");

            createNewSearchLink.Should().NotBeNull();
            createNewSearchLink.OuterHtml.Should().Contain("SearchMode=Category");
        }

        /// <summary>
        /// Form fields should no longer be visible and a message should be present
        /// to tell users loading of categories prevents filtering by them
        /// </summary>
        [Test]
        public void SearchModeCategoryNullCategoriesVisibilityTest()
        {
            ReferenceDataService.Setup(rds => rds.GetCategories());
            var searchUpdate = new searchUpdate();

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Category, false).ViewModel;
            var searchResultsViewModel = Mediator.Results(null, searchViewModel).ViewModel;
            var view = searchUpdate.RenderAsHtml(searchResultsViewModel.VacancySearch);

            view.GetElementbyId("Keywords").Should().BeNull();
            view.GetElementbyId("Location").Should().NotBeNull();
            view.GetElementbyId("loc-within").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-level").Should().NotBeNull();
            view.GetElementbyId("search-button").Should().NotBeNull();

            var createNewSearchLink = view.GetElementbyId("start-again-link");

            createNewSearchLink.Should().NotBeNull();
            createNewSearchLink.OuterHtml.Should().Contain("SearchMode=Category");
        }
    }
}