﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using System;
    using Candidate.Mediators.Register;
    using Candidate.ViewModels.Register;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ActivateTests : RegisterBaseTests
    {
        private const string SomeErrorMessage = "SomeErrorMessage";
        private const string ValidActivationCode = "ABC123";
        private const string ValidEmailAddress = "validEmailAddress@gmail.com";
        private const string InvalidActivationCode = "invalidActivationCode";

        [Test]
        public void ActivationError()
        {
            var activationViewModel = new ActivationViewModel
            {
                ActivationCode = ValidActivationCode,
                EmailAddress = ValidEmailAddress
            };

            _candidateServiceProvider.Setup(csp => csp.Activate(It.IsAny<ActivationViewModel>(), It.IsAny<Guid>()))
                .Returns(new ActivationViewModel
                {
                    State = ActivateUserState.Error,
                    ViewModelMessage = SomeErrorMessage
                });

            var response = _registerMediator.Activate(Guid.NewGuid(), activationViewModel);

            response.AssertMessage(RegisterMediatorCodes.Activate.ErrorActivating, SomeErrorMessage,
                UserMessageLevel.Error, true);
        }

        [Test]
        public void InvalidCode()
        {
            var activationViewModel = new ActivationViewModel
            {
                ActivationCode = ValidActivationCode,
                EmailAddress = ValidEmailAddress
            };

            _candidateServiceProvider.Setup(csp => csp.Activate(It.IsAny<ActivationViewModel>(), It.IsAny<Guid>()))
                .Returns(new ActivationViewModel
                {
                    State = ActivateUserState.InvalidCode
                });

            var response = _registerMediator.Activate(Guid.NewGuid(), activationViewModel);

            response.AssertValidationResult(RegisterMediatorCodes.Activate.InvalidActivationCode, true);
        }

        [Test]
        public void UserActivated()
        {
            var activationViewModel = new ActivationViewModel
            {
                ActivationCode = ValidActivationCode,
                EmailAddress = ValidEmailAddress
            };

            _candidateServiceProvider.Setup(csp => csp.Activate(It.IsAny<ActivationViewModel>(), It.IsAny<Guid>()))
                .Returns(new ActivationViewModel
                {
                    State = ActivateUserState.Activated
                });

            var response = _registerMediator.Activate(Guid.NewGuid(), activationViewModel);

            response.AssertMessage(RegisterMediatorCodes.Activate.SuccessfullyActivated,
                ActivationPageMessages.AccountActivated, UserMessageLevel.Success, true);
        }

        [Test]
        public void ValidationError()
        {
            var activationViewModel = new ActivationViewModel
            {
                ActivationCode = InvalidActivationCode
            };
            var response = _registerMediator.Activate(Guid.NewGuid(), activationViewModel);

            response.AssertValidationResult(RegisterMediatorCodes.Activate.FailedValidation, true);
        }
    }
}