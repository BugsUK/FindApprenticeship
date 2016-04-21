namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Builders;
    using FluentAssertions;
    using Migrate.Faa.Mappers;
    using NUnit.Framework;

    [TestFixture]
    public class ApplicationMappersTests
    {
        private readonly IApplicationMappers _applicationMappers = new ApplicationMappers(-1, -1);

        [Test]
        public void SavedVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(5).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

            //Act
            Action mapApplicationAction = () => _applicationMappers.MapApplication(vacancyApplication, candidate);

            //Assert
            mapApplicationAction.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void DraftVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(10).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

            //Act
            var application = _applicationMappers.MapApplication(vacancyApplication, candidate);

            //Assert
            application.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            application.CandidateId.Should().Be(candidate.LegacyCandidateId);
            application.VacancyId.Should().Be(vacancyApplication.Vacancy.Id);
            application.ApplicationStatusTypeId.Should().Be(1);
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

        [Test]
        public void SubmittingVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(20).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

            //Act
            var application = _applicationMappers.MapApplication(vacancyApplication, candidate);

            //Assert
            application.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            application.CandidateId.Should().Be(candidate.LegacyCandidateId);
            application.VacancyId.Should().Be(vacancyApplication.Vacancy.Id);
            application.ApplicationStatusTypeId.Should().Be(1);
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

        [Test]
        public void SubmittedVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(30).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

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

        [Test]
        public void InProgressVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(40).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

            //Act
            var application = _applicationMappers.MapApplication(vacancyApplication, candidate);

            //Assert
            application.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            application.CandidateId.Should().Be(candidate.LegacyCandidateId);
            application.VacancyId.Should().Be(vacancyApplication.Vacancy.Id);
            application.ApplicationStatusTypeId.Should().Be(3);
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

        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase("UNKNOWN", 0)]
        [TestCase("No longer interested", 1)]
        [TestCase("Accepted another Apprenticeship offer", 2)]
        [TestCase("Accepted an alternative job", 3)]
        [TestCase("Decided to go to college", 4)]
        [TestCase("Decided to stay on at 6th form", 5)]
        [TestCase("Want to be able to apply for other Apprenticeships", 6)]
        [TestCase("Personal reasons", 7)]
        [TestCase("Moving away", 8)]
        [TestCase("Pay or Conditions not acceptable", 9)]
        [TestCase("Other (please specify)", 10)]
        public void WithdrawnVacancyApplicationTest(string withdrawnOrDeclinedReason, int expectedWithdrawnOrDeclinedReasonId)
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(15).WithWithdrawnOrDeclinedReason(withdrawnOrDeclinedReason).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

            //Act
            var application = _applicationMappers.MapApplication(vacancyApplication, candidate);

            //Assert
            application.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            application.CandidateId.Should().Be(candidate.LegacyCandidateId);
            application.VacancyId.Should().Be(vacancyApplication.Vacancy.Id);
            application.ApplicationStatusTypeId.Should().Be(4);
            application.WithdrawnOrDeclinedReasonId.Should().Be(expectedWithdrawnOrDeclinedReasonId);
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

        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase("UNKNOWN", 0)]
        [TestCase("Please Select...", 0)]
        [TestCase("You’re not eligible for an apprenticeship", 1)]
        [TestCase("You haven’t met the requirements", 2)]
        [TestCase("You met the employer’s/provider's requirements but have been unsuccessful", 3)]
        [TestCase("You didn’t attend the interview", 4)]
        [TestCase("The apprenticeship has been withdrawn", 5)]
        [TestCase("You've been unsuccessful - other", 6)]
        [TestCase("Not suitable for vacancy due to journey / distance involved", 7)]
        [TestCase("Failed initial assessment test", 8)]
        [TestCase("Employer withdrew vacancy", 9)]
        [TestCase("Accepted an alternative apprenticeship position", 10)]
        [TestCase("Have found other employment", 11)]
        [TestCase("Taken up other learning or education", 12)]
        [TestCase("Other reason for Withdrawing Application", 13)]
        [TestCase("Other", 14)]
        [TestCase("You’re not eligible for a traineeship", 15)]
        [TestCase("The training provider couldn’t contact you", 16)]
        [TestCase("Offered the position but turned it down", 17)]
        public void UnsuccessfulVacancyApplicationTest(string unsuccessfulReason, int expectedUnsuccessfulReasonId)
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(90).WithUnsuccessfulReason(unsuccessfulReason).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

            //Act
            var application = _applicationMappers.MapApplication(vacancyApplication, candidate);

            //Assert
            application.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            application.CandidateId.Should().Be(candidate.LegacyCandidateId);
            application.VacancyId.Should().Be(vacancyApplication.Vacancy.Id);
            application.ApplicationStatusTypeId.Should().Be(5);
            application.WithdrawnOrDeclinedReasonId.Should().Be(0);
            application.UnsuccessfulReasonId.Should().Be(expectedUnsuccessfulReasonId);
            application.OutcomeReasonOther.Should().Be(expectedUnsuccessfulReasonId == 0 ? null : "");
            application.NextActionId.Should().Be(0);
            application.NextActionOther.Should().Be(null);
            application.AllocatedTo.Should().Be(expectedUnsuccessfulReasonId == 0 || expectedUnsuccessfulReasonId == 13 ? null : "");
            application.CVAttachmentId.Should().Be(null);
            application.BeingSupportedBy.Should().Be(null);
            application.LockedForSupportUntil.Should().Be(null);
            application.WithdrawalAcknowledged.Should().Be(expectedUnsuccessfulReasonId != 10 && expectedUnsuccessfulReasonId != 11 && expectedUnsuccessfulReasonId != 13);
            application.ApplicationGuid.Should().Be(vacancyApplication.Id);
        }

        [Test]
        public void SuccessfulVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(80).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

            //Act
            var application = _applicationMappers.MapApplication(vacancyApplication, candidate);

            //Assert
            application.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            application.CandidateId.Should().Be(candidate.LegacyCandidateId);
            application.VacancyId.Should().Be(vacancyApplication.Vacancy.Id);
            application.ApplicationStatusTypeId.Should().Be(6);
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

        [Test]
        public void SuccessfulApplicationWithHistoryTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(80).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

            //Act
            var application = _applicationMappers.MapApplicationWithHistory(vacancyApplication, candidate);

            //Assert
            application.Application.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            application.Application.CandidateId.Should().Be(candidate.LegacyCandidateId);
            application.Application.VacancyId.Should().Be(vacancyApplication.Vacancy.Id);
            application.Application.ApplicationStatusTypeId.Should().Be(6);
            application.Application.WithdrawnOrDeclinedReasonId.Should().Be(0);
            application.Application.UnsuccessfulReasonId.Should().Be(0);
            application.Application.OutcomeReasonOther.Should().Be(null);
            application.Application.NextActionId.Should().Be(0);
            application.Application.NextActionOther.Should().Be(null);
            application.Application.AllocatedTo.Should().Be(null);
            application.Application.CVAttachmentId.Should().Be(null);
            application.Application.BeingSupportedBy.Should().Be(null);
            application.Application.LockedForSupportUntil.Should().Be(null);
            application.Application.WithdrawalAcknowledged.Should().Be(true);
            application.Application.ApplicationGuid.Should().Be(vacancyApplication.Id);
            application.ApplicationHistory.Should().NotBeNullOrEmpty();
            application.ApplicationHistory.Count.Should().Be(4);
            application.ApplicationHistory.All(a => a.ApplicationId == application.Application.ApplicationId).Should().BeTrue();
        }

        [Test]
        public void NoLegacyIdsVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(10).WithLegacyApplicationId(0).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).WithLegacyCandidateId(0).Build();

            //Act
            var application = _applicationMappers.MapApplication(vacancyApplication, candidate);

            //Assert
            application.ApplicationId.Should().Be(-1);
            application.CandidateId.Should().Be(-1);
        }

        [Test]
        public void SubmittedVacancyApplicationDictionaryTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(30).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

            //Act
            var application = _applicationMappers.MapApplicationDictionary(vacancyApplication, candidate);

            //Assert
            application["ApplicationId"].Should().Be(vacancyApplication.LegacyApplicationId);
            application["CandidateId"].Should().Be(candidate.LegacyCandidateId);
            application["VacancyId"].Should().Be(vacancyApplication.Vacancy.Id);
            application["ApplicationStatusTypeId"].Should().Be(2);
            application["WithdrawnOrDeclinedReasonId"].Should().Be(0);
            application["UnsuccessfulReasonId"].Should().Be(0);
            application["OutcomeReasonOther"].Should().Be(null);
            application["NextActionId"].Should().Be(0);
            application["NextActionOther"].Should().Be(null);
            application["AllocatedTo"].Should().Be(null);
            application["CVAttachmentId"].Should().Be(null);
            application["BeingSupportedBy"].Should().Be(null);
            application["LockedForSupportUntil"].Should().Be(null);
            application["WithdrawalAcknowledged"].Should().Be(true);
            application["ApplicationGuid"].Should().Be(vacancyApplication.Id);
        }

        [Test]
        public void SubmittedApplicationWithHistoryDictionaryTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(30).Build();
            var candidate = new CandidateSummaryBuilder().WithCandidateId(vacancyApplication.CandidateId).Build();

            //Act
            var application = _applicationMappers.MapApplicationWithHistoryDictionary(vacancyApplication, candidate);

            //Assert
            application.Application["ApplicationId"].Should().Be(vacancyApplication.LegacyApplicationId);
            application.Application["CandidateId"].Should().Be(candidate.LegacyCandidateId);
            application.Application["VacancyId"].Should().Be(vacancyApplication.Vacancy.Id);
            application.Application["ApplicationStatusTypeId"].Should().Be(2);
            application.Application["WithdrawnOrDeclinedReasonId"].Should().Be(0);
            application.Application["UnsuccessfulReasonId"].Should().Be(0);
            application.Application["OutcomeReasonOther"].Should().Be(null);
            application.Application["NextActionId"].Should().Be(0);
            application.Application["NextActionOther"].Should().Be(null);
            application.Application["AllocatedTo"].Should().Be(null);
            application.Application["CVAttachmentId"].Should().Be(null);
            application.Application["BeingSupportedBy"].Should().Be(null);
            application.Application["LockedForSupportUntil"].Should().Be(null);
            application.Application["WithdrawalAcknowledged"].Should().Be(true);
            application.Application["ApplicationGuid"].Should().Be(vacancyApplication.Id);
            application.ApplicationHistory.Should().NotBeNull();
            application.ApplicationHistory.Count.Should().Be(2);
            application.ApplicationHistory.All(a => (int)a["ApplicationId"] == (int)application.Application["ApplicationId"]).Should().BeTrue();
        }
    }
}