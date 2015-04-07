namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Builders;
    using Common.Framework;
    using FluentAssertions;
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


        [TestCase(VacancySearchSortType.Relevancy, false)]
        [TestCase(VacancySearchSortType.Distance, false)]
        [TestCase(VacancySearchSortType.ClosingDate, false)]
        [TestCase(VacancySearchSortType.RecentlyAdded, true)]
        public void PostedDate(VacancySearchSortType sortType, bool shouldShowPostedDate)
        {
            // Arrange.
            const int hits = 5;

            var vacancySearchViewModel = new ApprenticeshipSearchViewModelBuilder()
                .WithSortType(sortType)
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

                if (shouldShowPostedDate)
                {
                    var friendlyPostedDate = vacancy.PostedDate.ToFriendlyDaysAgo();

                    element.Should().NotBeNull();
                    element.InnerText.Should().Be(friendlyPostedDate);
                }
                else
                {
                    element.Should().BeNull();
                }
            }
        }
    }
}