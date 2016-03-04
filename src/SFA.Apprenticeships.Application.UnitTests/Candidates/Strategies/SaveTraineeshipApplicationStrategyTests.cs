namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    using System;
    using Apprenticeships.Application.Candidate.Strategies.Traineeships;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class SaveTraineeshipApplicationStrategyTests
    {
        private Mock<ITraineeshipApplicationWriteRepository> _traineeshipApplicationWriteRepository;
        private Mock<ICandidateReadRepository> _candidateReadRepository;
        private Mock<ICandidateWriteRepository> _candidateWriteRepository;
        private SaveTraineeshipApplicationStrategy _strategy;
        private TraineeshipApplicationDetail _application;
        private ApplicationTemplate _newCandidateInformation;

        private const int VacancyId = 42;

        private const string AdditionalQuestion1Answer = "Yes";
        private const string AdditionalQuestion2Answer = "No";

        private Guid _candidateId;
        private Candidate _candidate;

        [SetUp]
        public void SetUp()
        {
            _traineeshipApplicationWriteRepository = new Mock<ITraineeshipApplicationWriteRepository>();
            _candidateReadRepository = new Mock<ICandidateReadRepository>();
            _candidateWriteRepository = new Mock<ICandidateWriteRepository>();

            _candidateId = Guid.NewGuid();

            _strategy = new SaveTraineeshipApplicationStrategy(
                _traineeshipApplicationWriteRepository.Object,
                _candidateReadRepository.Object,
                _candidateWriteRepository.Object);

            _newCandidateInformation = new Fixture()
                .Build<ApplicationTemplate>()
                .Create();

            _candidate = new Fixture()
                .Build<Candidate>()
                .Create();

            _application = new Fixture()
                .Build<TraineeshipApplicationDetail>()
                .With(fixture => fixture.CandidateId, _candidateId)
                .With(fixture => fixture.DateApplied, null)
                .With(fixture => fixture.AdditionalQuestion1Answer, AdditionalQuestion1Answer)
                .With(fixture => fixture.AdditionalQuestion2Answer, AdditionalQuestion2Answer)
                .With(fixture => fixture.CandidateInformation, _newCandidateInformation)
                .With(fixture => fixture.Vacancy, new Fixture()
                    .Build<TraineeshipSummary>()
                    .With(fixture => fixture.Id, VacancyId)
                    .Create())
                .Create();
        }

        [Test]
        public void ShouldSaveApplication()
        {
            _traineeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_application))
                .Returns(_application);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _application);

            // Assert.
            savedApplication.Should().NotBeNull();
            savedApplication.Should().Be(_application);

            _traineeshipApplicationWriteRepository
                .Verify(mock => mock.Save(_application), Times.Once);
        }

        [Test]
        public void ShouldSetDateApplied()
        {
            // Arrange.
            _traineeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_application))
                .Returns(_application);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _application);

            // Assert.
            savedApplication.Should().NotBeNull();
            savedApplication.DateApplied.Should().BeCloseTo(DateTime.UtcNow, 500);
        }

        [Test]
        public void ShouldUpdateAdditionalQuestionAnswers()
        {
            // Arrange.
            _traineeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_application))
                .Returns(_application);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _application);

            // Assert.
            savedApplication.Should().NotBeNull();
            savedApplication.AdditionalQuestion1Answer.Should().Be(AdditionalQuestion1Answer);
            savedApplication.AdditionalQuestion2Answer.Should().Be(AdditionalQuestion2Answer);
        }

        [Test]
        public void ShouldUpdateCandidateInformation()
        {
            // Arrange.
            _traineeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_application))
                .Returns(_application);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _application);

            // Assert.
            savedApplication.Should().NotBeNull();
            savedApplication.CandidateInformation.Should().Be(_newCandidateInformation);
        }

        [Test]
        public void ShouldSyncToCandidateApplicationTemplate()
        {
            // Arrange.
            _traineeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_application))
                .Returns(_application);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _application);

            // Assert.
            savedApplication.Should().NotBeNull();

            _candidateReadRepository.
                Verify(mock => mock.Get(_candidateId), Times.Once);

            _candidateWriteRepository.
                Verify(mock => mock.Save(_candidate), Times.Once);

            _candidate.ApplicationTemplate.Should().NotBeNull();
            _candidate.ApplicationTemplate.Qualifications.Should().BeEquivalentTo(_newCandidateInformation.Qualifications);
            _candidate.ApplicationTemplate.WorkExperience.Should().BeEquivalentTo(_newCandidateInformation.WorkExperience);
            _candidate.ApplicationTemplate.TrainingCourses.Should().BeEquivalentTo(_newCandidateInformation.TrainingCourses);
        }

        [TestCase(null, null, null)]
        [TestCase(DisabilityStatus.No, null, DisabilityStatus.No)]
        [TestCase(DisabilityStatus.Yes, null, DisabilityStatus.Yes)]
        [TestCase(DisabilityStatus.No, DisabilityStatus.Yes, DisabilityStatus.Yes)]
        [TestCase(DisabilityStatus.Yes, DisabilityStatus.No, DisabilityStatus.No)]
        [TestCase(DisabilityStatus.Yes, DisabilityStatus.Yes, DisabilityStatus.Yes)]
        public void ShouldSyncToCandidateDisabilityStatus(
            DisabilityStatus? applicationDisabilityStatus,
            DisabilityStatus? originalCandidateDisabilityStatus,
            DisabilityStatus? expectedCandidateDisabilityStatus)
        {
            // Arrange.
            _application.CandidateInformation.DisabilityStatus = applicationDisabilityStatus;
            _candidate.MonitoringInformation.DisabilityStatus = originalCandidateDisabilityStatus;

            _traineeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_application))
                .Returns(_application);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _application);

            // Assert.
            savedApplication.Should().NotBeNull();

            _candidateReadRepository.
                Verify(mock => mock.Get(_candidateId), Times.Once);

            _candidateWriteRepository.
                Verify(mock => mock.Save(_candidate), Times.Once);

            _candidate.MonitoringInformation.Should().NotBeNull();
            _candidate.MonitoringInformation.DisabilityStatus.Should().Be(expectedCandidateDisabilityStatus);
        }
    }
}