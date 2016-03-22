namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using Common.Providers;
    using Configuration;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Vacancy;

    [TestFixture]
    public class GetPendingQAVacanciesOverviewTests
    {
        private const int ExpectedSubmittedTodayCount = 5;
        private const int ExpectedSubmittedYesterdayCount = 4;
        private const int ExpectedSubmittedMoreThan48HoursCount = 6;
        private const int ExpectedResubmittedCount = 3;

        private Mock<IVacancyPostingService> _vacancyPostingService;
        private Mock<IProviderService> _providerService;
        private Mock<IConfigurationService> _configurationService;
        private Mock<IDateTimeService> _dateTimeService;
        private IVacancyQAProvider _provider;

        private List<VacancySummary> _vacanciesSubmittedToday;

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
                .CreateMany(ExpectedSubmittedTodayCount).ToList();

            var vacanciesSubmittedYesterdayUpperBoundary = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, utcNow.Date.AddSeconds(-1))
                .With(v => v.SubmissionCount, 1)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .CreateMany(ExpectedSubmittedYesterdayCount / 2).ToList();
            var vacanciesSubmittedYesterdayLowerBoundary = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, utcNow.Date.AddDays(-1))
                .With(v => v.SubmissionCount, 1)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .CreateMany(ExpectedSubmittedYesterdayCount / 2).ToList();

            var submittedMoreThan48HoursDate = utcNow.AddHours(-48).AddSeconds(-1);
            var vacanciesSubmittedMoreThan48Hours = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, submittedMoreThan48HoursDate)
                .With(v => v.SubmissionCount, 1)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .CreateMany(ExpectedSubmittedMoreThan48HoursCount).ToList();

            var resubmittedDate = utcNow.Date.AddDays(-1).AddSeconds(-1);
            var vacanciesResubmittedHours = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, resubmittedDate)
                .With(v => v.SubmissionCount, 3)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .CreateMany(ExpectedResubmittedCount).ToList();

            var vacancies = _vacanciesSubmittedToday;
            vacancies.AddRange(vacanciesSubmittedYesterdayUpperBoundary);
            vacancies.AddRange(vacanciesSubmittedYesterdayLowerBoundary);
            vacancies.AddRange(vacanciesSubmittedMoreThan48Hours);
            vacancies.AddRange(vacanciesResubmittedHours);

            _vacancyPostingService = new Mock<IVacancyPostingService>();
            _vacancyPostingService.Setup(p => p.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA)).Returns(vacancies);

            _providerService = new Mock<IProviderService>();
            _providerService.Setup(s => s.GetProvider(It.IsAny<string>())).Returns(new Fixture().Create<Provider>());
            _providerService.Setup(s => s.GetProviderViaOwnerParty(It.IsAny<int>())).Returns(new Fixture().Create<Provider>());

            _provider = new VacancyProviderBuilder().With(_vacancyPostingService).With(_providerService).With(_dateTimeService).Build();
        }

        [TestCase(DashboardVacancySummaryFilterTypes.All, ExpectedSubmittedTodayCount + ExpectedSubmittedYesterdayCount + ExpectedSubmittedMoreThan48HoursCount + ExpectedResubmittedCount)]
        [TestCase(DashboardVacancySummaryFilterTypes.SubmittedToday, ExpectedSubmittedTodayCount)]
        [TestCase(DashboardVacancySummaryFilterTypes.SubmittedYesterday, ExpectedSubmittedYesterdayCount)]
        [TestCase(DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours, ExpectedSubmittedMoreThan48HoursCount + ExpectedResubmittedCount)]
        [TestCase(DashboardVacancySummaryFilterTypes.Resubmitted, ExpectedResubmittedCount)]
        public void ReturnsCorrectSearchViewModel(DashboardVacancySummaryFilterTypes filterType, int expectedCount)
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = filterType
            };

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.SearchViewModel.FilterType.Should().Be(filterType);
            vacancySummariesViewModel.SearchViewModel.Should().Be(searchViewModel);
        }

        [TestCase(DashboardVacancySummaryFilterTypes.All, ExpectedSubmittedTodayCount + ExpectedSubmittedYesterdayCount + ExpectedSubmittedMoreThan48HoursCount + ExpectedResubmittedCount)]
        [TestCase(DashboardVacancySummaryFilterTypes.SubmittedToday, ExpectedSubmittedTodayCount)]
        [TestCase(DashboardVacancySummaryFilterTypes.SubmittedYesterday, ExpectedSubmittedYesterdayCount)]
        [TestCase(DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours, ExpectedSubmittedMoreThan48HoursCount + ExpectedResubmittedCount)]
        [TestCase(DashboardVacancySummaryFilterTypes.Resubmitted, ExpectedResubmittedCount)]
        public void BasicCountTests(DashboardVacancySummaryFilterTypes filterType, int expectedCount)
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = filterType
            };

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.Vacancies.Should().NotBeNullOrEmpty();
            vacancySummariesViewModel.Vacancies.Count.Should().Be(expectedCount);
        }

        [Test]
        public void GetAll_Counts()
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = DashboardVacancySummaryFilterTypes.All
            };

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.SubmittedTodayCount.Should().Be(ExpectedSubmittedTodayCount);
            vacancySummariesViewModel.SubmittedYesterdayCount.Should().Be(ExpectedSubmittedYesterdayCount);
            vacancySummariesViewModel.SubmittedMoreThan48HoursCount.Should().Be(ExpectedSubmittedMoreThan48HoursCount + ExpectedResubmittedCount);
            vacancySummariesViewModel.ResubmittedCount.Should().Be(ExpectedResubmittedCount);
        }

        [Test]
        public void GetAll_OrderedByDateSubmitted()
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = DashboardVacancySummaryFilterTypes.All
            };

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.Vacancies.Should().BeInAscendingOrder(v => v.DateSubmitted);
        }

        [Test]
        public void GetSubmittedToday_OrderedByDateFirstSubmitted()
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = DashboardVacancySummaryFilterTypes.SubmittedToday
            };

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.Vacancies.Should().BeInAscendingOrder(v => v.DateFirstSubmitted);
        }

        [Test]
        public void GetSubmittedYesterday_OrderedByDateFirstSubmitted()
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = DashboardVacancySummaryFilterTypes.SubmittedYesterday
            };

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.Vacancies.Should().BeInAscendingOrder(v => v.DateFirstSubmitted);
        }

        [Test]
        public void GetSubmittedMoreThan48Hours_OrderedByDateSubmitted()
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours
            };

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.Vacancies.Should().BeInAscendingOrder(v => v.DateSubmitted);
        }

        [Test]
        public void GetResubmitted_OrderedByDateFirstSubmitted()
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                FilterType = DashboardVacancySummaryFilterTypes.Resubmitted
            };

            //Act
            var vacancySummariesViewModel = _provider.GetPendingQAVacanciesOverview(searchViewModel);

            //Assert
            vacancySummariesViewModel.Vacancies.Should().BeInAscendingOrder(v => v.DateFirstSubmitted);
        }

        [Test]
        public void NoVacanciesRedirectToMetrics()
        {
            //Arrange
            var searchViewModel = new DashboardVacancySummariesSearchViewModel
            {
                Mode = DashboardVacancySummariesMode.Review
            };
            _vacancyPostingService.Reset();
            _vacancyPostingService.Setup(p => p.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA)).Returns(new List<VacancySummary>());

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
                FilterType = DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours,
                Mode = DashboardVacancySummariesMode.Review
            };
            _vacancyPostingService.Reset();
            _vacancyPostingService.Setup(p => p.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA)).Returns(_vacanciesSubmittedToday);

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

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(new List<VacancySummary>());

            providerService.Setup(ps => ps.GetProviderViaOwnerParty(It.IsAny<int>())).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .Build();

            //Act
            vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel());

            // Assert
            vacancyPostingService.Verify(vps => vps.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA), Times.Once);
        }

        [Test]
        public void GetPendingQAVacanciesOverviewShouldReturnVacanciesSubmittedToday()
        {
            // Arrange
            var today = new DateTime(2016, 3, 16, 12, 0, 0);
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(dts => dts.UtcNow).Returns(today);
            const int anInt = 1;
            const string username = "userName";

            var apprenticeshipVacancies = new List<VacancySummary>
            {
                new VacancySummary
                {
                    ClosingDate = today,
                    DateSubmitted = today,
                    OwnerPartyId = anInt,
                    VacancyReferenceNumber = anInt,
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = username,
                    DateStartedToQA = null
                }
            };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProviderViaOwnerParty(It.IsAny<int>())).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(dateTimeService)
                    .Build();

            //Act
            var vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedToday
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(1);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedYesterday
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.All
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(1);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.Resubmitted
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);
        }

        [Test]
        public void GetPendingQAVacanciesOverviewShouldReturnVacanciesSubmittedYesterday()
        {
            // Arrange
            var today = new DateTime(2016, 3, 16, 12, 0, 0);
            var yesterday = new DateTime(2016, 3, 15, 12, 0, 0);
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(dts => dts.UtcNow).Returns(today);
            const int anInt = 1;
            const string username = "userName";

            var apprenticeshipVacancies = new List<VacancySummary>
            {
                new VacancySummary
                {
                    ClosingDate = yesterday,
                    DateSubmitted = yesterday,
                    OwnerPartyId = anInt,
                    VacancyReferenceNumber = anInt,
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = username,
                    DateStartedToQA = null
                }
            };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProviderViaOwnerParty(It.IsAny<int>())).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(dateTimeService)
                    .Build();

            //Act
            var vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedToday
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedYesterday
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(1);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.All
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(1);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.Resubmitted
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);
        }

        [Test]
        public void GetPendingQAVacanciesOverviewShouldReturnVacanciesSubmittedMoreThan48HoursAgo()
        {
            // Arrange
            var today = new DateTime(2016, 3, 16, 12, 0, 0);
            var fourtyEightHoursAndOneMinuteAgo = today.AddMinutes(-(48 * 60 + 1));
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(dts => dts.UtcNow).Returns(today);
            const int anInt = 1;
            const string username = "userName";

            var apprenticeshipVacancies = new List<VacancySummary>
            {
                new VacancySummary
                {
                    ClosingDate = fourtyEightHoursAndOneMinuteAgo,
                    DateSubmitted = fourtyEightHoursAndOneMinuteAgo,
                    OwnerPartyId = anInt,
                    VacancyReferenceNumber = anInt,
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = username,
                    DateStartedToQA = null
                }
            };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProviderViaOwnerParty(It.IsAny<int>())).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(dateTimeService)
                    .Build();

            //Act
            var vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedToday
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedYesterday
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(1);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.All
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(1);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.Resubmitted
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);
        }


        [Test]
        public void GetPendingQAVacanciesOverviewShouldReturnVacanciesResubmitted()
        {
            // Arrange
            var today = new DateTime(2016, 3, 16, 12, 0, 0);
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(dts => dts.UtcNow).Returns(today);
            const int anInt = 1;
            const string username = "userName";
            const int submissionCount = 2;

            var apprenticeshipVacancies = new List<VacancySummary>
            {
                new VacancySummary
                {
                    ClosingDate = today,
                    DateSubmitted = today,
                    OwnerPartyId = anInt,
                    VacancyReferenceNumber = anInt,
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = username,
                    DateStartedToQA = null,
                    SubmissionCount = submissionCount
                }
            };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProviderViaOwnerParty(It.IsAny<int>())).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(dateTimeService)
                    .Build();

            //Act
            var vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedToday
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(1);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedYesterday
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(0);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.All
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(1);

            //Act
            vacancies =
                vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                {
                    FilterType = DashboardVacancySummaryFilterTypes.Resubmitted
                }).Vacancies;

            //Assert
            vacancies.Should().HaveCount(1);
        }
    }
}