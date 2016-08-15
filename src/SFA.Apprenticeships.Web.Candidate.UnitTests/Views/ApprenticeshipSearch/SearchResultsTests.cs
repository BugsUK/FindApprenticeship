namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Globalization;
    using System.Linq;
    using Builders;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using NUnit.Framework;
    using RazorGenerator.Testing;

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

        [TestCase(null)]
        [TestCase(false)]
        [TestCase(true)]
        public void ShowHideSaveVacancyLinks(bool? isCandidateActivated)
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
            var view = new SearchResultsViewBuilder().With(viewModel).Build();

            view.ViewBag.IsCandidateActivated = isCandidateActivated;

            var result = view.RenderAsHtml(viewModel);

            // Assert.
            viewModel.Vacancies.Count().Should().Be(hits);

            foreach (var vacancy in viewModel.Vacancies)
            {
                var idFormats = new[] { "save-vacancy-link-{0}", "resume-link-{0}", "applied-label-{0}" };

                foreach (var idFormat in idFormats)
                {
                    var id = string.Format(idFormat, vacancy.Id);

                    var element = result.GetElementbyId(id);

                    if (isCandidateActivated.HasValue && isCandidateActivated.Value)
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


        [Test]
        public void ShowIsPositiveAboutDisabled()
        {
            // Arrange.
            const int hits = 5;

            var viewModel = new ApprenticeshipSearchResponseViewModelBuilder()
                .WithIsPositiveAboutDisability(true)
                .WithTotalLocalHits(hits)
                .Build();

            // Act.
            var view = new SearchResultsViewBuilder().With(viewModel).Render();

            //Assert.

            foreach (var vacancy in viewModel.Vacancies)
            {
                var disabledLink = view.GetElementbyId(string.Format("positive-about-disabled-{0}", vacancy.Id));
                disabledLink.Should().NotBeNull();
                disabledLink.GetAttributeValue("href", null).Should().Be("https://www.gov.uk/looking-for-work-if-disabled");
                disabledLink.GetAttributeValue("target", null).Should().Be("_blank");
            }
        }

        [Test]
        public void HideIsPositiveAboutDisabled()
        {
            // Arrange.
            const int hits = 5;

            var viewModel = new ApprenticeshipSearchResponseViewModelBuilder()
                .WithIsPositiveAboutDisability(false)
                .WithTotalLocalHits(hits)
                .Build();

            // Act.
            var view = new SearchResultsViewBuilder().With(viewModel).Render();

            //Assert.

            foreach (var vacancy in viewModel.Vacancies)
            {
                var disabledLink = view.GetElementbyId(string.Format("positive-about-disabled-{0}", vacancy.Id));
                disabledLink.Should().BeNull();
            }
        }

        [TestCase(5)]
        [TestCase(1)]
        public void NumberOfPositions(int numberOfPositions)
        {
            // Arrange.
            const int hits = 5;

            var vacancySearchViewModel = new ApprenticeshipSearchViewModelBuilder()
                .Build();

            var viewModel = new ApprenticeshipSearchResponseViewModelBuilder()
                .WithVacancySearch(vacancySearchViewModel)
                .WithTotalLocalHits(hits)
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

                element.Should().NotBeNull();
                element.InnerText.Should().Contain(numberOfPositions.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}