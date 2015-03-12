namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels.Account;
    using Common.Constants;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DeleteSavedSearchTests
    {
        [Test]
        public void Ok()
        {
            var savedSearchId = Guid.NewGuid();

            var candidateService = new Mock<ICandidateServiceProvider>();
            candidateService.Setup(cs => cs.DeleteSavedSearch(savedSearchId)).Returns(new SavedSearchViewModel());
            var mediator = new AccountMediatorBuilder().With(candidateService).Build();

            var response = mediator.DeleteSavedSearch(savedSearchId);

            response.AssertCode(AccountMediatorCodes.DeleteSavedSearch.Ok, true);
            candidateService.Verify(cs => cs.DeleteSavedSearch(savedSearchId), Times.Once);
        }

        [Test]
        public void HasError()
        {
            var savedSearchId = Guid.NewGuid();

            var candidateService = new Mock<ICandidateServiceProvider>();
            candidateService.Setup(cs => cs.DeleteSavedSearch(savedSearchId)).Returns(new SavedSearchViewModel { ViewModelMessage = "Error"});
            var mediator = new AccountMediatorBuilder().With(candidateService).Build();

            var response = mediator.DeleteSavedSearch(savedSearchId);

            response.AssertMessage(AccountMediatorCodes.DeleteSavedSearch.HasError, AccountPageMessages.DeleteSavedSearchFailed, UserMessageLevel.Error, true);
            candidateService.Verify(cs => cs.DeleteSavedSearch(savedSearchId), Times.Once);
        }
    }
}