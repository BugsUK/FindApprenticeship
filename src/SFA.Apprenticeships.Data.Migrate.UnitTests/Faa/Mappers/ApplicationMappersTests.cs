namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Mappers
{
    using System;
    using FluentAssertions;
    using Migrate.Faa.Entities.Mongo;
    using Migrate.Faa.Mappers;
    using NUnit.Framework;

    [TestFixture]
    public class ApplicationMappersTests
    {
        private readonly IApplicationMappers _applicationMappers = new ApplicationMappers(-1, -1);

        [Test]
        public void SubmittedVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplication
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Status = 30,
                CandidateId = Guid.NewGuid(),
                LegacyApplicationId = 123456,
                WithdrawnOrDeclinedReason = null,
                UnsuccessfulReason = null,
                Vacancy = new Vacancy
                {
                    Id = 654321,
                    Title = "VacancyTitle",
                    VacancyReference = "VAC000987654"
                }
            };
            var candidate = new Candidate
            {
                Id = vacancyApplication.CandidateId,
                LegacyCandidateId = 456789
            };

            //Act
            var application = _applicationMappers.MapApplication(vacancyApplication, candidate);

            //Assert
            application.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            application.CandidateId.Should().Be(candidate.LegacyCandidateId);
            application.VacancyId.Should().Be(vacancyApplication.Vacancy.Id);
            application.ApplicationStatusTypeId.Should().Be(2);
            application.WithdrawnOrDeclinedReasonId.Should().Be(0);
            application.UnsuccessfulReasonId.Should().Be(0);
            application.OutcomeReasonOther.Should().Be(null);
            application.NextActionId.Should().Be(0);
            application.NextActionOther.Should().Be(null);
            application.AllocatedTo.Should().Be(null);
            application.CVAttachmentId.Should().Be(null);
            application.BeingSupportedBy.Should().Be(null);
            application.LockedForSupportUntil.Should().Be(null);
            application.WithdrawalAcknowledged.Should().Be(true);
            application.ApplicationGuid.Should().Be(vacancyApplication.Id);
        }
    }
}