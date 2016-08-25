namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using Application.Interfaces.Candidates;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels.Account;
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using System;

    [TestFixture]
    [Parallelizable]
    public class VerifyAccountSettingsTests
    {
        [Test]
        public void SaveValidationErrorTest()
        {
            var settingsViewModel = new SettingsViewModel();
            var deleteAccountSettingsViewModel = new DeleteAccountSettingsViewModel();
            var accountProvider = new Mock<IAccountProvider>();
            accountProvider.Setup(x => x.GetSettingsViewModel(It.IsAny<Guid>())).Returns(new SettingsViewModel());
            var accountMediator = new AccountMediatorBuilder().With(accountProvider.Object).Build();
            deleteAccountSettingsViewModel.EmailAddress = settingsViewModel.EmailAddress;
            deleteAccountSettingsViewModel.Password = settingsViewModel.Password;
            var response = accountMediator.VerifyAccountSettings(Guid.NewGuid(), deleteAccountSettingsViewModel);
            response.Code.Should().Be(AccountMediatorCodes.ValidateUserAccountBeforeDelete.ValidationError);
            response.ValidationResult.Should().NotBeNull();
        }

        [Test]
        public void MatchAndAuthenticateUserCredentialsTest()
        {
            var candidateId = Guid.NewGuid();
            var deleteAccountSettingsViewModel = new DeleteAccountSettingsViewModel
            {
                EmailAddress = "test.one@gmail.com",
                Password = "P@ssw0rd"
            };
            var candidate = new Candidate
            {
                EntityId = candidateId
            };
            var candidateService = new Mock<ICandidateService>();

            candidateService
                .Setup(cs => cs.Authenticate(deleteAccountSettingsViewModel.EmailAddress, deleteAccountSettingsViewModel.Password))
                .Returns(candidate);

            var accountMediator = new AccountMediatorBuilder().With(candidateService).Build();

            var response = accountMediator.VerifyAccountSettings(candidateId, deleteAccountSettingsViewModel);
            response.Code.Should().Be(AccountMediatorCodes.ValidateUserAccountBeforeDelete.Ok);
        }
    }
}