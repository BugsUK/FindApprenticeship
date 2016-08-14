namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels.Account;
    using Common.Constants;
    using Constants.Pages;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class UpdateEmailAddressTests
    {
        [Test]
        public void UpdateEmailAddressOk()
        {
            var emailViewModel = new EmailViewModel()
            {
                EmailAddress = "krister.bone@gmail.com",
                UpdateStatus = UpdateEmailStatus.Ok
            };
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.UpdateEmailAddress(It.IsAny<Guid>(), It.IsAny<EmailViewModel>())).Returns(emailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.UpdateEmailAddress(Guid.NewGuid(), emailViewModel);

            response.Code.Should().Be(AccountMediatorCodes.UpdateEmailAddress.Ok);
            response.ViewModel.Should().Be(emailViewModel);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(UpdateEmailAddressMessages.VerificationCodeSent);
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }

        [Test]
        public void UpdateEmailAddressValidationFailed()
        {
            var emailViewModel = new EmailViewModel()
            {
                EmailAddress = "invalidemail"
            };
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.UpdateEmailAddress(It.IsAny<Guid>(), It.IsAny<EmailViewModel>())).Returns(emailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.UpdateEmailAddress(Guid.NewGuid(), emailViewModel);

            response.Code.Should().Be(AccountMediatorCodes.UpdateEmailAddress.ValidationError);
            response.ValidationResult.Should().NotBeNull();
            response.ValidationResult.Errors.Should().NotBeNull();
            response.ValidationResult.Errors.Count.Should().Be(1);
            response.ValidationResult.Errors[0].ErrorMessage.Should().Be("Email address " + Whitelists.EmailAddressWhitelist.ErrorText);
            response.ViewModel.Should().Be(emailViewModel);
            response.Message.Should().BeNull();
        }

        [Test]
        public void UpdateEmailAddressHasError()
        {
            var emailViewModel = new EmailViewModel()
            {
                EmailAddress = "krister.bone@gmail.com",
                UpdateStatus = UpdateEmailStatus.Error,
                ViewModelMessage = UpdateEmailStatus.Error.ToString()
            };
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.UpdateEmailAddress(It.IsAny<Guid>(), It.IsAny<EmailViewModel>())).Returns(emailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.UpdateEmailAddress(Guid.NewGuid(), emailViewModel);

            response.Code.Should().Be(AccountMediatorCodes.UpdateEmailAddress.HasError);
            response.ViewModel.Should().Be(emailViewModel);
            response.Message.Text.Should().Be(UpdateEmailAddressMessages.UpdateEmailAddressError);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }
    }
}
