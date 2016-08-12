namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels.Account;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class DeleteSavedSearchTests
    {
        [Test]
        public void HasError()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var savedSearchId = Guid.NewGuid();

            var candidateService = new Mock<ICandidateServiceProvider>();

            candidateService.Setup(cs => cs.DeleteSavedSearch(candidateId, savedSearchId))
                .Returns(new SavedSearchViewModel {ViewModelMessage = "Error"});

            var mediator = new AccountMediatorBuilder().With(candidateService).Build();

            // Act.
            var response = mediator.DeleteSavedSearch(candidateId, savedSearchId);

            // Assert.
            response.AssertMessage(AccountMediatorCodes.DeleteSavedSearch.HasError,
                AccountPageMessages.DeleteSavedSearchFailed, UserMessageLevel.Error, true);
            candidateService.Verify(cs => cs.DeleteSavedSearch(candidateId, savedSearchId), Times.Once);
        }

        [Test]
        public void Ok()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var savedSearchId = Guid.NewGuid();

            var candidateService = new Mock<ICandidateServiceProvider>();
            candidateService.Setup(cs => cs.DeleteSavedSearch(candidateId, savedSearchId))
                .Returns(new SavedSearchViewModel());
            var mediator = new AccountMediatorBuilder().With(candidateService).Build();

            // Act.
            var response = mediator.DeleteSavedSearch(candidateId, savedSearchId);

            // Assert.
            response.AssertCode(AccountMediatorCodes.DeleteSavedSearch.Ok, true);
            candidateService.Verify(cs => cs.DeleteSavedSearch(candidateId, savedSearchId), Times.Once);
        }
    }
}