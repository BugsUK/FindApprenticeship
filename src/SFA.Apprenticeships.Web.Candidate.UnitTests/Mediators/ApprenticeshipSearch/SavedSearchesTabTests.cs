namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using System;
    using System.Linq;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.ViewModels.Account;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    [Parallelizable]
    public class SavedSearchesTabTests
    {
        [TestCase(1)]
        [TestCase(42)]
        public void Ok_CandidateIsLoggedInAndHasSavedSearches(int savedSearchCount)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();

            var savedSearches = new Fixture()
                .Build<SavedSearchViewModel>()
                .CreateMany(savedSearchCount);

            candidateServiceProvider
                .Setup(p => p.GetSavedSearches(candidateId)).Returns(savedSearches);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            candidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var mediator = new ApprenticeshipSearchMediatorBuilder()
                .With(candidateServiceProvider).Build();

            // Act.
            var response = mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false);

            // Assert.
            candidateServiceProvider.Verify(p => p.GetSavedSearches(candidateId), Times.Once);
            response.ViewModel.SavedSearches.Count().Should().Be(savedSearchCount);
            response.ViewModel.SavedSearches.Should().BeInDescendingOrder(each => each.DateCreated);
        }

        [Test]
        public void Ok_CandidateIsLoggedInAndHasNoSavedSearches()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();

            var savedSearches = new SavedSearchViewModel[] { };

            candidateServiceProvider
                .Setup(p => p.GetSavedSearches(candidateId)).Returns(savedSearches);

            var candidate = new Domain.Entities.Candidates.Candidate
            {
                RegistrationDetails = new RegistrationDetails { Address = new Address { Postcode = "CANDIDATE POSTCODE" } }
            };

            candidateServiceProvider
                .Setup(p => p.GetCandidate(candidateId)).Returns(candidate);

            var mediator = new ApprenticeshipSearchMediatorBuilder()
                .With(candidateServiceProvider).Build();

            // Act.
            var response = mediator.Index(candidateId, ApprenticeshipSearchMode.SavedSearches, false);

            // Assert.
            candidateServiceProvider.Verify(p => p.GetSavedSearches(candidateId), Times.Once);
            response.ViewModel.SavedSearches.Count().Should().Be(savedSearches.Length);
            response.Code.Should().Be(ApprenticeshipSearchMediatorCodes.Index.Ok);
        }

        [Test]
        public void Ok_CandidateIsNotLoggedIn()
        {
            // Arrange.
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();

            var mediator = new ApprenticeshipSearchMediatorBuilder()
                .With(candidateServiceProvider).Build();

            // Act.
            var response = mediator.Index(null, ApprenticeshipSearchMode.SavedSearches, false);

            // Assert.
            candidateServiceProvider.Verify(p => p.GetSavedSearches(It.IsAny<Guid>()), Times.Never);
            response.Code.Should().Be(ApprenticeshipSearchMediatorCodes.Index.Ok);
        }
    }
}
