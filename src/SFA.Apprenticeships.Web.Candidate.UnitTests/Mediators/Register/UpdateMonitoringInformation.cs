namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using System;
    using System.Linq;
    using Candidate.Mediators.Register;
    using Candidate.ViewModels;
    using Candidate.ViewModels.Candidate;
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Constants.Pages;
    using Constants.ViewModels;
    using Candidate.ViewModels.Register;
    using Common.Constants;

    [TestFixture]
    public class UpdateMonitoringInformation : RegisterBaseTests
    {
        [Test]
        public void Success()
        {
            var response = _registerMediator.UpdateMonitoringInformation(Guid.NewGuid(), new MonitoringInformationViewModel());
            response.Code.Should().Be(RegisterMediatorCodes.UpdateMonitoringInformation.SuccessfullyUpdated);
            response.Message.Should().BeNull();
        }

        [Test]
        public void Fail()
        {
            _candidateServiceProvider.Setup(x => x.UpdateMonitoringInformation(It.IsAny<Guid>(), It.IsAny<MonitoringInformationViewModel>())).Throws<Exception>();
            var response = _registerMediator.UpdateMonitoringInformation(Guid.NewGuid(), new MonitoringInformationViewModel());
            response.Code.Should().Be(RegisterMediatorCodes.UpdateMonitoringInformation.ErrorUpdating);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(ActivationPageMessages.UpdatingMonitoringInformationFailure);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }

        [Test]
        public void FailValidation()
        {
            var viewModel = new MonitoringInformationViewModel { RequiresSupportForInterview = true };
            var response = _registerMediator.UpdateMonitoringInformation(Guid.NewGuid(), viewModel);
            response.AssertValidationResult(RegisterMediatorCodes.UpdateMonitoringInformation.FailedValidation);
        }
    }
}