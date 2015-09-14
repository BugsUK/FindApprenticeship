using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using Candidate.Mediators.Login;
    using Candidate.Providers;
    using Candidate.ViewModels.Register;
    using Common.Constants;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ForgottenPasswordTests
    {
        private const string InvalidEmailAddress = "invalidEmailAddress";
        private const string ValidEmailAddress = "ValidEmailAddress@gmail.com";

        [Test]
        public void PasswordNotSent()
        {
            var forgottenPasswordViewModel = new ForgottenPasswordViewModel
            {
                EmailAddress = ValidEmailAddress
            };

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(csp => csp.RequestForgottenPasswordResetCode(It.IsAny<ForgottenPasswordViewModel>())).Returns(false);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.ForgottenPassword(new ForgottenCredentialsViewModel { ForgottenPasswordViewModel = forgottenPasswordViewModel });

            response.AssertMessage(LoginMediatorCodes.ForgottenPassword.FailedToSendResetCode,
                PasswordResetPageMessages.FailedToSendPasswordResetCode, UserMessageLevel.Warning, true);
        }

        [Test]
        public void PasswordSent()
        {
            var forgottenPasswordViewModel = new ForgottenPasswordViewModel
            {
                EmailAddress = ValidEmailAddress
            };

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(csp => csp.RequestForgottenPasswordResetCode(It.IsAny<ForgottenPasswordViewModel>())).Returns(true);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.ForgottenPassword(new ForgottenCredentialsViewModel { ForgottenPasswordViewModel = forgottenPasswordViewModel });

            response.AssertCode(LoginMediatorCodes.ForgottenPassword.PasswordSent, true);
        }

        [Test]
        public void ValidationErrors()
        {
            var forgottenPasswordViewModel = new ForgottenPasswordViewModel
            {
                EmailAddress = InvalidEmailAddress
            };

            var mediator = new LoginMediatorBuilder().Build();

            var response = mediator.ForgottenPassword(new ForgottenCredentialsViewModel { ForgottenPasswordViewModel = forgottenPasswordViewModel });

            response.AssertValidationResult(LoginMediatorCodes.ForgottenPassword.FailedValidation, true);
        }
    }
}