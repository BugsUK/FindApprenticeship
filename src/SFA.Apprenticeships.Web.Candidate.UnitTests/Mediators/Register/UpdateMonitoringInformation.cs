namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using System;
    using Candidate.Mediators.Register;
    using Candidate.ViewModels.Candidate;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Constants.Pages;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class UpdateMonitoringInformation : RegisterBaseTests
    {
        [Test]
        public void Fail()
        {
            _candidateServiceProvider.Setup(
                x => x.UpdateMonitoringInformation(It.IsAny<Guid>(), It.IsAny<MonitoringInformationViewModel>()))
                .Throws<Exception>();
            var response = _registerMediator.UpdateMonitoringInformation(Guid.NewGuid(),
                new MonitoringInformationViewModel());
            response.Code.Should().Be(RegisterMediatorCodes.UpdateMonitoringInformation.ErrorUpdating);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(ActivationPageMessages.UpdatingMonitoringInformationFailure);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }

        [Test]
        public void FailValidation()
        {
            var viewModel = new MonitoringInformationViewModel
            {
                AnythingWeCanDoToSupportYourInterview = new string('X', 9999)
            };

            var response = _registerMediator.UpdateMonitoringInformation(Guid.NewGuid(), viewModel);
            response.AssertValidationResult(RegisterMediatorCodes.UpdateMonitoringInformation.FailedValidation);
        }

        [Test]
        public void Success()
        {
            var response = _registerMediator.UpdateMonitoringInformation(Guid.NewGuid(),
                new MonitoringInformationViewModel());
            response.Code.Should().Be(RegisterMediatorCodes.UpdateMonitoringInformation.SuccessfullyUpdated);
            response.Message.Should().BeNull();
        }
    }
}