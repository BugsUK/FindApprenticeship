﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using System;
    using Candidate.Mediators.Register;
    using Candidate.ViewModels.Register;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResetPasswordTests : RegisterBaseTests
    {
        private const string ValidPassword = "?Password01!";
        private const string ValidPasswordResetCode = "ABC123";
        private const string VaildEmailAddress = "validEmailAddress@gmail.com";
        private const string InvalidPasswordResetCode = "invalidPasswordResetCode";
        private const string ErrorMessage = "Some error message";

        private static PasswordResetViewModel GetValidPasswordResetViewModel()
        {
            var resetPasswordViewModel = new PasswordResetViewModel
            {
                PasswordResetCode = ValidPasswordResetCode,
                EmailAddress = VaildEmailAddress,
                Password = ValidPassword,
                ConfirmPassword = ValidPassword
            };
            return resetPasswordViewModel;
        }

        [Test]
        public void ErrorVerifyingPassword()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            _candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel
                {
                    ViewModelMessage = ErrorMessage
                });

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertMessage(RegisterMediatorCodes.ResetPassword.FailedToResetPassword, ErrorMessage,
                UserMessageLevel.Warning, true);
        }

        [Test]
        public void InvalidResetCode()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            _candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel
                {
                    IsPasswordResetCodeValid = false
                });

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertValidationResult(RegisterMediatorCodes.ResetPassword.InvalidResetCode);
        }

        [Test]
        public void SuccessfullResetPassword()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            _candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel { IsPasswordResetCodeValid = true });

            _candidateServiceProvider.Setup(gc => gc.GetCandidate(It.IsAny<string>()))
                .Returns(new Candidate() {EntityId = Guid.NewGuid()});

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertMessage(RegisterMediatorCodes.ResetPassword.SuccessfullyResetPassword,
                PasswordResetPageMessages.SuccessfulPasswordReset, UserMessageLevel.Success, true);
        }

        [Test]
        public void UserLocked()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            _candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel
                {
                    UserStatus = UserStatuses.Locked
                });

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertCode(RegisterMediatorCodes.ResetPassword.UserAccountLocked, true);
        }

        [Test]
        public void ValidationFailure()
        {
            var resetPasswordViewModel = new PasswordResetViewModel
            {
                PasswordResetCode = InvalidPasswordResetCode
            };

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertValidationResult(RegisterMediatorCodes.ResetPassword.FailedValidation, true);
        }

        [Test]
        public void UserUnactivated()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            _candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel { UserStatus = UserStatuses.PendingActivation, IsPasswordResetCodeValid = true });

            _candidateServiceProvider.Setup(gc => gc.GetCandidate(It.IsAny<string>()))
                .Returns(new Candidate() { EntityId = Guid.NewGuid() });

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertMessage(RegisterMediatorCodes.ResetPassword.SuccessfullyResetPassword,
                PasswordResetPageMessages.SuccessfulPasswordReset, UserMessageLevel.Success, true);
        }
    }
}