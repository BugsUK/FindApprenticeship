namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApprenticeshipVacancyProvider
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class FindVacanciesTests
    {
        private const int PageSize = 10;

        private Mock<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>> _apprenticeshipSearchService;
        private Mock<ICandidateService> _candidateService; 
        private ApprenticeshipCandidateWebMappers _apprenticeshipMapper;
        private Mock<ILogService> _logService;
        private Mock<ICandidateVacancyService> _candidateVacancyService;

        [SetUp]
        public void Setup()
        {
            _apprenticeshipSearchService = new Mock<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>();
            _candidateService = new Mock<ICandidateService>();
            _apprenticeshipMapper = new ApprenticeshipCandidateWebMappers();
            _logService = new Mock<ILogService>();
            _candidateVacancyService = new Mock<ICandidateVacancyService>();
        }

        [TestCase]
        public void ShouldFindVacanciesFromCriteria()
        {
            SetupReturnOneHundredResultsOfType(ApprenticeshipLocationType.National);

            var search = GetASearchViewModelOfType(ApprenticeshipLocationType.National);

            var apprenticeshipVacancyProvider = GetApprenticeshipVacancyProvider();

            var test = apprenticeshipVacancyProvider.FindVacancies(search);

            test.Should().NotBeNull();
            test.Pages.Should().Be(10);
            test.NextPage.Should().Be(2);
            test.PrevPage.Should().Be(0);
            test.TotalNationalHits.Should().Be(100);
            test.TotalLocalHits.Should().Be(0);
            test.VacancySearch.Should().Be(search);
        }

        [Test]
        public void IfThereIsntNonNationalResultsButThereAreNationalResuts_ShouldReturnLocationTypeAsNational()
        {
            SetupReturnOneHundredResultsOfType(ApprenticeshipLocationType.National);

            var search = GetASearchViewModelOfType(ApprenticeshipLocationType.NonNational);

            var apprenticeshipVacancyProvider = GetApprenticeshipVacancyProvider();

            var vacancies = apprenticeshipVacancyProvider.FindVacancies(search);

            vacancies.VacancySearch.LocationType.Should().Be(ApprenticeshipLocationType.National);
            vacancies.VacancySearch.SortType.Should().NotBe(VacancySearchSortType.Distance);
            vacancies.VacancySearch.SortType.Should().Be(VacancySearchSortType.ClosingDate);
        }

        [Test]
        public void
            IfItsANationalSearchButThereIsntNationalResuls_TheNonNationalResultsAreReturned()
        {
            SetupReturnOneHundredResultsOfType(ApprenticeshipLocationType.NonNational);

            var search = GetASearchViewModelOfType(ApprenticeshipLocationType.National);

            var apprenticeshipVacancyProvider = GetApprenticeshipVacancyProvider();

            var vacancies = apprenticeshipVacancyProvider.FindVacancies(search);

            vacancies.Vacancies.Should().HaveCount(1);
            vacancies.Vacancies.First().VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        private static ApprenticeshipSearchViewModel GetASearchViewModelOfType(ApprenticeshipLocationType locationType)
        {
            var search = new ApprenticeshipSearchViewModel
            {
                Location = "Test",
                Longitude = 0d,
                Latitude = 0d,
                PageNumber = 1,
                WithinDistance = 2,
                ResultsPerPage = PageSize,
                LocationType = locationType
            };

            if (locationType == ApprenticeshipLocationType.NonNational)
                search.SortType = VacancySearchSortType.Distance;

            return search;
        }

        private void SetupReturnOneHundredResultsOfType(ApprenticeshipLocationType locationType)
        {
            _apprenticeshipSearchService.Setup(
                x => x.Search(It.Is<ApprenticeshipSearchParameters>(asp => asp.VacancyLocationType == locationType))).Returns<ApprenticeshipSearchParameters>(asp => new
                SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>(100, new List<ApprenticeshipSearchResponse>
                {
                    new ApprenticeshipSearchResponse
                    {
                        VacancyLocationType = locationType
                    }
                }, new List<AggregationResult>(0), asp));

            _apprenticeshipSearchService.Setup(
                x => x.Search(It.Is<ApprenticeshipSearchParameters>(asp => asp.VacancyLocationType != locationType))).Returns<ApprenticeshipSearchParameters>(asp => new
                SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>(0, new List<ApprenticeshipSearchResponse>
                {
                    new ApprenticeshipSearchResponse
                    {
                        VacancyLocationType = locationType
                    }
                }, new List<AggregationResult>(0), asp));
        }

        private ApprenticeshipVacancyProvider GetApprenticeshipVacancyProvider()
        {
            var searchProvider = new ApprenticeshipVacancyProvider(
                _apprenticeshipSearchService.Object,
                _candidateService.Object,
                _apprenticeshipMapper,
                _logService.Object,
                _candidateVacancyService.Object);
            return searchProvider;
        }
    }
}