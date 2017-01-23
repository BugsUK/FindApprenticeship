namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Communication.Candidate
{
    using System;
    using Application.Interfaces.Communications;
    using Common.Configuration;
    using SFA.Infrastructure.Interfaces;
    using Common.IoC;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Infrastructure.Communication.IoC;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    using StructureMap;
    using ErrorCodes = Application.Interfaces.Communications.ErrorCodes;

    [TestFixture]
    public class CandidateEmailTests
    {
        private const string TestToEmail = "valtechnas@gmail.com";

        private IEmailDispatcher _dispatcher;

        private readonly Mock<ILogService> _logServiceMock = new Mock<ILogService>();

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            container.Configure(c => c.For<ILogService>().Use(_logServiceMock.Object));

            _dispatcher = container.GetInstance<IEmailDispatcher>("SendGridEmailDispatcher");
            _logServiceMock.ResetCalls();
        }

        // TODO: AG: why are there 3 apparently identical tests here.

        [Test, Category("Integration")]
        public void ShoudSendEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateActivationEmailTokens(),
                MessageType = MessageTypes.SendActivationCode
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShoudSendEmailWithFromEmailInTemplateConfiguration()
        {
            // NOTE: FromEmail is not set and is defined in SendGrid email template.
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateActivationEmailTokens(),
                MessageType = MessageTypes.SendActivationCode
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShoudSendEmailWithSubjectInTemplate()
        {
            // NOTE: Subject is not set and is defined in SendGrid email template.
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateActivationEmailTokens(),
                MessageType = MessageTypes.SendActivationCode
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldSendAccountUnlockCodeEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateAccountUnlockCodeTokens(),
                MessageType = MessageTypes.SendAccountUnlockCode
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldSendApprenticeshipApplicationSubmittedEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateApprenticeshipApplicationSubmittedTokens(),
                MessageType = MessageTypes.ApprenticeshipApplicationSubmitted
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldSendTraineeshipApplicationSubmittedEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateTraineeshipApplicationSubmittedTokens(),
                MessageType = MessageTypes.TraineeshipApplicationSubmitted
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldSendPasswordResetCodeEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreatePasswordResetTokens(),
                MessageType = MessageTypes.SendPasswordResetCode
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldSendPasswordResetConfirmationEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreatePasswordResetConfirmationTokens(),
                MessageType = MessageTypes.PasswordChanged
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldSendDailyDigestEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateDailyDigestTokens(4),
                MessageType = MessageTypes.DailyDigest
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldSendContactUsMessageEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateContactUsMessageTokensWithDetails("UserEnquiryDetails"),
                MessageType = MessageTypes.CandidateContactUsMessage
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldNotSendContactMessageEmailWithBlankEnquiryDetails()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateContactUsMessageTokensWithDetails(string.Empty),
                MessageType = MessageTypes.CandidateContactUsMessage
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldNotSendContactMessageEmailWithNullEnquiryDetails()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateContactUsMessageTokensWithDetails(null),
                MessageType = MessageTypes.CandidateContactUsMessage
            };

            Action sendEmail = () => _dispatcher.SendEmail(request);

            sendEmail.ShouldThrow<CustomException>().Which.Code.Should().Be(ErrorCodes.EmailFormatError);
            VerifyErrorsLogged(Times.Once());
        }

        [Test, Category("Integration")]
        public void ShouldSendFeedbackMessageEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateFeedbackTokens(),
                MessageType = MessageTypes.CandidateFeedbackMessage
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }


        [Test, Category("Integration")]
        public void ShouldSendSavedSearchAlertsEmailWithoutSubCategories()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateSavedSearchAlertTokens(5),
                MessageType = MessageTypes.SavedSearchAlert
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldSendSavedSearchAlertsEmailWithSubCategories()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateSavedSearchAlertTokens(5, true),
                MessageType = MessageTypes.SavedSearchAlert
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        [Test, Category("Integration")]
        public void ShouldSendPendingUsernameCodeEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = CandidateEmailTokenGenerator.CreateSendPendingUsernameCodeTokens(),
                MessageType = MessageTypes.SendPendingUsernameCode
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        #region Helpers

        private void VerifyErrorsLogged(Times times)
        {
            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<object[]>()), times);
        }

        #endregion
    }
}