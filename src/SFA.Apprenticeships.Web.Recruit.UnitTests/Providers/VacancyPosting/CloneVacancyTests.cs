namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using System;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

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
            MockTimeService.Setup(s => s.UtcNow()).Returns(dateTimeNow);
            
            var provider = GetVacancyPostingProvider();

            provider.CloneVacancy(initialVacancyReferenceNumber);

            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(v => CheckClonedVacancy(v, newVacancyReferenceNumber, dateTimeNow))));
        }

        private bool CheckClonedVacancy(ApprenticeshipVacancy clonedVacancy, long newVacancyReferenceNumber, DateTime dateTimeNow)
        {
            clonedVacancy.Title.Should().StartWith("(Copy of) ");
            clonedVacancy.DateCreated.Should().Be(dateTimeNow);
            clonedVacancy.DateUpdated.Should().NotHaveValue();
            clonedVacancy.VacancyReferenceNumber.Should().Be(newVacancyReferenceNumber);
            clonedVacancy.DateSubmitted.Should().NotHaveValue();
            clonedVacancy.DateFirstSubmitted.Should().NotHaveValue();
            clonedVacancy.DateStartedToQA.Should().NotHaveValue();
            clonedVacancy.DateQAApproved.Should().NotHaveValue();
            clonedVacancy.Status.Should().Be(ProviderVacancyStatuses.Draft);
            clonedVacancy.ClosingDate.Should().NotHaveValue();
            clonedVacancy.PossibleStartDate.Should().NotHaveValue();
            clonedVacancy.SubmissionCount.Should().Be(0);
            CheckAllCommentsAreNull(clonedVacancy);

            return true;
        }

        private static void CheckAllCommentsAreNull(ApprenticeshipVacancy clonedVacancy)
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

        private ApprenticeshipVacancy GetLiveVacancyWithComments(long initialVacancyReferenceNumber, string initialVacancyTitle)
        {
            return new ApprenticeshipVacancy
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
                DateCreated = DateTime.UtcNow.AddDays(-4),
                DateUpdated = DateTime.UtcNow.AddDays(-1),
                DateSubmitted = DateTime.UtcNow.AddHours(-12),
                DateStartedToQA = DateTime.UtcNow.AddHours(-8),
                QAUserName = "some user name",
                DateQAApproved = DateTime.UtcNow.AddHours(-4),
                Status = ProviderVacancyStatuses.Live,
                ClosingDate = DateTime.UtcNow.AddDays(10),
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    DateCreated = DateTime.UtcNow.AddDays(-10),
                    Description = "some description",
                    EntityId = Guid.NewGuid(),
                    ProviderSiteErn = "ern",
                    WebsiteUrl = "www.google.com",
                    DateUpdated = DateTime.UtcNow.AddDays(-5),
                    Employer = new Employer
                    {
                        Address = new Address
                        {
                            AddressLine1 = "address line 1",
                            AddressLine2 = "address line 2",
                            AddressLine3 = "address line 3",
                            AddressLine4 = "address line 4",
                            Uprn = "uprn",
                            GeoPoint = new GeoPoint(),
                            Postcode = "postcode"
                        },
                        DateCreated = DateTime.UtcNow.AddDays(-20),
                        DateUpdated = null,
                        Ern = "ern",
                        EntityId = Guid.NewGuid(),
                        Name = "employer name"
                    }
                }
            };
        }
    }
}