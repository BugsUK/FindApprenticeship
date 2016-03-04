namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    using System;
    using Apprenticeships.Application.Candidate.Strategies.Apprenticeships;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class SaveApprenticeshipApplicationStrategyTests
    {
        private Mock<IApprenticeshipApplicationReadRepository> _apprenticeshipApplicationReadRepository;
        private Mock<IApprenticeshipApplicationWriteRepository> _apprenticeshipApplicationWriteRepository;
        private Mock<ICandidateReadRepository> _candidateReadRepository;
        private Mock<ICandidateWriteRepository> _candidateWriteRepository;
        private SaveApprenticeshipApplicationStrategy _strategy;
        private ApprenticeshipApplicationDetail _newApplication;
        private ApprenticeshipApplicationDetail _oldApplication;
        private ApplicationTemplate _newCandidateInformation;

        private const int VacancyId = 42;

        private const string AdditionalQuestion1Answer = "Yes";
        private const string AdditionalQuestion2Answer = "No";

        private Guid _candidateId;
        private Candidate _candidate;

        [SetUp]
        public void SetUp()
        {
            _apprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            _apprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            _candidateReadRepository = new Mock<ICandidateReadRepository>();
            _candidateWriteRepository = new Mock<ICandidateWriteRepository>();

            _candidateId = Guid.NewGuid();

            _strategy = new SaveApprenticeshipApplicationStrategy(
                _apprenticeshipApplicationReadRepository.Object,
                _apprenticeshipApplicationWriteRepository.Object,
                _candidateReadRepository.Object,
                _candidateWriteRepository.Object);

            _oldApplication = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.CandidateId, _candidateId)
                .With(fixture => fixture.Status, ApplicationStatuses.Draft)
                .Create();

            _newCandidateInformation = new Fixture()
                .Build<ApplicationTemplate>()
                .Create();

            _candidate = new Fixture()
                .Build<Candidate>()
                .Create();

            _newApplication = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.CandidateId, _candidateId)
                .With(fixture => fixture.AdditionalQuestion1Answer, AdditionalQuestion1Answer)
                .With(fixture => fixture.AdditionalQuestion2Answer, AdditionalQuestion2Answer)
                .With(fixture => fixture.CandidateInformation, _newCandidateInformation)
                .With(fixture => fixture.Vacancy, new Fixture()
                    .Build<ApprenticeshipSummary>()
                    .With(fixture => fixture.Id, VacancyId)
                    .Create())
                .Create();
        }

        [Test]
        public void ShouldSaveApplication()
        {
            // Arrange.
            _apprenticeshipApplicationReadRepository
                .Setup(mock => mock.GetForCandidate(_candidateId, VacancyId, true))
                .Returns(_oldApplication);

            _apprenticeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_oldApplication))
                .Returns(_oldApplication);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _newApplication);

            // Assert.
            savedApplication.Should().NotBeNull();
            savedApplication.Should().Be(_oldApplication);

            _apprenticeshipApplicationReadRepository
                .Verify(mock => mock.GetForCandidate(_candidateId, VacancyId, true), Times.Once);

            _apprenticeshipApplicationWriteRepository
                .Verify(mock => mock.Save(_oldApplication), Times.Once);
        }

        [Test]
        public void ShouldUpdateAdditionalQuestionAnswers()
        {
            // Arrange.
            _apprenticeshipApplicationReadRepository
                .Setup(mock => mock.GetForCandidate(_candidateId, VacancyId, true))
                .Returns(_oldApplication);

            _apprenticeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_oldApplication))
                .Returns(_oldApplication);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _newApplication);

            // Assert.
            savedApplication.Should().NotBeNull();
            savedApplication.AdditionalQuestion1Answer.Should().Be(AdditionalQuestion1Answer);
            savedApplication.AdditionalQuestion2Answer.Should().Be(AdditionalQuestion2Answer);
        }

        [Test]
        public void ShouldUpdateCandidateInformation()
        {
            // Arrange.
            _apprenticeshipApplicationReadRepository
                .Setup(mock => mock.GetForCandidate(_candidateId, VacancyId, true))
                .Returns(_oldApplication);

            _apprenticeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_oldApplication))
                .Returns(_oldApplication);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _newApplication);

            // Assert.
            savedApplication.Should().NotBeNull();
            savedApplication.CandidateInformation.Should().Be(_newCandidateInformation);
        }

        [Test]
        public void ShouldSyncToCandidateApplicationTemplate()
        {
            // Arrange.
            _apprenticeshipApplicationReadRepository
                .Setup(mock => mock.GetForCandidate(_candidateId, VacancyId, true))
                .Returns(_oldApplication);

            _apprenticeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_oldApplication))
                .Returns(_oldApplication);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _newApplication);

            // Assert.
            savedApplication.Should().NotBeNull();

            _candidateReadRepository.
                Verify(mock => mock.Get(_candidateId), Times.Once);

            _candidateWriteRepository.
                Verify(mock => mock.Save(_candidate), Times.Once);

            _candidate.ApplicationTemplate.Should().NotBeNull();
            _candidate.ApplicationTemplate.AboutYou.Should().Be(_newCandidateInformation.AboutYou);
            _candidate.ApplicationTemplate.EducationHistory.Should().Be(_newCandidateInformation.EducationHistory);
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
            _newApplication.CandidateInformation.DisabilityStatus = applicationDisabilityStatus;
            _candidate.MonitoringInformation.DisabilityStatus = originalCandidateDisabilityStatus;

            _apprenticeshipApplicationReadRepository
                .Setup(mock => mock.GetForCandidate(_candidateId, VacancyId, true))
                .Returns(_oldApplication);

            _apprenticeshipApplicationWriteRepository
                .Setup(mock => mock.Save(_oldApplication))
                .Returns(_oldApplication);

            _candidateReadRepository
                .Setup(mock => mock.Get(_candidateId))
                .Returns(_candidate);

            // Act.
            var savedApplication = _strategy.SaveApplication(_candidateId, VacancyId, _newApplication);

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