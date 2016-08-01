namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using Candidate.Mediators.Login;
    using Candidate.Providers;
    using Candidate.ViewModels.Register;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ForgottenEmailTests
    {
        [Test]
        public void EmailSent()
        {
            const string phoneNumber = "0123456789";

            var viewModel = new ForgottenCredentialsViewModel
            {
                ForgottenEmailViewModel = new ForgottenEmailViewModel
                {
                    PhoneNumber = phoneNumber
                }
            };

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(csp => csp.RequestEmailReminder(It.IsAny<ForgottenEmailViewModel>()))
                .Returns(true);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.ForgottenEmail(viewModel);

            var message = string.Format(LoginPageMessages.ForgottenEmailSent, phoneNumber);
            response.AssertMessage(LoginMediatorCodes.ForgottenEmail.EmailSent, message, UserMessageLevel.Success, true);
        }

        [Test]
        public void FailedToSendEmail()
        {
            const string phoneNumber = "0123456789";

            var viewModel = new ForgottenCredentialsViewModel
            {
                ForgottenEmailViewModel = new ForgottenEmailViewModel
                {
                    PhoneNumber = phoneNumber
                }
            };

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(csp => csp.RequestEmailReminder(It.IsAny<ForgottenEmailViewModel>()))
                .Returns(false);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.ForgottenEmail(viewModel);

            var message = string.Format(LoginPageMessages.ForgottenEmailSent, phoneNumber);
            response.AssertMessage(LoginMediatorCodes.ForgottenEmail.FailedToSendEmail, message,
                UserMessageLevel.Success, true);
        }

        [Test]
        public void FailedValidation()
        {
            const string phoneNumber = "NotANumber";

            var viewModel = new ForgottenCredentialsViewModel
            {
                ForgottenEmailViewModel = new ForgottenEmailViewModel
                {
                    PhoneNumber = phoneNumber
                }
            };

            var mediator = new LoginMediatorBuilder().Build();

            var response = mediator.ForgottenEmail(viewModel);

            response.AssertValidationResult(LoginMediatorCodes.ForgottenEmail.FailedValidation);
        }
    }
}