namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class NationwideResultsTests : MediatorTestsBase
    {
        [Test]
        public void LocalAndNationWideResultsInALocalSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = VacancyLocationType.NonNational
                },
                TotalLocalHits = 2,
                TotalNationalHits = 3
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">2</b> apprenticeships in your selected area.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've also found <a id='nationwideLocationTypeLink' class='update-results'")
                    .And.Contain("3 apprenticeships with positions elsewhere in England</a>.");
        }

        [Test]
        public void LocalAndNationWideResultsInANationwideSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = VacancyLocationType.National
                },
                TotalLocalHits = 2,
                TotalNationalHits = 3
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">2</b> <a id='localLocationTypeLink' class='update-results' href=")
                    .And.Contain("apprenticeships in your selected area</a>.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Be("We've also found 3 apprenticeships with positions elsewhere in England.");
        }

        [Test]
        public void OneLocalResultAndSomeNationWideResultsInALocalSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = VacancyLocationType.NonNational
                },
                TotalLocalHits = 1,
                TotalNationalHits = 2
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">1</b> apprenticeship in your selected area.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've also found <a id='nationwideLocationTypeLink' class='update-results'")
                    .And.Contain("2 apprenticeships with positions elsewhere in England</a>.");
        }

        [Test]
        public void OneLocalResultAndSomeNationWideResultsInANationwideSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = VacancyLocationType.National
                },
                TotalLocalHits = 1,
                TotalNationalHits = 3
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">1</b> <a id='localLocationTypeLink' class='update-results' href=")
                    .And.Contain("apprenticeship in your selected area</a>.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Be("We've also found 3 apprenticeships with positions elsewhere in England.");
        }

        [Test]
        public void NoLocalResultAndSomeNationWideResultsInALocalSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = VacancyLocationType.National //Location type switches to national if no local results were found
                },
                TotalLocalHits = 0,
                TotalNationalHits = 2
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should().Be("There are currently no matching apprenticeships in your selected area.");

            view.GetElementbyId("national-results-message").InnerHtml.Should().Be("We've found 2 apprenticeships with positions elsewhere in England.");
        }

        [Test]
        public void NoLocalResultAndSomeNationWideResultsInANationwideSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = VacancyLocationType.National
                },
                TotalLocalHits = 0,
                TotalNationalHits = 3
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should().Be("There are currently no matching apprenticeships in your selected area.");

            view.GetElementbyId("national-results-message").InnerHtml.Should().Be("We've found 3 apprenticeships with positions elsewhere in England.");
        }

        [Test]
        public void SomeLocalResultsAndOneNationWideResultsInALocalSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = VacancyLocationType.NonNational
                },
                TotalLocalHits = 2,
                TotalNationalHits = 1
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">2</b> apprenticeships in your selected area.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've also found <a id='nationwideLocationTypeLink' class='update-results'")
                    .And.Contain("1 apprenticeship with positions elsewhere in England</a>.");
        }

        [Test]
        public void SomeLocalResultsAndOneNationWideResultInANationwideSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = VacancyLocationType.National
                },
                TotalLocalHits = 3,
                TotalNationalHits = 1
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">3</b> <a id='localLocationTypeLink' class='update-results' href=")
                    .And.Contain("apprenticeships in your selected area</a>.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Be("We've also found 1 apprenticeship with positions elsewhere in England.");
        }

        [Test]
        public void SomeLocalResultsAndNoNationWideResultsInALocalSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = VacancyLocationType.NonNational
                },
                TotalLocalHits = 2,
                TotalNationalHits = 0
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">2</b> apprenticeships in your selected area.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should().BeEmpty();
        }

        [Test]
        public void SomeLocalResultAndNoNationWideResultsInANationwideSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = VacancyLocationType.National
                },
                TotalLocalHits = 3,
                TotalNationalHits = 0
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">3</b> <a id='localLocationTypeLink' class='update-results' href=")
                    .And.Contain("apprenticeships in your selected area</a>.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should().BeEmpty();
        }
    }
}