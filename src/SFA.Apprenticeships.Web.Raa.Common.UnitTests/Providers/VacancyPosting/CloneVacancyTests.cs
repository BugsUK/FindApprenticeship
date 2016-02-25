namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class CloneVacancyTests : TestBase
    {
        [Test]
        public void CloneVacancyShouldSaveTheClonedVacancy()
        {
            const long initialVacancyReferenceNumber = 1;
            const string initialVacancyTitle = "title";
            const long newVacancyReferenceNumber = 2;
            var dateTimeNow = DateTime.UtcNow;

            MockVacancyPostingService.Setup(s => s.GetVacancy(initialVacancyReferenceNumber))
                .Returns(GetLiveVacancyWithComments(initialVacancyReferenceNumber, initialVacancyTitle));
            MockVacancyPostingService.Setup(s => s.GetNextVacancyReferenceNumber()).Returns(newVacancyReferenceNumber);
            MockProviderService.Setup(s => s.GetVacancyParty(It.IsAny<int>()))
                .Returns(new Fixture().Build<VacancyParty>().Create());
            MockEmployerService.Setup(s => s.GetEmployer(It.IsAny<int>()))
                .Returns(new Fixture().Build<Employer>().Create());

            MockTimeService.Setup(s => s.UtcNow()).Returns(dateTimeNow);
            
            var provider = GetVacancyPostingProvider();

            provider.CloneVacancy(initialVacancyReferenceNumber);

            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<Vacancy>(v => CheckClonedVacancy(v, newVacancyReferenceNumber, dateTimeNow))));
        }

        private bool CheckClonedVacancy(Vacancy clonedVacancy, long newVacancyReferenceNumber, DateTime dateTimeNow)
        {
            clonedVacancy.Title.Should().StartWith("(Copy of) ");
            clonedVacancy.CreatedDateTime.Should().Be(dateTimeNow);
            clonedVacancy.UpdatedDateTime.Should().NotHaveValue();
            clonedVacancy.VacancyReferenceNumber.Should().Be(newVacancyReferenceNumber);
            clonedVacancy.DateSubmitted.Should().NotHaveValue();
            clonedVacancy.DateFirstSubmitted.Should().NotHaveValue();
            clonedVacancy.DateStartedToQA.Should().NotHaveValue();
            clonedVacancy.DateQAApproved.Should().NotHaveValue();
            clonedVacancy.Status.Should().Be(VacancyStatus.Draft);
            clonedVacancy.ClosingDate.Should().NotHaveValue();
            clonedVacancy.PossibleStartDate.Should().NotHaveValue();
            clonedVacancy.SubmissionCount.Should().Be(0);
            CheckAllCommentsAreNull(clonedVacancy);

            return true;
        }

        private static void CheckAllCommentsAreNull(Vacancy clonedVacancy)
        {
            clonedVacancy.WorkingWeekComment.Should().BeNull();
            clonedVacancy.ApprenticeshipLevelComment.Should().BeNull();
            clonedVacancy.ClosingDateComment.Should().BeNull();
            clonedVacancy.DesiredQualificationsComment.Should().BeNull();
            clonedVacancy.DesiredSkillsComment.Should().BeNull();
            clonedVacancy.DurationComment.Should().BeNull();
            clonedVacancy.FirstQuestionComment.Should().BeNull();
            clonedVacancy.FrameworkCodeNameComment.Should().BeNull();
            clonedVacancy.FutureProspectsComment.Should().BeNull();
            clonedVacancy.LongDescriptionComment.Should().BeNull();
        }

        private Vacancy GetLiveVacancyWithComments(long initialVacancyReferenceNumber, string initialVacancyTitle)
        {
            return new Vacancy
            {
                VacancyReferenceNumber = initialVacancyReferenceNumber,
                Title = initialVacancyTitle,
                WorkingWeekComment = "some comment",
                ApprenticeshipLevelComment = "some comment",
                ClosingDateComment = "some comment",
                DesiredQualificationsComment = "some comment",
                DesiredSkillsComment = "some comment",
                DurationComment = "some comment",
                FirstQuestionComment = "some comment",
                FrameworkCodeNameComment = "some comment",
                FutureProspectsComment = "some comment",
                LongDescriptionComment = "some comment",
                CreatedDateTime = DateTime.UtcNow.AddDays(-4),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-1),
                DateSubmitted = DateTime.UtcNow.AddHours(-12),
                DateStartedToQA = DateTime.UtcNow.AddHours(-8),
                QAUserName = "some user name",
                DateQAApproved = DateTime.UtcNow.AddHours(-4),
                Status = VacancyStatus.Live,
                ClosingDate = DateTime.UtcNow.AddDays(10),
                OwnerPartyId = 42
            };
        }
    }
}