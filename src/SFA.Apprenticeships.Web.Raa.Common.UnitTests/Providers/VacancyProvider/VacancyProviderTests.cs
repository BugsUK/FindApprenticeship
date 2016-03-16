namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Common.Providers;
    using Configuration;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using FluentAssertions;
    using Moq;
    using Moq.Language.Flow;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Vacancy;
    using Web.Common.Configuration;
    using Web.Common.ViewModels;

    [TestFixture]
    public class VacancyProviderTests
    {
        private const int QAVacancyTimeout = 10;

        

        private static FurtherVacancyDetailsViewModel GetValidVacancySummaryViewModel(int vacancyReferenceNumber)
        {
            return new FurtherVacancyDetailsViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(20)),
                    PossibleStartDate = new DateViewModel(DateTime.UtcNow.AddDays(30))
                },
                Duration = 3,
                DurationType = DurationType.Years,
                LongDescription = "A description",
                WageType = WageType.ApprenticeshipMinimumWage,
                HoursPerWeek = 30,
                WorkingWeek = "A working week"
            };
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
            var fourtyEightHoursAndOneMinuteAgo = today.AddMinutes(-(48*60 + 1));
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

        [Test]
        public void GetPendingQAVacanciesShouldOnlyReturnVacanciesAvailableToQa()
        {
            // Arrange
            var today = new DateTime(2016, 3, 16, 12, 0, 0);
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var dateTimeService = new Mock<IDateTimeService>();
            var vacancyLockingService = new Mock<IVacancyLockingService>();
            dateTimeService.Setup(dts => dts.UtcNow).Returns(today);
            const int anInt = 1;
            const string username = "userName";
            const int submissionCount = 2;
            const int vacancyAvailableToQAReferenceNumber = 1;
            const int vacancyNotAvailableToQAReferenceNumber = 2;

            var apprenticeshipVacancies = new List<VacancySummary>
            {
                new VacancySummary
                {
                    ClosingDate = today,
                    DateSubmitted = today,
                    OwnerPartyId = anInt,
                    VacancyReferenceNumber = vacancyAvailableToQAReferenceNumber,
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = username,
                    DateStartedToQA = null,
                    SubmissionCount = submissionCount
                },
                new VacancySummary
                {
                    ClosingDate = today,
                    DateSubmitted = today,
                    OwnerPartyId = anInt,
                    VacancyReferenceNumber = vacancyNotAvailableToQAReferenceNumber,
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

            vacancyLockingService.Setup(
                vls =>
                    vls.IsVacancyAvailableToQABy(username,
                        It.Is<VacancySummary>(vs => vs.VacancyReferenceNumber == vacancyAvailableToQAReferenceNumber))).Returns(true);

            vacancyLockingService.Setup(
                vls =>
                    vls.IsVacancyAvailableToQABy(username,
                        It.Is<VacancySummary>(vs => vs.VacancyReferenceNumber == vacancyNotAvailableToQAReferenceNumber))).Returns(false);

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null); // TODO: move to service!!

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(dateTimeService)
                    .With(vacancyLockingService)
                    .Build();

            var vacancies = vacancyProvider.GetPendingQAVacancies();
            vacancies.Should().HaveCount(1);
            vacancies.Single().VacancyReferenceNumber.Should().Be(vacancyAvailableToQAReferenceNumber);

        }

        [Test]
        public void GetVacanciesPendingQAShouldCallRepositoryWithPendingQAAsDesiredStatus()
        {
            //Arrange
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            const int ownerPartyId = 42;
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration {QAVacancyTimeout = QAVacancyTimeout});
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {BlacklistedCategoryCodes = ""});

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(new List<VacancySummary>
                {
                    new VacancySummary
                    {
                        ClosingDate = DateTime.Now,
                        DateSubmitted = DateTime.Now,
                        OwnerPartyId = ownerPartyId,
                        Status = VacancyStatus.Submitted
                    }
                });

            providerService.Setup(ps => ps.GetProviderViaOwnerParty(ownerPartyId)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(configurationService)
                    .Build();

            //Act
            vacancyProvider.GetPendingQAVacancies();

            //Assert
            vacancyPostingService.Verify(avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA));
            providerService.Verify(ps => ps.GetProviderViaOwnerParty(ownerPartyId), Times.Once);
        }

        [Test]
        public void ReserveForQA_UsernameIsSavedFromCurrentPrinciple()
        {
            //Arrange
            const int vacancyReferenceNumber = 123456;
            const string username = "qa@test.com";
            var reservedVacancy =
                new Fixture().Build<Vacancy>()
                    .With(av => av.Status, VacancyStatus.ReservedForQA)
                    .With(av => av.StandardId, null)
                    .Create();
            var vacancyWithReservedStatus = new Fixture().Build<VacancyViewModel>()
                .With(vvm => vvm.Status, VacancyStatus.ReservedForQA)
                .Create();
            var providerSite = new Fixture().Build<ProviderSite>().Create();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            vacancyPostingService.Setup(r => r.ReserveVacancyForQA(vacancyReferenceNumber)).Returns(reservedVacancy);
            var providerService = new Mock<IProviderService>();
            providerService.Setup(s => s.GetProviderSite(It.IsAny<int>())).Returns(providerSite);
            providerService.Setup(s => s.GetVacancyParty(It.IsAny<int>()))
                .Returns(new Fixture().Build<VacancyParty>().Create());
            var employerService = new Mock<IEmployerService>();
            employerService.Setup(s => s.GetEmployer(It.IsAny<int>())).Returns(new Fixture().Build<Employer>().Create());
            var referenceDataService = new Mock<IReferenceDataService>();
            referenceDataService.Setup(s => s.GetSubCategoryByCode(It.IsAny<string>())).Returns(new Category());
            referenceDataService.Setup(s => s.GetCategoryByCode(It.IsAny<string>())).Returns(new Category());
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration {QAVacancyTimeout = QAVacancyTimeout});
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {BlacklistedCategoryCodes = ""});
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Vacancy, VacancyViewModel>(reservedVacancy))
                .Returns(vacancyWithReservedStatus);

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(referenceDataService)
                    .With(configurationService)
                    .With(vacancyPostingService)
                    .With(employerService)
                    .With(mapper)
                    .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);

            //Act
            var vacancy = vacancyProvider.ReserveVacancyForQA(vacancyReferenceNumber);

            //Assert
            vacancyPostingService.Verify();
            vacancy.Status.Should().Be(VacancyStatus.ReservedForQA);
        }

        [Test]
        public void ShouldSaveCommentsWhenUpdatingVacancyQuestionsViewModel()
        {
            const int vacancyReferenceNumber = 1;
            const string firstQuestionComment = "First question comment";
            const string secondQuestionComment = "Second question comment";

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var provider = new VacancyProviderBuilder().With(vacancyPostingService).Build();

            vacancyPostingService.Setup(vp => vp.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(new Vacancy());
            vacancyPostingService.Setup(vp => vp.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(new Vacancy());

            var viewModel = new VacancyQuestionsViewModel
            {
                FirstQuestionComment = firstQuestionComment,
                SecondQuestionComment = secondQuestionComment,
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            provider.UpdateVacancyWithComments(viewModel);

            vacancyPostingService.Verify(vp => vp.GetVacancyByReferenceNumber(vacancyReferenceNumber));
            vacancyPostingService.Verify(
                vp =>
                    vp.UpdateVacancy(
                        It.Is<Vacancy>(
                            v =>
                                v.FirstQuestionComment == firstQuestionComment &&
                                v.SecondQuestionComment == secondQuestionComment)));
        }

        [Test]
        public void ShouldSaveCommentsWhenUpdatingVacancySummaryViewModel()
        {
            const int vacancyReferenceNumber = 1;
            const string closingDateComment = "Closing date comment";
            const string workingWeekComment = "Working week comment";
            const string wageComment = "Wage comment";
            const string durationComment = "Duration comment";
            const string longDescriptionComment = "Long description comment";
            const string possibleStartDateComment = "Possible start date comment";

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var configService = new Mock<IConfigurationService>();
            configService.Setup(m => m.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {BlacklistedCategoryCodes = string.Empty});
            var provider = new VacancyProviderBuilder().With(vacancyPostingService).With(configService).Build();
            var viewModel = GetValidVacancySummaryViewModel(vacancyReferenceNumber);
            vacancyPostingService.Setup(vp => vp.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(new Vacancy());
            vacancyPostingService.Setup(vp => vp.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(new Vacancy());
            viewModel.VacancyDatesViewModel.ClosingDateComment = closingDateComment;
            viewModel.DurationComment = durationComment;
            viewModel.LongDescriptionComment = longDescriptionComment;
            viewModel.VacancyDatesViewModel.PossibleStartDateComment = possibleStartDateComment;
            viewModel.WageComment = wageComment;
            viewModel.WorkingWeekComment = workingWeekComment;

            provider.UpdateVacancyWithComments(viewModel);

            vacancyPostingService.Verify(vp => vp.GetVacancyByReferenceNumber(vacancyReferenceNumber));
            vacancyPostingService.Verify(
                vp =>
                    vp.UpdateVacancy(
                        It.Is<Vacancy>(
                            v =>
                                v.ClosingDateComment == closingDateComment &&
                                v.DurationComment == durationComment &&
                                v.LongDescriptionComment == longDescriptionComment &&
                                v.PossibleStartDateComment == possibleStartDateComment &&
                                v.WageComment == wageComment &&
                                v.WorkingWeekComment == workingWeekComment)));
        }

        [Test]
        public void UpdateTrainingDetailsWithComments()
        {
            //Arrange
            const string ukprn = "ukprn";

            var trainingDetailsViewModel = new Fixture().Build<TrainingDetailsViewModel>().Create();

            var sectorList = new List<Sector>
            {
                new Fixture().Build<Sector>().Create()
            };

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration {QAVacancyTimeout = QAVacancyTimeout});
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {BlacklistedCategoryCodes = ""});
            var referenceDataService = new Mock<IReferenceDataService>();
            referenceDataService.Setup(m => m.GetSectors()).Returns(sectorList);
            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            //Arrange: get AV, update retrieved AV with NVVM, save modified AV returning same modified AV, map AV to new NVVM with same properties as input
            vacancyPostingService.Setup(
                vps => vps.GetVacancyByReferenceNumber(trainingDetailsViewModel.VacancyReferenceNumber.Value)).Returns(
                    (long refNo) =>
                    {
                        return new Fixture().Build<Vacancy>()
                            .With(av => av.VacancyReferenceNumber, trainingDetailsViewModel.VacancyReferenceNumber.Value)
                            .With(av => av.ApprenticeshipLevelComment, Guid.NewGuid().ToString())
                            .With(av => av.FrameworkCodeNameComment, Guid.NewGuid().ToString())
                            .With(av => av.ApprenticeshipLevel, trainingDetailsViewModel.ApprenticeshipLevel)
                            .With(av => av.FrameworkCodeName, Guid.NewGuid().ToString())
                            .With(av => av.StandardIdComment, Guid.NewGuid().ToString())
                            .With(av => av.StandardId, null)
                            .Create();
                    });

            vacancyPostingService.Setup(vps => vps.UpdateVacancy(It.IsAny<Vacancy>())).Returns((Vacancy av) => av);

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Vacancy, TrainingDetailsViewModel>(It.IsAny<Vacancy>()))
                .Returns((Vacancy av) => trainingDetailsViewModel);

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .With(referenceDataService)
                    .With(mapper)
                    .Build();

            //Act
            var result = vacancyProvider.UpdateVacancyWithComments(trainingDetailsViewModel);

            //Assert
            vacancyPostingService.Verify(
                vps => vps.GetVacancyByReferenceNumber(trainingDetailsViewModel.VacancyReferenceNumber.Value),
                Times.Once);
            vacancyPostingService.Verify(
                vps =>
                    vps.UpdateVacancy(
                        It.Is<Vacancy>(
                            av => av.VacancyReferenceNumber == trainingDetailsViewModel.VacancyReferenceNumber.Value)));
            result.VacancyReferenceNumber.Should().Be(trainingDetailsViewModel.VacancyReferenceNumber);
            result.ApprenticeshipLevelComment.Should().Be(trainingDetailsViewModel.ApprenticeshipLevelComment);
            result.FrameworkCodeNameComment.Should().Be(trainingDetailsViewModel.FrameworkCodeNameComment);
            result.StandardIdComment.Should().Be(trainingDetailsViewModel.StandardIdComment);
            result.StandardId.Should().Be(trainingDetailsViewModel.StandardId);
            result.ApprenticeshipLevel.Should().Be(trainingDetailsViewModel.ApprenticeshipLevel);
            result.FrameworkCodeName.Should().Be(trainingDetailsViewModel.FrameworkCodeName);
        }

        [Test]
        public void UpdateVacancyBasicDetailsShouldExpectVacancyReferenceNumber()
        {
            //Arrange
            var newVacancyVM = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.VacancyReferenceNumber, null)
                .Create();

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration {QAVacancyTimeout = QAVacancyTimeout});
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {BlacklistedCategoryCodes = ""});

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .Build();

            //Act
            Action action = () => vacancyProvider.UpdateVacancyWithComments(newVacancyVM);

            //Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void UpdateVacancyBasicDetailsWithComments()
        {
            //Arrange
            const string ukprn = "ukprn";

            var newVacancyVM = new Fixture().Build<NewVacancyViewModel>().Create();

            var sectorList = new List<Sector>
            {
                new Fixture().Build<Sector>().Create()
            };

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration {QAVacancyTimeout = QAVacancyTimeout});
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {BlacklistedCategoryCodes = ""});
            var referenceDataService = new Mock<IReferenceDataService>();
            referenceDataService.Setup(m => m.GetSectors()).Returns(sectorList);
            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            //Arrange: get AV, update retrieved AV with NVVM, save modified AV returning same modified AV, map AV to new NVVM with same properties as input
            vacancyPostingService.Setup(
                vps => vps.GetVacancyByReferenceNumber(newVacancyVM.VacancyReferenceNumber.Value)).Returns(
                    (long refNo) =>
                    {
                        return new Fixture().Build<Vacancy>()
                            .With(av => av.VacancyReferenceNumber, newVacancyVM.VacancyReferenceNumber.Value)
                            .With(av => av.OfflineApplicationInstructionsComment, Guid.NewGuid().ToString())
                            .With(av => av.OfflineApplicationUrlComment, Guid.NewGuid().ToString())
                            .With(av => av.ShortDescriptionComment, Guid.NewGuid().ToString())
                            .With(av => av.TitleComment, Guid.NewGuid().ToString())
                            .With(av => av.OfflineApplicationUrl, $"http://www.google.com/{Guid.NewGuid()}")
                            .With(av => av.OfflineApplicationInstructions, Guid.NewGuid().ToString())
                            .With(av => av.ShortDescription, Guid.NewGuid().ToString())
                            .With(av => av.Title, Guid.NewGuid().ToString())
                            .Create();
                    });

            vacancyPostingService.Setup(vps => vps.UpdateVacancy(It.IsAny<Vacancy>())).Returns((Vacancy av) => av);

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>()))
                .Returns((Vacancy av) => newVacancyVM);

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .With(referenceDataService)
                    .With(mapper)
                    .Build();

            //Act
            var result = vacancyProvider.UpdateVacancyWithComments(newVacancyVM);

            //Assert
            vacancyPostingService.Verify(
                vps => vps.GetVacancyByReferenceNumber(newVacancyVM.VacancyReferenceNumber.Value), Times.Once);
            vacancyPostingService.Verify(
                vps =>
                    vps.UpdateVacancy(
                        It.Is<Vacancy>(av => av.VacancyReferenceNumber == newVacancyVM.VacancyReferenceNumber.Value)));
            result.VacancyReferenceNumber.Should().Be(newVacancyVM.VacancyReferenceNumber);
            result.OfflineApplicationInstructionsComment.Should().Be(newVacancyVM.OfflineApplicationInstructionsComment);
            result.OfflineApplicationUrlComment.Should().Be(newVacancyVM.OfflineApplicationUrlComment);
            result.ShortDescriptionComment.Should().Be(newVacancyVM.ShortDescriptionComment);
            result.TitleComment.Should().Be(newVacancyVM.TitleComment);
            result.OfflineApplicationInstructions.Should().Be(newVacancyVM.OfflineApplicationInstructions);
            result.OfflineApplicationUrl.Should().Be(newVacancyVM.OfflineApplicationUrl);
            result.ShortDescription.Should().Be(newVacancyVM.ShortDescription);
            result.Title.Should().Be(newVacancyVM.Title);
        }

        [Test]
        public void UpdateVacancyRequirementsAndProspectsWithComments()
        {
            //Arrange
            var vacancyVm = new Fixture().Build<VacancyRequirementsProspectsViewModel>()
                .Create();

            var appVacancy = new Fixture().Build<Vacancy>()
                .With(x => x.VacancyReferenceNumber, vacancyVm.VacancyReferenceNumber)
                .With(x => x.Status, VacancyStatus.Submitted)
                .Create();

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {BlacklistedCategoryCodes = ""});

            vacancyPostingService.Setup(
                vps => vps.GetVacancyByReferenceNumber(vacancyVm.VacancyReferenceNumber)).Returns(appVacancy);

            vacancyPostingService.Setup(vps => vps.UpdateVacancy(It.IsAny<Vacancy>())).Returns(appVacancy);

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .Build();

            //Act
            var result = vacancyProvider.UpdateVacancyWithComments(vacancyVm);

            //Assert
            vacancyPostingService.Verify(vps => vps.GetVacancyByReferenceNumber(vacancyVm.VacancyReferenceNumber),
                Times.Once);
            vacancyPostingService.Verify(
                vps =>
                    vps.UpdateVacancy(It.Is<Vacancy>(av => av.VacancyReferenceNumber == vacancyVm.VacancyReferenceNumber)));
            result.VacancyReferenceNumber.Should().Be(vacancyVm.VacancyReferenceNumber);
            result.DesiredQualifications.Should().Be(vacancyVm.DesiredQualifications);
            result.DesiredQualificationsComment.Should().Be(vacancyVm.DesiredQualificationsComment);
            result.DesiredSkills.Should().Be(vacancyVm.DesiredSkills);
            result.DesiredSkillsComment.Should().Be(vacancyVm.DesiredSkillsComment);
            result.FutureProspectsComment.Should().Be(vacancyVm.FutureProspectsComment);
            result.FutureProspects.Should().Be(vacancyVm.FutureProspects);
            result.PersonalQualitiesComment.Should().Be(vacancyVm.PersonalQualitiesComment);
            result.PersonalQualities.Should().Be(vacancyVm.PersonalQualities);
            result.ThingsToConsiderComment.Should().Be(vacancyVm.ThingsToConsiderComment);
            result.ThingsToConsider.Should().Be(vacancyVm.ThingsToConsider);
        }
    }

    public static class MoqExtensions
    {
        public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup,
            params TResult[] results) where T : class
        {
            setup.Returns(new Queue<TResult>(results).Dequeue);
        }
    }
}