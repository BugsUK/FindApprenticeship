using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using Application.Interfaces.Candidates;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Providers.AccountProvider;
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

        [Test]
        public void UserStatusChangeSuccess()
        {
            var candidateId = new Guid();
            var candidate = new Candidate
            {
                EntityId = candidateId
            };
            var user = new User
            {
                Status = UserStatuses.Active,
                EntityId = candidateId
            };
            var candidateService = new Mock<ICandidateService>();
            var userReadRepository = new Mock<IUserReadRepository>();

            candidateService
                .Setup(cs => cs.GetCandidate(candidateId))
                .Returns(candidate);

            userReadRepository.Setup(ur => ur.Get(candidate.EntityId)).Returns(user);

            candidateService
                .Setup(cs => cs.SetCandidateDeletionPending(candidate))
                .Returns(true);

            var provider = new AccountProviderBuilder().With(candidateService).Build();

            var result = provider.SetUserAccountDeletionPending(candidateId);
            result.Should().BeTrue();
        }
    }
}