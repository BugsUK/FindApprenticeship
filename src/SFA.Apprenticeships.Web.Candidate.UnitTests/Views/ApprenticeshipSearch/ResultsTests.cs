namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using Builders;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class ResultsTests : MediatorTestsBase
    {
        [Test]
        public void Category_NoResults()
        {
            var results = new searchResults();

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Category, false).ViewModel;
            searchViewModel.Category = "1";
            var searchResultsViewModel = Mediator.Results(null, searchViewModel).ViewModel;
            var view = results.RenderAsHtml(searchResultsViewModel);

            view.GetElementbyId("search-no-results").Should().NotBeNull();
            view.GetElementbyId("search-no-results-category").Should().NotBeNull();
            view.GetElementbyId("search-no-results-sub-category").Should().BeNull();
        }

        [Test]
        public void SubCategory_NoResults()
        {
            var results = new searchResults();

            var searchViewModel = Mediator.Index(null, ApprenticeshipSearchMode.Category, false).ViewModel;
            searchViewModel.Category = "1";
            searchViewModel.SubCategories = new []{"1_1"};
            var searchResultsViewModel = Mediator.Results(null, searchViewModel).ViewModel;
            var view = results.RenderAsHtml(searchResultsViewModel);

            view.GetElementbyId("search-no-results").Should().NotBeNull();
            view.GetElementbyId("search-no-results-category").Should().BeNull();
            view.GetElementbyId("search-no-results-sub-category").Should().NotBeNull();
        }

        [Test]
        public void NoResults()
        {
            var results = new searchResults();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel()
            });

            view.GetElementbyId("sort-results").Should().BeNull();
            view.GetElementbyId("search-no-results-title").Should().NotBeNull();
        }

        [Test]
        public void ShowApprenticeshipLevelAdvice()
        {
            var results = new searchResults();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    ApprenticeshipLevel = "Intermediate"
                },
            });

            view.GetElementbyId("search-no-results-apprenticeship-levels").Should().NotBeNull();
        }

        [Test]
        public void HideApprenticeshipLevelAdvice()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    ApprenticeshipLevel = "All"
                },
            });

            view.GetElementbyId("search-no-results-apprenticeship-levels").Should().BeNull();
        }

        [Test]
        public void ShowNoResultsReferenceNumber()
        {
            var results = new searchResults();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    SearchMode = ApprenticeshipSearchMode.Keyword,
                    Keywords = "VAC000514705"
                },
            });

            view.GetElementbyId("search-no-results-reference-number").Should().NotBeNull();
            view.GetElementbyId("search-no-results-keywords").Should().BeNull();
        }

        [Test]
        public void ShowNoResultsKeywords()
        {
            var results = new searchResults();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    SearchMode = ApprenticeshipSearchMode.Keyword,
                    Keywords = "Chef"
                },
            });

            view.GetElementbyId("search-no-results-reference-number").Should().BeNull();
            view.GetElementbyId("search-no-results-keywords").Should().NotBeNull();
        }

        [Test]
        public void SaveSearchAvailableNotLoggedIn()
        {
            var vacancySearchViewModel = new ApprenticeshipSearchViewModelBuilder().WithLocation("CV1").Build();
            var searchResponseViewModel = new ApprenticeshipSearchResponseViewModelBuilder()
                .WithVacancySearch(vacancySearchViewModel)
                .Build();

            var result = new ResultsViewBuilder()
                .With(searchResponseViewModel)
                .Render();

            var receiveSaveSearchAlert = result.GetElementbyId("receiveSaveSearchAlert");

            receiveSaveSearchAlert.Should().NotBeNull();
        }
    }
}