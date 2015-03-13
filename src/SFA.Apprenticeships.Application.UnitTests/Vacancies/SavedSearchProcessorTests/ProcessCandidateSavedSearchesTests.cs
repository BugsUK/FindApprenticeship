namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.SavedSearchProcessorTests
{
    using System;
    using System.Linq;
    using Application.Vacancies.Entities;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Interfaces.Repositories;
    using Interfaces.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Vacancy;

    [TestFixture]
    public class ProcessCandidateSavedSearchesTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void RetrieveCandidateAndUser(bool userActivated)
        {
            var candidateId = Guid.NewGuid();
            var candidateSavedSearch = new CandidateSavedSearches {CandidateId = candidateId};

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(candidateId)).Returns(new UserBuilder(candidateId).Activated(userActivated).Build());
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(new CandidateBuilder(candidateId).Build);
            var processor = new SavedSearchProcessorBuilder().With(userReadRepository).With(candidateReadRepository).Build();

            processor.ProcessCandidateSavedSearches(candidateSavedSearch);

            userReadRepository.Verify(r => r.Get(candidateId), Times.Once);
            if (userActivated)
            {
                candidateReadRepository.Verify(r => r.Get(candidateId), Times.Once);
            }
            else
            {
                candidateReadRepository.Verify(r => r.Get(candidateId), Times.Never);
            }
        }

        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, false, false)]
        public void GetForCandidateCalled(bool userActivated, bool sendSavedSearchAlerts, bool expectCall)
        {
            var candidateId = Guid.NewGuid();
            var candidateSavedSearch = new CandidateSavedSearches { CandidateId = candidateId };

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(candidateId)).Returns(new UserBuilder(candidateId).Activated(userActivated).Build);
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(new CandidateBuilder(candidateId).AllowEmail(sendSavedSearchAlerts).SendSavedSearchAlerts(sendSavedSearchAlerts).Build);
            var savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
            var processor = new SavedSearchProcessorBuilder().With(userReadRepository).With(candidateReadRepository).With(savedSearchReadRepository).Build();

            processor.ProcessCandidateSavedSearches(candidateSavedSearch);

            if (expectCall)
            {
                savedSearchReadRepository.Verify(r => r.GetForCandidate(candidateId), Times.Once);
            }
            else
            {
                savedSearchReadRepository.Verify(r => r.GetForCandidate(candidateId), Times.Never);
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ExecuteSearch(bool alertsEnabled)
        {
            var candidateId = Guid.NewGuid();
            var candidateSavedSearch = new CandidateSavedSearches { CandidateId = candidateId };

            var savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
            savedSearchReadRepository.Setup(r => r.GetForCandidate(candidateId)).Returns(new Fixture().Build<SavedSearch>().With(s => s.AlertsEnabled, alertsEnabled).CreateMany(3).ToList());
            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(candidateId)).Returns(new UserBuilder(candidateId).Activated(true).Build);
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(new CandidateBuilder(candidateId).AllowEmail(true).SendSavedSearchAlerts(true).Build);
            var vacancySearchProvider = new Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>();
            var processor = new SavedSearchProcessorBuilder().With(savedSearchReadRepository).With(userReadRepository).With(candidateReadRepository).With(vacancySearchProvider).Build();

            processor.ProcessCandidateSavedSearches(candidateSavedSearch);

            if (alertsEnabled)
            {
                vacancySearchProvider.Verify(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>()), Times.Exactly(3));
            }
            else
            {
                vacancySearchProvider.Verify(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>()), Times.Never);
            }
        }

        [Test]
        public void NewResults()
        {
            var candidateId = Guid.NewGuid();
            var candidateSavedSearch = new CandidateSavedSearches { CandidateId = candidateId };

            var savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
            savedSearchReadRepository.Setup(r => r.GetForCandidate(candidateId)).Returns(new Fixture().Build<SavedSearch>().With(s => s.AlertsEnabled, true).CreateMany(1).ToList());
            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(candidateId)).Returns(new UserBuilder(candidateId).Activated(true).Build);
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(new CandidateBuilder(candidateId).AllowEmail(true).SendSavedSearchAlerts(true).Build);
            var vacancySearchProvider = new Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>();
            var savedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();
            var processor = new SavedSearchProcessorBuilder().With(savedSearchReadRepository).With(userReadRepository).With(candidateReadRepository).With(vacancySearchProvider).With(savedSearchAlertRepository).Build();

            processor.ProcessCandidateSavedSearches(candidateSavedSearch);

            savedSearchAlertRepository.Verify(r => r.Save(It.IsAny<SavedSearchAlert>()), Times.Once);
        }
    }
}