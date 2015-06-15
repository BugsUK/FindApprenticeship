namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.TraineeshipSearch
{
    using System.Globalization;
    using System.Linq;
    using Builders;
    using Common.Framework;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SearchResultsTests
    {
        [Test]
        public void PostedDate()
        {
            // Arrange.
            const int hits = 5;

            var vacancySearchViewModel = new TraineeshipSearchViewModelBuilder()
                .Build();

            var viewModel = new TraineeshipSearchResponseViewModelBuilder()
                .WithVacancySearch(vacancySearchViewModel)
                .WithTotalHits(hits)
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

        [TestCase(5, true)]
        [TestCase(1, false)]
        public void NumberOfPositions(int numberOfPositions, bool shouldShowNumberOfPositions)
        {
            // Arrange.
            const int hits = 5;

            var vacancySearchViewModel = new TraineeshipSearchViewModelBuilder()
                .Build();

            var viewModel = new TraineeshipSearchResponseViewModelBuilder()
                .WithVacancySearch(vacancySearchViewModel)
                .WithTotalHits(hits)
                .WithNumberOfPositions(numberOfPositions)
                .Build();

            // Act.
            var result = new SearchResultsViewBuilder().With(viewModel).Render();

            // Assert.
            viewModel.Vacancies.Count().Should().Be(hits);

            foreach (var vacancy in viewModel.Vacancies)
            {
                var id = string.Format("number-of-positions-{0}", vacancy.Id);

                var element = result.GetElementbyId(id);

                if (shouldShowNumberOfPositions)
                {
                    element.Should().NotBeNull();
                    element.InnerText.Should().Contain(numberOfPositions.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    element.Should().BeNull();                    
                }
            }
        }
    }
}