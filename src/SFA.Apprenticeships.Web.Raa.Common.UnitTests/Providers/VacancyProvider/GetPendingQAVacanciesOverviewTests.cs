namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using Common.Providers;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Application.Interfaces;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;
    using ViewModels.Vacancy;

    [TestFixture]
    [Parallelizable]
    public class GetPendingQAVacanciesOverviewTests
    {
        private const int ExpectedSubmittedTodayCount = 5;
        private const int ExpectedSubmittedYesterdayCount = 4;
        private const int ExpectedSubmittedMoreThan48HoursCount = 6;
        private const int ExpectedResubmittedCount = 3;
        private const int ProviderId = 2;
        private Mock<IVacancyPostingService> _vacancyPostingService;
        private Mock<IProviderService> _providerService;
        private Mock<IDateTimeService> _dateTimeService;
        private List<VacancySummary> _vacanciesSubmittedToday;
        private IVacancyQAProvider _provider;
        private Mock<IVacancySummaryRepository> _respository;

        [SetUp]
        public void Setup()
        {
            var utcNow = new DateTime(2016, 01, 29, 11, 39, 34, DateTimeKind.Utc);
            _dateTimeService = new Mock<IDateTimeService>();
            _dateTimeService.Setup(s => s.UtcNow).Returns(utcNow);

            var submittedTodayDate = utcNow.Date;
            _vacanciesSubmittedToday = new Fixture().Build<VacancySummary>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, submittedTodayDate)
                .With(v => v.SubmissionCount, 1)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .With(v => v.ContractOwnerId, ProviderId)
                .CreateMany(ExpectedSubmittedTodayCount).ToList();

            var vacanciesSubmittedYesterdayUpperBoundary = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, utcNow.Date.AddSeconds(-1))
                .With(v => v.SubmissionCount, 1)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .With(v => v.ContractOwnerId, ProviderId)
                .CreateMany(ExpectedSubmittedYesterdayCount / 2).ToList();
            var vacanciesSubmittedYesterdayLowerBoundary = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, utcNow.Date.AddDays(-1))
                .With(v => v.SubmissionCount, 1)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .With(v => v.ContractOwnerId, ProviderId)
                .CreateMany(ExpectedSubmittedYesterdayCount / 2).ToList();

            var submittedMoreThan48HoursDate = utcNow.AddHours(-48).AddSeconds(-1);
            var vacanciesSubmittedMoreThan48Hours = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, submittedMoreThan48HoursDate)
                .With(v => v.SubmissionCount, 1)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .With(v => v.ContractOwnerId, ProviderId)
                .CreateMany(ExpectedSubmittedMoreThan48HoursCount).ToList();

            var resubmittedDate = utcNow.Date.AddDays(-1).AddSeconds(-1);
            var vacanciesResubmittedHours = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, resubmittedDate)
                .With(v => v.SubmissionCount, 3)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .With(v => v.ContractOwnerId, ProviderId)
                .CreateMany(ExpectedResubmittedCount).ToList();

            var vacancies = _vacanciesSubmittedToday;
            vacancies.AddRange(vacanciesSubmittedYesterdayUpperBoundary);
            vacancies.AddRange(vacanciesSubmittedYesterdayLowerBoundary);
            vacancies.AddRange(vacanciesSubmittedMoreThan48Hours);
            vacancies.AddRange(vacanciesResubmittedHours);

            var metrics = new List<RegionalTeamMetrics>()
            {
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.EastAnglia },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.EastMidlands },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.North },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.NorthWest },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.SouthEast },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.SouthWest },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.WestMidlands },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.YorkshireAndHumberside },
            };

            int total;

            _vacancyPostingService = new Mock<IVacancyPostingService>();
            _vacancyPostingService.Setup(p => p.GetWithStatus(It.IsAny<VacancySummaryByStatusQuery>(), out total)).Returns(vacancies);
            _vacancyPostingService.Setup(p => p.GetRegionalTeamsMetrics(It.IsAny<VacancySummaryByStatusQuery>())).Returns(metrics);

            _providerService = new Mock<IProviderService>();
            _providerService.Setup(s => s.GetProvider(It.IsAny<string>(), true)).Returns(new Fixture().Create<Provider>());
            _providerService.Setup(s => s.GetProviders(It.IsAny<IEnumerable<int>>())).Returns(new List<Provider>{new Fixture().Build<Provider>().With(p => p.ProviderId, ProviderId).Create()});

            _provider = new VacancyProviderBuilder().With(_vacancyPostingService).With(_providerService).With(_dateTimeService).Build();
        }

        [TestCase()]
        public void ReturnsCorrectSearchViewModel()
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = VacanciesSummaryFilterTypes.All
            };

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.SearchViewModel.FilterType.Should().Be(VacanciesSummaryFilterTypes.All);
            vacancySummariesViewModel.SearchViewModel.Should().Be(searchViewModel);
        }

        [TestCase()]
        public void BasicCountTests()
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = VacanciesSummaryFilterTypes.All
            };

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.Vacancies.Should().NotBeNullOrEmpty();
            vacancySummariesViewModel.Vacancies.Count.Should().Be(18);
        }

        [Test]
        public void NoVacanciesRedirectToMetrics()
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                Mode = DashboardVacancySummariesMode.Review
            };

            int total;

            _vacancyPostingService.Reset();
            _vacancyPostingService.Setup(p => p.GetWithStatus(It.IsAny<VacancySummaryByStatusQuery>(), out total)).Returns(new List<VacancySummary>());

            var metrics = new List<RegionalTeamMetrics>()
            {
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.EastAnglia },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.EastMidlands },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.North },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.NorthWest },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.SouthEast },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.SouthWest },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.WestMidlands },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.YorkshireAndHumberside },
            };

            _vacancyPostingService.Setup(p => p.GetRegionalTeamsMetrics(It.IsAny<VacancySummaryByStatusQuery>())).Returns(metrics);

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.SearchViewModel.Mode.Should().Be(DashboardVacancySummariesMode.Metrics);
        }

        [Test]
        public void NoVacanciesForSelectionAllowReview()
        {
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = VacanciesSummaryFilterTypes.SubmittedMoreThan48Hours,
                Mode = DashboardVacancySummariesMode.Review
            };

            int total;

            _vacancyPostingService.Reset();
            _vacancyPostingService.Setup(p => p.GetWithStatus(It.IsAny<VacancySummaryByStatusQuery>(), out total)).Returns(_vacanciesSubmittedToday);

            var metrics = new List<RegionalTeamMetrics>()
            {
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.EastAnglia, TotalCount = 1 },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.EastMidlands },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.North },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.NorthWest },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.SouthEast },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.SouthWest },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.WestMidlands },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.YorkshireAndHumberside },
            };

            _vacancyPostingService.Setup(p => p.GetRegionalTeamsMetrics(It.IsAny<VacancySummaryByStatusQuery>())).Returns(metrics);

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.SearchViewModel.Mode.Should().Be(DashboardVacancySummariesMode.Review);
        }

        [Test]
        public void GetPendingQAVacanciesOverviewShouldGetSubmittedAndReservedForQAVacanciesFromVacancyPostingService()
        {
            // Arrange
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();

            int total;

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(It.IsAny<VacancySummaryByStatusQuery>(), out total))
                .Returns(new List<VacancySummary>());

            var metrics = new List<RegionalTeamMetrics>()
            {
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.EastAnglia },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.EastMidlands },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.North },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.NorthWest },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.SouthEast },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.SouthWest },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.WestMidlands },
                new RegionalTeamMetrics() { RegionalTeam = RegionalTeam.YorkshireAndHumberside },
            };

            vacancyPostingService.Setup(p => p.GetRegionalTeamsMetrics(It.IsAny<VacancySummaryByStatusQuery>())).Returns(metrics);
            
            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .Build();

            //Act
            vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel());

            // Assert
            vacancyPostingService.Verify(vps => vps.GetWithStatus(It.IsAny<VacancySummaryByStatusQuery>(), out total), Times.Once);
            vacancyPostingService.Verify(vps => vps.GetRegionalTeamsMetrics(It.IsAny<VacancySummaryByStatusQuery>()), Times.Once);
        }
    }
}