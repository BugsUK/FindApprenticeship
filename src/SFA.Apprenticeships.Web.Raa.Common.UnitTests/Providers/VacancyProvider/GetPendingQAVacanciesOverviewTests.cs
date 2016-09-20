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
                .With(v => v.ProviderId, ProviderId)
                .CreateMany(ExpectedSubmittedTodayCount).ToList();

            var vacanciesSubmittedYesterdayUpperBoundary = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, utcNow.Date.AddSeconds(-1))
                .With(v => v.SubmissionCount, 1)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .With(v => v.ProviderId, ProviderId)
                .CreateMany(ExpectedSubmittedYesterdayCount / 2).ToList();
            var vacanciesSubmittedYesterdayLowerBoundary = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, utcNow.Date.AddDays(-1))
                .With(v => v.SubmissionCount, 1)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .With(v => v.ProviderId, ProviderId)
                .CreateMany(ExpectedSubmittedYesterdayCount / 2).ToList();

            var submittedMoreThan48HoursDate = utcNow.AddHours(-48).AddSeconds(-1);
            var vacanciesSubmittedMoreThan48Hours = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, submittedMoreThan48HoursDate)
                .With(v => v.SubmissionCount, 1)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .With(v => v.ProviderId, ProviderId)
                .CreateMany(ExpectedSubmittedMoreThan48HoursCount).ToList();

            var resubmittedDate = utcNow.Date.AddDays(-1).AddSeconds(-1);
            var vacanciesResubmittedHours = new Fixture().Build<Vacancy>()
                .With(v => v.Status, VacancyStatus.Submitted)
                .With(v => v.DateSubmitted, resubmittedDate)
                .With(v => v.SubmissionCount, 3)
                .With(v => v.RegionalTeam, RegionalTeam.Other)
                .With(v => v.ProviderId, ProviderId)
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
            _providerService.Setup(s => s.GetProviders(It.IsAny<IEnumerable<int>>())).Returns(new List<Provider>{new Fixture().Build<Provider>().With(p => p.ProviderId, ProviderId).Create()});

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

            providerService.Setup(ps => ps.GetProvider(It.IsAny<int>())).Returns(new Provider());

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
                    ProviderId = ProviderId,
                    VacancyReferenceNumber = anInt,
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = username,
                    DateStartedToQA = null
                }
            };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(s => s.GetProviders(It.IsAny<IEnumerable<int>>())).Returns(new List<Provider> { new Fixture().Build<Provider>().With(p => p.ProviderId, ProviderId).Create() });

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
            //sub fri, not seen: sat, seen: sun <==== special (should be sat)
            //sub sat, seen: sun <= normal
            //sub sun, seen: mon <= normal
            //sub mon, seen: tues <= normal
            //sub tues, seen: weds <= normal
            //sub weds, seen: thurs <= normal
            //sub thurs, seen: fri <= normal

            var _lastSundayIn2015 = new DateTime(2015, 12, 27);
            var _lastMondayIn2015 = new DateTime(2015, 12, 28);
            var _lastTuesdayIn2015 = new DateTime(2015, 12, 29);
            var _lastWednesdayIn2015 = new DateTime(2015, 12, 30);
            var _lastThursdayIn2015 = new DateTime(2015, 12, 31);
            var _firstDayOf2016_Friday = new DateTime(2016, 1, 1);
            var _firstSaturdayIn2016 = new DateTime(2016, 1, 2);
            var _firstSundayIn2016 = new DateTime(2016, 1, 3);

            //Date submitted, Date viewed, should be Visible
            var paramSets = new List<Tuple<DateTime, DateTime, bool>>();
            
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_firstDayOf2016_Friday, _firstSaturdayIn2016, false));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_firstDayOf2016_Friday, _firstSundayIn2016, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_firstSaturdayIn2016, _firstSundayIn2016, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastSundayIn2015, _lastMondayIn2015, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastMondayIn2015, _lastTuesdayIn2015, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastTuesdayIn2015, _lastWednesdayIn2015, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastThursdayIn2015, _firstDayOf2016_Friday, true));

            foreach (var paramset in paramSets)
            {
                //Arrange
                var dateSubmitted = paramset.Item1;
                var closingDate = paramset.Item2.AddDays(14);
                var vacancyPostingService = new Mock<IVacancyPostingService>();
                var providerService = new Mock<IProviderService>();
                var dateTimeService = new Mock<IDateTimeService>();
                dateTimeService.Setup(dts => dts.UtcNow).Returns(paramset.Item2);
                const int anInt = 1;
                const string username = "userName";

                var apprenticeshipVacancies = new List<VacancySummary>
                {
                    new VacancySummary
                    {
                        ClosingDate = closingDate,
                        DateSubmitted = dateSubmitted,
                        ProviderId = ProviderId,
                        VacancyReferenceNumber = anInt,
                        Status = VacancyStatus.ReservedForQA,
                        QAUserName = username,
                        DateStartedToQA = null
                    }
                };

                vacancyPostingService.Setup(
                    avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                    .Returns(apprenticeshipVacancies);

                providerService.Setup(s => s.GetProviders(It.IsAny<IEnumerable<int>>())).Returns(new List<Provider> { new Fixture().Build<Provider>().With(p => p.ProviderId, ProviderId).Create() });

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
                var shouldBeVisible = paramset.Item3;
                if (shouldBeVisible)
                {
                    vacancies.Should().HaveCount(1);
                }
                else
                {
                    vacancies.Should().HaveCount(0);
                }

                //REMOVED THIS SECTION, AS THERE IS MORE TO THIS CALL, NOW
                ////Act
                //vacancies =
                //    vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                //    {
                //        FilterType = DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours
                //    }).Vacancies;
                //
                ////Assert
                //vacancies.Should().HaveCount(0);

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
        }

        [Test]
        public void GetPendingQAVacanciesOverviewShouldReturnVacanciesSubmittedMoreThan48HoursAgo_IgnoreWeekends()
        {
            //sub fri, not seen: sun mon, seen: tues <==== special (should be sun)
            //sub sat, not seen: mon tues, seen: weds <=== special (should be mon)
            //sub sun, not seen: tues, seen: weds <=== special (should be tues)
            //sub mon, not seen: tues, seen: weds <= normal
            //sub tues, not seen: weds, seen: thurs <= normal
            //sub weds, not seen: thurs, seen: fri <= normal
            //sub thurs, not seen: fri, seen: sat <= normal

            var _lastFridayIn2015 = new DateTime(2015, 12, 25);
            var _lastSaturdayIn2015 = new DateTime(2015, 12, 26);
            var _lastSundayIn2015 = new DateTime(2015, 12, 27);
            var _lastMondayIn2015 = new DateTime(2015, 12, 28);
            var _lastTuesdayIn2015 = new DateTime(2015, 12, 29);
            var _lastWednesdayIn2015 = new DateTime(2015, 12, 30);
            var _lastThursdayIn2015 = new DateTime(2015, 12, 31);
            var _firstDayOf2016_Friday = new DateTime(2016, 1, 1);
            var _firstSaturdayIn2016 = new DateTime(2016, 1, 2);

            //Date submitted, Date viewed, should be Visible
            var paramSets = new List<Tuple<DateTime, DateTime, bool>>();

            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastFridayIn2015, _lastSundayIn2015, false));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastFridayIn2015, _lastMondayIn2015, false));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastFridayIn2015, _lastTuesdayIn2015, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastSaturdayIn2015, _lastMondayIn2015, false));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastSaturdayIn2015, _lastTuesdayIn2015, false));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastSaturdayIn2015, _lastWednesdayIn2015, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastSundayIn2015, _lastTuesdayIn2015, false));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastSundayIn2015, _lastWednesdayIn2015, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastMondayIn2015, _lastWednesdayIn2015, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastTuesdayIn2015, _lastThursdayIn2015, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastWednesdayIn2015, _firstDayOf2016_Friday, true));
            paramSets.Add(new Tuple<DateTime, DateTime, bool>(_lastThursdayIn2015, _firstSaturdayIn2016, true));

            foreach (var paramSet in paramSets)
            {
                // Arrange
                var today = paramSet.Item2;
                var dateSubmitted = paramSet.Item1;
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
                        ClosingDate = dateSubmitted.AddDays(14),
                        DateSubmitted = dateSubmitted,
                        ProviderId = ProviderId,
                        VacancyReferenceNumber = anInt,
                        Status = VacancyStatus.ReservedForQA,
                        QAUserName = username,
                        DateStartedToQA = null
                    }
                };

                vacancyPostingService.Setup(
                    avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                    .Returns(apprenticeshipVacancies);

                providerService.Setup(s => s.GetProviders(It.IsAny<IEnumerable<int>>())).Returns(new List<Provider> { new Fixture().Build<Provider>().With(p => p.ProviderId, ProviderId).Create() });

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

                //REMOVED THIS SECTION, AS THERE IS MORE TO THIS CALL, NOW
                ////Act
                //vacancies =
                //    vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                //    {
                //        FilterType = DashboardVacancySummaryFilterTypes.SubmittedYesterday
                //    }).Vacancies;
                //
                ////Assert
                //vacancies.Should().HaveCount(0);

                //Act
                vacancies =
                    vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel
                    {
                        FilterType = DashboardVacancySummaryFilterTypes.SubmittedMoreThan48Hours
                    }).Vacancies;

                //Assert
                var shouldBeVisible = paramSet.Item3;
                if (shouldBeVisible)
                {
                    vacancies.Should().HaveCount(1);
                }
                else
                {
                    vacancies.Should().HaveCount(0, $"${paramSet.Item1} ${paramSet.Item2} ${paramSet.Item3}");
                }

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
                    ProviderId = ProviderId,
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

            providerService.Setup(s => s.GetProviders(It.IsAny<IEnumerable<int>>())).Returns(new List<Provider> { new Fixture().Build<Provider>().With(p => p.ProviderId, ProviderId).Create() });

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