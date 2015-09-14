using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using System;
    using Candidate.Mediators.Login;
    using Candidate.Mediators.Register;
    using Candidate.Providers;
    using Candidate.ViewModels.Register;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResetPasswordTests
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

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel
                {
                    ViewModelMessage = ErrorMessage
                });
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.ResetPassword(resetPasswordViewModel);

            response.AssertMessage(LoginMediatorCodes.ResetPassword.FailedToResetPassword, ErrorMessage,
                UserMessageLevel.Warning, true);
        }

        [Test]
        public void InvalidResetCode()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel
                {
                    IsPasswordResetCodeValid = false
                });
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.ResetPassword(resetPasswordViewModel);

            response.AssertValidationResult(LoginMediatorCodes.ResetPassword.InvalidResetCode);
        }

        [Test]
        public void SuccessfullResetPassword()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>())).Returns(new PasswordResetViewModel { IsPasswordResetCodeValid = true });
            candidateServiceProvider.Setup(gc => gc.GetCandidate(It.IsAny<string>())).Returns(new Candidate() { EntityId = Guid.NewGuid() });
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.ResetPassword(resetPasswordViewModel);

            response.AssertMessage(LoginMediatorCodes.ResetPassword.SuccessfullyResetPassword,
                PasswordResetPageMessages.SuccessfulPasswordReset, UserMessageLevel.Success, true);
        }

        [Test]
        public void UserLocked()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel
                {
                    UserStatus = UserStatuses.Locked
                });
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.ResetPassword(resetPasswordViewModel);

            response.AssertCode(LoginMediatorCodes.ResetPassword.UserAccountLocked, true);
        }

        [Test]
        public void ValidationFailure()
        {
            var resetPasswordViewModel = new PasswordResetViewModel
            {
                PasswordResetCode = InvalidPasswordResetCode
            };

            var mediator = new LoginMediatorBuilder().Build();

            var response = mediator.ResetPassword(resetPasswordViewModel);

            response.AssertValidationResult(LoginMediatorCodes.ResetPassword.FailedValidation, true);
        }

        [Test]
        public void UserUnactivated()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>())).Returns(new PasswordResetViewModel { UserStatus = UserStatuses.PendingActivation, IsPasswordResetCodeValid = true });
            candidateServiceProvider.Setup(gc => gc.GetCandidate(It.IsAny<string>())).Returns(new Candidate { EntityId = Guid.NewGuid() });
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.ResetPassword(resetPasswordViewModel);

            response.AssertMessage(LoginMediatorCodes.ResetPassword.SuccessfullyResetPassword,
                PasswordResetPageMessages.SuccessfulPasswordReset, UserMessageLevel.Success, true);
        }
    }
}