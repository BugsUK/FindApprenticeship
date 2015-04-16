namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.TraineeshipSearch
{
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
    }
}