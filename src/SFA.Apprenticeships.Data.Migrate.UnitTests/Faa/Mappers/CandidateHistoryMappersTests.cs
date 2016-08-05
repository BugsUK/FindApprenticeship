namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Builders;
    using FluentAssertions;
    using Migrate.Faa.Mappers;
    using NUnit.Framework;

    [TestFixture]
    public class CandidateHistoryMappersTests
    {
        [Test]
        public void UnactivatedCandidateUserTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).Build();

            //Act
            var candidateHistory = candidateUser.MapCandidateHistory(candidateUser.Candidate.LegacyCandidateId, new Dictionary<int, Dictionary<int, int>>());

            //Assert
            candidateHistory.Should().NotBeNullOrEmpty();
            candidateHistory.Count.Should().Be(1);
            var createdHistory = candidateHistory.First();
            createdHistory.CandidateId.Should().Be(candidateUser.Candidate.LegacyCandidateId);
            createdHistory.CandidateHistoryEventTypeId.Should().Be(1);
            createdHistory.CandidateHistorySubEventTypeId.Should().Be(1);
            createdHistory.EventDate.Should().Be(candidateUser.User.DateCreated);
            createdHistory.Comment.Should().BeNull();
            createdHistory.UserName.Should().Be("dummy");
        }

        [TestCase(20)] //Activated
        [TestCase(30)] //Inactive
        [TestCase(90)] //Locked
        [TestCase(100)] //Dormant
        public void ActivatedCandidateUserTest(int status)
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(status).Build();

            //Act
            var candidateHistory = candidateUser.MapCandidateHistory(candidateUser.Candidate.LegacyCandidateId, new Dictionary<int, Dictionary<int, int>>());

            //Assert
            candidateHistory.Should().NotBeNullOrEmpty();
            candidateHistory.Count.Should().Be(3);
            var createdHistory = candidateHistory[0];
            createdHistory.CandidateHistoryEventTypeId.Should().Be(1);
            createdHistory.CandidateHistorySubEventTypeId.Should().Be(1);
            var activatedHistory = candidateHistory[1];
            activatedHistory.CandidateId.Should().Be(candidateUser.Candidate.LegacyCandidateId);
            activatedHistory.CandidateHistoryEventTypeId.Should().Be(1);
            activatedHistory.CandidateHistorySubEventTypeId.Should().Be(2);
            // ReSharper disable once PossibleInvalidOperationException
            activatedHistory.EventDate.Should().Be(candidateUser.User.ActivationDate.Value);
            activatedHistory.Comment.Should().BeNull();
            activatedHistory.UserName.Should().Be("NAS Gateway");
            var noteHistory = candidateHistory[2];
            noteHistory.CandidateId.Should().Be(candidateUser.Candidate.LegacyCandidateId);
            noteHistory.CandidateHistoryEventTypeId.Should().Be(3);
            noteHistory.CandidateHistorySubEventTypeId.Should().Be(0);
            noteHistory.EventDate.Should().Be(candidateUser.User.ActivationDate.Value);
            noteHistory.Comment.Should().Be("NAS Exemplar registered Candidate.");
            noteHistory.UserName.Should().Be("NAS Gateway");
        }

        [TestCase(true, 0, true)]
        [TestCase(false, 0, false)]
        [TestCase(true, 456789, true)]
        [TestCase(false, 456789, true)]
        public void PendingDeletionCandidateUserTest(bool activated, int legacyCandidateId, bool expectedActivated)
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(999).WithActivated(activated).WithLegacyCandidateId(legacyCandidateId).Build();

            //Act
            var candidateHistory = candidateUser.MapCandidateHistory(candidateUser.Candidate.LegacyCandidateId, new Dictionary<int, Dictionary<int, int>>());

            //Assert
            int expectedCount = expectedActivated ? 3 : 2;
            candidateHistory.Should().NotBeNullOrEmpty();
            candidateHistory.Count.Should().Be(expectedCount);
            var createdHistory = candidateHistory[0];
            createdHistory.CandidateHistoryEventTypeId.Should().Be(1);
            createdHistory.CandidateHistorySubEventTypeId.Should().Be(1);
            if (activated)
            {
                var activatedHistory = candidateHistory[1];
                activatedHistory.CandidateId.Should().Be(candidateUser.Candidate.LegacyCandidateId);
                activatedHistory.CandidateHistoryEventTypeId.Should().Be(1);
                activatedHistory.CandidateHistorySubEventTypeId.Should().Be(2);
                // ReSharper disable once PossibleInvalidOperationException
                activatedHistory.EventDate.Should().Be(candidateUser.User.ActivationDate.Value);
                activatedHistory.Comment.Should().BeNull();
                activatedHistory.UserName.Should().Be("NAS Gateway");
            }
            var noteHistory = candidateHistory[expectedCount-1];
            noteHistory.CandidateId.Should().Be(candidateUser.Candidate.LegacyCandidateId);
            noteHistory.CandidateHistoryEventTypeId.Should().Be(3);
            noteHistory.CandidateHistorySubEventTypeId.Should().Be(0);
            noteHistory.EventDate.Should().Be(candidateUser.User.ActivationDate ?? candidateUser.User.DateUpdated ?? candidateUser.User.DateCreated);
            noteHistory.Comment.Should().Be("NAS Exemplar registered Candidate.");
            noteHistory.UserName.Should().Be("NAS Gateway");
        }

        [Test]
        public void ActivatedCandidateUserDictionaryTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(20).Build();

            //Act
            var candidateHistory = candidateUser.MapCandidateHistory(candidateUser.Candidate.LegacyCandidateId, new Dictionary<int, Dictionary<int, int>>());
            var candidateHistoryDictionary = candidateHistory.MapCandidateHistoryDictionary();

            //Assert
            candidateHistoryDictionary.Should().NotBeNullOrEmpty();
            candidateHistoryDictionary.Count.Should().Be(3);
            var createdHistory = candidateHistoryDictionary[0];
            createdHistory["CandidateHistoryEventTypeId"].Should().Be(1);
            createdHistory["CandidateHistorySubEventTypeId"].Should().Be(1);
            var activatedHistory = candidateHistoryDictionary[1];
            activatedHistory["CandidateHistoryEventTypeId"].Should().Be(1);
            activatedHistory["CandidateHistorySubEventTypeId"].Should().Be(2);
            var noteHistory = candidateHistoryDictionary[2];
            noteHistory["CandidateId"].Should().Be(candidateUser.Candidate.LegacyCandidateId);
            noteHistory["CandidateHistoryEventTypeId"].Should().Be(3);
            noteHistory["CandidateHistorySubEventTypeId"].Should().Be(0);
            // ReSharper disable once PossibleInvalidOperationException
            noteHistory["EventDate"].Should().Be(candidateUser.User.ActivationDate.Value);
            noteHistory["Comment"].Should().Be("NAS Exemplar registered Candidate.");
            noteHistory["UserName"].Should().Be("NAS Gateway");
        }

        [Test]
        public void NoCandidateIdCandidateUserTest()
        {
            //Arrange
            var candidateUser = new CandidateUserBuilder().WithStatus(10).WithLegacyCandidateId(0).Build();

            //Act
            var candidateHistory = candidateUser.MapCandidateHistory(candidateUser.Candidate.LegacyCandidateId, new Dictionary<int, Dictionary<int, int>>());

            //Assert
            candidateHistory[0].CandidateHistoryId.Should().Be(0);
        }

        [Test]
        public void MatchingCandidateIdCandidateUserTest()
        {
            //Arrange
            const int candidateId = 42;
            const int candidateHistoryId = 43;
            var candidateUser = new CandidateUserBuilder().WithStatus(20).WithLegacyCandidateId(candidateId).Build();
            var candidateHistoryIds = new Dictionary<int, Dictionary<int, int>>
            {
                { candidateId, new Dictionary<int, int> {{ 1, candidateHistoryId }}}
            };

            //Act
            var candidateHistory = candidateUser.MapCandidateHistory(candidateUser.Candidate.LegacyCandidateId, candidateHistoryIds);

            //Assert
            candidateHistory[0].CandidateHistoryId.Should().Be(43);
            candidateHistory[1].CandidateHistoryId.Should().Be(0);
        }
    }
}