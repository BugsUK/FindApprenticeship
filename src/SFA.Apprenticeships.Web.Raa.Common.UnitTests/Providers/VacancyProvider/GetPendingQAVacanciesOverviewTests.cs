namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Linq;
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using Common.Providers;
    using Configuration;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using Manage.UnitTests.Providers.VacancyProvider;
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

        [TestFixtureSetUp]
        public void Setup()
        {
            var utcNow = new DateTime(2016, 01, 29, 11, 39, 34, DateTimeKind.Utc);
            _dateTimeService = new Mock<IDateTimeService>();
            _dateTimeService.Setup(s => s.UtcNow()).Returns(utcNow);

            var submittedTodayDate = utcNow.Date;
            var vacanciesSubmittedToday = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.Status, ProviderVacancyStatuses.PendingQA)
                .With(v => v.DateSubmitted, submittedTodayDate)
                .With(v => v.SubmissionCount, 1)
                .CreateMany(ExpectedSubmittedTodayCount).ToList();

            var vacanciesSubmittedYesterdayUpperBoundary = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.Status, ProviderVacancyStatuses.PendingQA)
                .With(v => v.DateSubmitted, utcNow.Date.AddSeconds(-1))
                .With(v => v.SubmissionCount, 1)
                .CreateMany(ExpectedSubmittedYesterdayCount / 2).ToList();
            var vacanciesSubmittedYesterdayLowerBoundary = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.Status, ProviderVacancyStatuses.PendingQA)
                .With(v => v.DateSubmitted, utcNow.Date.AddDays(-1))
                .With(v => v.SubmissionCount, 1)
                .CreateMany(ExpectedSubmittedYesterdayCount / 2).ToList();

            var submittedMoreThan48HoursDate = utcNow.AddHours(-48).AddSeconds(-1);
            var vacanciesSubmittedMoreThan48Hours = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.Status, ProviderVacancyStatuses.PendingQA)
                .With(v => v.DateSubmitted, submittedMoreThan48HoursDate)
                .With(v => v.SubmissionCount, 1)
                .CreateMany(ExpectedSubmittedMoreThan48HoursCount).ToList();

            var resubmittedDate = utcNow.Date.AddDays(-1).AddSeconds(-1);
            var vacanciesResubmittedHours = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.Status, ProviderVacancyStatuses.PendingQA)
                .With(v => v.DateSubmitted, resubmittedDate)
                .With(v => v.SubmissionCount, 3)
                .CreateMany(ExpectedResubmittedCount).ToList();

            var vacancies = vacanciesSubmittedToday;
            vacancies.AddRange(vacanciesSubmittedYesterdayUpperBoundary);
            vacancies.AddRange(vacanciesSubmittedYesterdayLowerBoundary);
            vacancies.AddRange(vacanciesSubmittedMoreThan48Hours);
            vacancies.AddRange(vacanciesResubmittedHours);

            _vacancyPostingService = new Mock<IVacancyPostingService>();
            _vacancyPostingService.Setup(p => p.GetWithStatus(ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA)).Returns(vacancies);

            _providerService = new Mock<IProviderService>();
            _providerService.Setup(s => s.GetProvider(It.IsAny<string>())).Returns(new Fixture().Create<Provider>());

            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(x => x.Get<ManageWebConfiguration>()).Returns(new ManageWebConfiguration { QAVacancyTimeout = 10 });

            _provider = new VacancyProviderBuilder().With(_vacancyPostingService).With(_providerService).With(_configurationService).With(_dateTimeService).Build();
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
    }
}