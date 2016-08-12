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
    public class ResendUpdateEmailAddressCodeTests
    {
        [Test]
        public void ResendUpdateEmailAddressCodeOk()
        {
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.ResendUpdateEmailAddressCode(It.IsAny<Guid>()))
                .Returns(new VerifyUpdatedEmailViewModel());
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.ResendUpdateEmailAddressCode(Guid.NewGuid());

            response.Code.Should().Be(AccountMediatorCodes.ResendUpdateEmailAddressCode.Ok);
            response.ViewModel.Should().NotBeNull();
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(UpdateEmailAddressMessages.UpdateEmailCodeResent);
            response.Message.Level.Should().Be(UserMessageLevel.Success);
        }

        [Test]
        public void ResendUpdateEmailAddressCodeHasError()
        {
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.ResendUpdateEmailAddressCode(It.IsAny<Guid>()))
                .Returns(new VerifyUpdatedEmailViewModel("Error"));
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.ResendUpdateEmailAddressCode(Guid.NewGuid());

            response.Code.Should().Be(AccountMediatorCodes.ResendUpdateEmailAddressCode.HasError);
            response.ViewModel.Should().NotBeNull();
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(UpdateEmailAddressMessages.UpdateEmailAddressError);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }
    }
}
