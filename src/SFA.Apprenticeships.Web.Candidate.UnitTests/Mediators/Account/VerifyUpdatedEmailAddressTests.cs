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
    public class VerifyUpdatedEmailAddressTests
    {
        [Test]
        public void VerifyUpdatedEmailAddressOk()
        {
            var verifyUpdatedEmailViewModel = new VerifyUpdatedEmailViewModel { PendingUsernameCode = "ABC123", VerifyPassword = "Password01" };
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.VerifyUpdatedEmailAddress(It.IsAny<Guid>(), It.IsAny<VerifyUpdatedEmailViewModel>())).Returns(verifyUpdatedEmailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.VerifyUpdatedEmailAddress(Guid.NewGuid(), verifyUpdatedEmailViewModel);

            response.Code.Should().Be(AccountMediatorCodes.VerifyUpdatedEmailAddress.Ok);
            response.ViewModel.Should().Be(verifyUpdatedEmailViewModel);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(UpdateEmailAddressMessages.UpdatedEmailSuccess);
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }

        [Test]
        public void VerifyUpdatedEmailAddressHasError()
        {
            var verifyUpdatedEmailViewModel = new VerifyUpdatedEmailViewModel { PendingUsernameCode = "ABC123", VerifyPassword = "Password01", ViewModelMessage = "Error" };
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.VerifyUpdatedEmailAddress(It.IsAny<Guid>(), It.IsAny<VerifyUpdatedEmailViewModel>())).Returns(verifyUpdatedEmailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.VerifyUpdatedEmailAddress(Guid.NewGuid(), verifyUpdatedEmailViewModel);

            response.Code.Should().Be(AccountMediatorCodes.VerifyUpdatedEmailAddress.HasError);
            response.ViewModel.Should().Be(verifyUpdatedEmailViewModel);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(UpdateEmailAddressMessages.UpdateEmailAddressError);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }

        [Test]
        public void VerifyUpdatedEmailAddressFailedValidation()
        {
            var verifyUpdatedEmailViewModel = new VerifyUpdatedEmailViewModel { PendingUsernameCode = "ABC123", VerifyPassword = "" };
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.VerifyUpdatedEmailAddress(It.IsAny<Guid>(), It.IsAny<VerifyUpdatedEmailViewModel>())).Returns(verifyUpdatedEmailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.VerifyUpdatedEmailAddress(Guid.NewGuid(), verifyUpdatedEmailViewModel);

            response.Code.Should().Be(AccountMediatorCodes.VerifyUpdatedEmailAddress.ValidationError);
            response.ViewModel.Should().Be(verifyUpdatedEmailViewModel);
            response.ValidationResult.Errors.Should().NotBeNull();
            response.ValidationResult.Errors.Count.Should().Be(3);
            response.Message.Should().BeNull();
        }
    }
}
