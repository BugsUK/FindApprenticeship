using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Moq;
    using NUnit.Framework;
    using System;

    [TestFixture]
    [Parallelizable]
    public class SetAccountSettingsToDeleteTests
    {
        [Test]
        public void SuccessTest()
        {
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.SetUserAccountDeletionPending(It.IsAny<Guid>())).Returns(true);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.SetAccountStatusToDelete(Guid.NewGuid());
            response.AssertCodeAndMessage(AccountMediatorCodes.Settings.Success);
        }

        public void FailureTest()
        {
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.SetUserAccountDeletionPending(It.IsAny<Guid>())).Returns(false);
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.SetAccountStatusToDelete(Guid.NewGuid());
            response.AssertCodeAndMessage(AccountMediatorCodes.Settings.SaveError);
        }
    }
}