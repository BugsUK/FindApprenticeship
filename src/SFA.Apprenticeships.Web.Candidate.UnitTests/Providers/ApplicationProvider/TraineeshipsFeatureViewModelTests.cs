namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.ReferenceData;
    using Candidate.Providers;
    using Common.Configuration;
    using Common.Providers;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    [TestFixture]
    public class TraineeshipsFeatureViewModelTests
    {
        const int UnsuccessfulApplications = 3;

        private Mock<ILogService> _logService;
        //private Mock<ICandidateService> _candidateService;
        private Mock<ICandidateApplicationService> _candidateApplicationService;
        private Mock<IConfigurationService> _configurationService;
        private Mock<IUserDataProvider> _userDataProvider;
        private Mock<IReferenceDataService> _referenceDataService;

        private ApprenticeshipApplicationProvider _apprenticeshipApplicationProvider;

        [SetUp]
        public void SetUp()
        {
            _logService = new Mock<ILogService>();
            var candidateService = new Mock<ICandidateService>();
            _candidateApplicationService = new Mock<ICandidateApplicationService>();
            _configurationService = new Mock<IConfigurationService>();
            _userDataProvider = new Mock<IUserDataProvider>();
            _referenceDataService = new Mock<IReferenceDataService>();

            _configurationService.Setup(cm => cm.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { UnsuccessfulApplicationsToShowTraineeshipsPrompt = UnsuccessfulApplications });

            var candidateApplicationsProvider = new CandidateApplicationsProvider(_candidateApplicationService.Object,
                _userDataProvider.Object, _configurationService.Object, _logService.Object);

            _apprenticeshipApplicationProvider = new ApprenticeshipApplicationProvider(null, candidateService.Object,
                null, _logService.Object, _referenceDataService.Object, candidateApplicationsProvider);
        }

        [Test]
        public void GivenAUserHasMoreThanNUnsuccessfulApplications_ShouldSeeTheTraineeshipsPrompt()
        {
            //Arrange
            _candidateApplicationService.Setup(cs => cs.GetCandidate(It.IsAny<Guid>()))
                .Returns(new Candidate());

            _candidateApplicationService.Setup(cs => cs.GetApprenticeshipApplications(It.IsAny<Guid>(), true)).
                Returns(GetUnsuccessfulApplicationSummaries(UnsuccessfulApplications));

            _candidateApplicationService.Setup(cs => cs.GetTraineeshipApplications(It.IsAny<Guid>()))
                .Returns(GetTraineeshipApplicationSummaries(0));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.TraineeshipFeature.ShowTraineeshipsPrompt.Should().BeTrue();
            results.TraineeshipFeature.ShowTraineeshipsLink.Should().BeTrue();
        }

        [Test]
        public void GivenAUserHasMoreThanNUnsuccessfulApplications_AndOneSuccessfulApplication_ShouldntSeeTheTraineeshipsPrompt()
        {
            //Arrange
            _candidateApplicationService.Setup(cs => cs.GetCandidate(It.IsAny<Guid>()))
                .Returns(new Candidate());

            var apprenticeshipApplicationSummaries = GetUnsuccessfulApplicationSummaries(UnsuccessfulApplications);
            apprenticeshipApplicationSummaries.AddRange(GetSuccessfulApplicationSummaries(1));
            _candidateApplicationService.Setup(cs => cs.GetApprenticeshipApplications(It.IsAny<Guid>(), true)).
                Returns(apprenticeshipApplicationSummaries);

            _candidateApplicationService.Setup(cs => cs.GetTraineeshipApplications(It.IsAny<Guid>()))
                .Returns(GetTraineeshipApplicationSummaries(0));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.TraineeshipFeature.ShowTraineeshipsPrompt.Should().BeFalse();
            results.TraineeshipFeature.ShowTraineeshipsLink.Should().BeTrue();
        }

        [Test]
        public void GivenAUserHasMoreThanNUnsuccessfulApplications_AndUserHasOptedNotToAllowTraineeshipsPrompt_ShouldntSeeTheTraineeshipsPrompt()
        {
            //Arrange
            _candidateApplicationService.Setup(cs => cs.GetCandidate(It.IsAny<Guid>()))
                .Returns(new Candidate
                {
                    CommunicationPreferences = new CommunicationPreferences
                    {
                        AllowTraineeshipPrompts = false
                    }
                });

            _candidateApplicationService.Setup(cs => cs.GetApprenticeshipApplications(It.IsAny<Guid>(), true)).
                Returns(GetUnsuccessfulApplicationSummaries(UnsuccessfulApplications));

            _candidateApplicationService.Setup(cs => cs.GetTraineeshipApplications(It.IsAny<Guid>()))
                .Returns(GetTraineeshipApplicationSummaries(0));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.TraineeshipFeature.ShowTraineeshipsPrompt.Should().BeFalse();
            results.TraineeshipFeature.ShowTraineeshipsLink.Should().BeTrue();
        }

        [Test]
        public void GivenAUserHasLessThanNUnsuccessfulApplications_ShouldntSeeTheTraineeshipsPrompt()
        {
            //Arrange
            const int unsuccessfulApplicationsThreshold = UnsuccessfulApplications - 1;

            _candidateApplicationService.Setup(cs => cs.GetCandidate(It.IsAny<Guid>()))
                .Returns(new Candidate());

            _candidateApplicationService.Setup(cs => cs.GetApprenticeshipApplications(It.IsAny<Guid>(), true))
                .Returns(GetUnsuccessfulApplicationSummaries(unsuccessfulApplicationsThreshold));

            _candidateApplicationService.Setup(cs => cs.GetTraineeshipApplications(It.IsAny<Guid>()))
                .Returns(GetTraineeshipApplicationSummaries(0));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.TraineeshipFeature.ShowTraineeshipsPrompt.Should().BeFalse();
            results.TraineeshipFeature.ShowTraineeshipsLink.Should().BeFalse();
        }

        [Test]
        public void GivenIveAppliedForAtLeastOneTraineeship_ShouldntSeeTheTraineeshipsPrompt()
        {
            //Arrange
            _candidateApplicationService.Setup(cs => cs.GetCandidate(It.IsAny<Guid>()))
                .Returns(new Candidate());

            _candidateApplicationService.Setup(cs => cs.GetApprenticeshipApplications(It.IsAny<Guid>(), true))
                .Returns(GetUnsuccessfulApplicationSummaries(UnsuccessfulApplications));

            _candidateApplicationService.Setup(cs => cs.GetTraineeshipApplications(It.IsAny<Guid>()))
                .Returns(GetTraineeshipApplicationSummaries(1));

            //Act
            var results = _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            results.TraineeshipFeature.ShowTraineeshipsPrompt.Should().BeFalse();
            results.TraineeshipFeature.ShowTraineeshipsLink.Should().BeTrue();
        }

        [Test]
        public void GivenException_ThenExceptionIsRethrown()
        {
            //Arrange
            _candidateApplicationService.Setup(cs => cs.GetApprenticeshipApplications(It.IsAny<Guid>(), true))
                .Returns(GetUnsuccessfulApplicationSummaries(UnsuccessfulApplications));

            _candidateApplicationService.Setup(cs => cs.GetTraineeshipApplications(It.IsAny<Guid>())).Throws<Exception>();

            //Act
            Action action = () => _apprenticeshipApplicationProvider.GetMyApplications(Guid.NewGuid());

            //Assert
            action.ShouldThrow<Exception>();
        }

        private static List<ApprenticeshipApplicationSummary> GetUnsuccessfulApplicationSummaries(int applicationSummariesCount)
        {
            return Enumerable.Range(1, applicationSummariesCount)
                .Select(i => new ApprenticeshipApplicationSummary {Status = ApplicationStatuses.Unsuccessful})
                .ToList();
        }

        private static List<ApprenticeshipApplicationSummary> GetSuccessfulApplicationSummaries(int applicationSummariesCount)
        {
            return Enumerable.Range(1, applicationSummariesCount)
                .Select(i => new ApprenticeshipApplicationSummary {Status = ApplicationStatuses.Successful})
                .ToList();
        }

        private static List<TraineeshipApplicationSummary> GetTraineeshipApplicationSummaries(int traineeshipSummariesCount)
        {
            if (traineeshipSummariesCount == 0) return new List<TraineeshipApplicationSummary>();

            return Enumerable.Range(1, traineeshipSummariesCount)
                .Select(i => new TraineeshipApplicationSummary())
                .ToList();
        }
    }
}
