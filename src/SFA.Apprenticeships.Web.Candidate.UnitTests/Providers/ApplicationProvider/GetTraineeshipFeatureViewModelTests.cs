namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Domain.Entities.Applications;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class GetTraineeshipFeatureViewModelTests// : ApprenticeshipApplicationProviderTestsBase
    {
        [Test]
        public void GivenException_ThenExceptionIsRethrown()
        {
            var candidateId = Guid.NewGuid();
            var candidateApplicationService = new Mock<ICandidateApplicationService>();

            candidateApplicationService.Setup(cs => cs.GetApprenticeshipApplications(candidateId, true)).Throws<Exception>();

            Action action = () => new ApprenticeshipApplicationProviderBuilder()
                .With(candidateApplicationService).Build()
                .GetTraineeshipFeatureViewModel(candidateId);;

            action.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenSuccess_ThenViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateApplicationService = new Mock<ICandidateApplicationService>();

            candidateApplicationService.Setup(cs => cs.GetApprenticeshipApplications(candidateId, true)).Returns(new ApprenticeshipApplicationSummary[0]);
            candidateApplicationService.Setup(cs => cs.GetTraineeshipApplications(candidateId)).Returns(new TraineeshipApplicationSummary[0]);
            candidateApplicationService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).Build());

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(candidateApplicationService).Build()
                .GetTraineeshipFeatureViewModel(candidateId);
            returnedViewModel.Should().NotBeNull();
        }
    }
}