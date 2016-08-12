namespace SFA.Apprenticeships.Web.Candidate.UnitTests.ViewModels
{
    using Application.Interfaces.Vacancies;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ApprenticeshipSearchViewModelTests
    {
        [TestCase(ApprenticeshipLevel.Intermediate, "admin", 51.51713, -0.10619, "London (City of London, Greater London)", ApprenticeshipLocationType.NonNational, 1, SearchAction.Search, "All", ApprenticeshipSearchMode.Keyword, VacancySearchSortType.Relevancy, 5, null, null, "/apprenticeships?ApprenticeshipLevel=Intermediate&Hash=0&Keywords=admin&Latitude=51.51713&Longitude=-0.10619&Location=London%20%28City%20of%20London%2C%20Greater%20London%29&LocationType=NonNational&PageNumber=1&ResultsPerPage=5&SearchAction=Search&SearchField=All&SearchMode=Keyword&SortType=Relevancy&WithinDistance=5")]
        [TestCase(ApprenticeshipLevel.Intermediate, null, 51.51713, -0.10619, "London (City of London, Greater London)", ApprenticeshipLocationType.NonNational, 1, SearchAction.Search, "All", ApprenticeshipSearchMode.Category, VacancySearchSortType.Distance, 5, "ALB", new [] {"511", "526"}, "apprenticeships?Category=ALB&SubCategories=511&SubCategories=526&Location=London+%28City+of+London%2C+Greater+London%29&WithinDistance=5&ApprenticeshipLevel=Intermediate&SearchAction=Search&Latitude=51.51713&Longitude=-0.10619&Hash=549292876&SearchMode=Category&LocationType=NonNational&SortType=Distance&SearchAction=Search&resultsPerPage=5")]
        public void FromSearchUrlParseTests(
            ApprenticeshipLevel apprenticeshipLevel, 
            string keywords,
            double latitude,
            double longitude,
            string location,
            ApprenticeshipLocationType locationType,
            int pageNumber,
            SearchAction searchAction,
            string searchField,
            ApprenticeshipSearchMode searchMode,
            VacancySearchSortType searchSortType,
            int withinDistance,
            string category,
            string[] subCategories,
            string url)
        {
            var searchViewModel = ApprenticeshipSearchViewModel.FromSearchUrl(url);

            searchViewModel.Should().NotBeNull();
            searchViewModel.ApprenticeshipLevel.Should().Be(apprenticeshipLevel.ToString());
            searchViewModel.Keywords.Should().Be(keywords);
            searchViewModel.Latitude.Should().Be(latitude);
            searchViewModel.Longitude.Should().Be(longitude);
            searchViewModel.Location.Should().Be(location);
            searchViewModel.LocationType.Should().Be(locationType);
            searchViewModel.PageNumber.Should().Be(1);
            searchViewModel.ResultsPerPage.Should().Be(5);
            searchViewModel.SearchAction.Should().Be(searchAction);
            searchViewModel.SearchField.Should().Be(searchField);
            searchViewModel.SearchMode.Should().Be(searchMode);
            searchViewModel.SortType.Should().Be(searchSortType);
            searchViewModel.WithinDistance.Should().Be(withinDistance);
            searchViewModel.Category.Should().Be(category);
            searchViewModel.SubCategories.ShouldAllBeEquivalentTo(subCategories);
        }
    }
}
