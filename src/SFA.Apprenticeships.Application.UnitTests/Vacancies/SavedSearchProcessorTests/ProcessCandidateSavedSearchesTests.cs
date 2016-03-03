namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.SavedSearchProcessorTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Vacancies.Entities;
    using Apprenticeships.Application.Vacancies.Extensions;
    using Builders;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Locations;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Locations;
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
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(new CandidateBuilder(candidateId).EnableSavedSearchAlertsViaEmailAndText(sendSavedSearchAlerts).Build);
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
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(new CandidateBuilder(candidateId).EnableSavedSearchAlertsViaEmailAndText(true).Build);
            var vacancySearchProvider = new Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>();
            ApprenticeshipSearchParameters searchParameters = null;
            vacancySearchProvider.Setup(p => p.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>()))
                .Returns(new ApprenticeshipSearchResultsBuilder().WithResultCount(3).Build)
                .Callback<ApprenticeshipSearchParameters>(p => { searchParameters = p; });
            var processor = new SavedSearchProcessorBuilder().With(savedSearchReadRepository).With(userReadRepository).With(candidateReadRepository).With(vacancySearchProvider).Build();

            processor.ProcessCandidateSavedSearches(candidateSavedSearch);

            if (alertsEnabled)
            {
                vacancySearchProvider.Verify(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>()), Times.Exactly(3));
                searchParameters.Should().NotBeNull();
            }
            else
            {
                vacancySearchProvider.Verify(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>()), Times.Never);
                searchParameters.Should().BeNull();
            }
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void FindLocation(bool latLongSpecified, bool locationFound)
        {
            var candidateId = Guid.NewGuid();
            var candidateSavedSearch = new CandidateSavedSearches { CandidateId = candidateId };

            var savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
            List<SavedSearch> savedSearches;
            if (latLongSpecified)
            {
                savedSearches = new Fixture().Build<SavedSearch>().With(s => s.AlertsEnabled, true).CreateMany(3).ToList();
            }
            else
            {
                savedSearches = new Fixture().Build<SavedSearch>()
                    .With(s => s.Latitude, null)
                    .With(s => s.Longitude, null)
                    .With(s => s.AlertsEnabled, true)
                    .CreateMany(3).ToList();
            }
            savedSearchReadRepository.Setup(r => r.GetForCandidate(candidateId)).Returns(savedSearches);
            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(candidateId)).Returns(new UserBuilder(candidateId).Activated(true).Build);
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(new CandidateBuilder(candidateId).EnableSavedSearchAlertsViaEmailAndText(true).Build);
            var locationSearchService = new Mock<ILocationSearchService>();
            if (locationFound)
            {
                locationSearchService.Setup(s => s.FindLocation(It.IsAny<string>())).Returns(new Fixture().Build<Location>().CreateMany(3));
            }
            var vacancySearchProvider = new Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>();
            ApprenticeshipSearchParameters searchParameters = null;
            vacancySearchProvider.Setup(p => p.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>()))
                .Returns(new ApprenticeshipSearchResultsBuilder().WithResultCount(3).Build)
                .Callback<ApprenticeshipSearchParameters>(p => { searchParameters = p; });
            var savedSearchWriteRepository = new Mock<ISavedSearchWriteRepository>();
            var processor = new SavedSearchProcessorBuilder().With(savedSearchReadRepository).With(userReadRepository).With(candidateReadRepository).With(locationSearchService).With(vacancySearchProvider).With(savedSearchWriteRepository).Build();

            processor.ProcessCandidateSavedSearches(candidateSavedSearch);

            if (latLongSpecified)
            {
                locationSearchService.Verify(l => l.FindLocation(It.IsAny<string>()), Times.Never);
                savedSearchWriteRepository.Verify(p => p.Save(It.IsAny<SavedSearch>()), Times.Exactly(3));
                searchParameters.Should().NotBeNull();
                vacancySearchProvider.Verify(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>()), Times.Exactly(3));
            }
            else
            {
                locationSearchService.Verify(l => l.FindLocation(It.IsAny<string>()), Times.Exactly(3));
                if (locationFound)
                {
                    //One save to update lat/long and one for the new results. 3 searches x 2 = 6 
                    savedSearchWriteRepository.Verify(p => p.Save(It.IsAny<SavedSearch>()), Times.Exactly(6));
                    searchParameters.Should().NotBeNull();
                    vacancySearchProvider.Verify(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>()), Times.Exactly(3));
                }
                else
                {
                    savedSearchWriteRepository.Verify(p => p.Save(It.IsAny<SavedSearch>()), Times.Never);
                    searchParameters.Should().BeNull();
                    vacancySearchProvider.Verify(sp => sp.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>()), Times.Never);
                }
            }
        }

        [Test]
        public void ExistingResults()
        {
            var candidateId = Guid.NewGuid();
            var candidateSavedSearch = new CandidateSavedSearches { CandidateId = candidateId };

            var searchResults = new ApprenticeshipSearchResultsBuilder().WithResultCount(3).Build();

            var savedSearches = new Fixture().Build<SavedSearch>()
                .With(s => s.AlertsEnabled, true)
                .With(s => s.LastResultsHash, searchResults.Results.GetResultsHash())
                .With(s => s.DateProcessed, new DateTime(2015, 01, 01))
                .CreateMany(1).ToList();

            var savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
            savedSearchReadRepository.Setup(r => r.GetForCandidate(candidateId)).Returns(savedSearches);
            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(candidateId)).Returns(new UserBuilder(candidateId).Activated(true).Build);
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(new CandidateBuilder(candidateId).EnableSavedSearchAlertsViaEmailAndText(true).Build);
            var vacancySearchProvider = new Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>();
            vacancySearchProvider.Setup(p => p.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>())).Returns(searchResults);
            var savedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();
            var savedSearchWriteRepository = new Mock<ISavedSearchWriteRepository>();
            var processor = new SavedSearchProcessorBuilder().With(savedSearchReadRepository).With(userReadRepository).With(candidateReadRepository).With(vacancySearchProvider).With(savedSearchAlertRepository).With(savedSearchWriteRepository).Build();

            processor.ProcessCandidateSavedSearches(candidateSavedSearch);

            savedSearchWriteRepository.Verify(r => r.Save(It.IsAny<SavedSearch>()), Times.Never);
            savedSearchAlertRepository.Verify(r => r.GetUnsentSavedSearchAlert(It.IsAny<SavedSearch>()), Times.Never);
            savedSearchAlertRepository.Verify(r => r.Save(It.IsAny<SavedSearchAlert>()), Times.Never);
        }

        [Test]
        public void NewResults()
        {
            var candidateId = Guid.NewGuid();
            var candidateSavedSearch = new CandidateSavedSearches { CandidateId = candidateId };

            var existingSearchResults = new ApprenticeshipSearchResultsBuilder().WithResultCount(2).Build();
            var dateProcessed = new DateTime(2015, 01, 01);
            var savedSearches = new Fixture().Build<SavedSearch>()
                .With(s => s.AlertsEnabled, true)
                .With(s => s.LastResultsHash, existingSearchResults.Results.GetResultsHash())
                .With(s => s.DateProcessed, dateProcessed)
                .CreateMany(1).ToList();

            var savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
            savedSearchReadRepository.Setup(r => r.GetForCandidate(candidateId)).Returns(savedSearches);
            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(candidateId)).Returns(new UserBuilder(candidateId).Activated(true).Build);
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(new CandidateBuilder(candidateId).EnableSavedSearchAlertsViaEmailAndText(true).Build);
            var vacancySearchProvider = new Mock<IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>();
            vacancySearchProvider.Setup(p => p.FindVacancies(It.IsAny<ApprenticeshipSearchParameters>())).Returns(new ApprenticeshipSearchResultsBuilder().WithResultCount(3).Build);
            var savedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();
            SavedSearchAlert savedSearchAlert = null;
            savedSearchAlertRepository.Setup(r => r.Save(It.IsAny<SavedSearchAlert>())).Callback<SavedSearchAlert>(a => { savedSearchAlert = a; });
            var savedSearchWriteRepository = new Mock<ISavedSearchWriteRepository>();
            SavedSearch savedSearch = null;
            savedSearchWriteRepository.Setup(r => r.Save(It.IsAny<SavedSearch>())).Callback<SavedSearch>(s => { savedSearch = s; });
            var processor = new SavedSearchProcessorBuilder().With(savedSearchReadRepository).With(userReadRepository).With(candidateReadRepository).With(vacancySearchProvider).With(savedSearchAlertRepository).With(savedSearchWriteRepository).Build();

            processor.ProcessCandidateSavedSearches(candidateSavedSearch);

            savedSearchWriteRepository.Verify(r => r.Save(It.IsAny<SavedSearch>()), Times.Once);
            savedSearch.Should().NotBeNull();
            savedSearch.LastResultsHash.Should().NotBeNullOrEmpty();
            savedSearch.DateProcessed.HasValue.Should().BeTrue();
            savedSearch.DateProcessed.Should().BeAfter(dateProcessed);
            savedSearchAlertRepository.Verify(r => r.GetUnsentSavedSearchAlert(It.IsAny<SavedSearch>()), Times.Once);
            savedSearchAlertRepository.Verify(r => r.Save(It.IsAny<SavedSearchAlert>()), Times.Once);
            savedSearchAlert.Should().NotBeNull();
            savedSearchAlert.Parameters.Should().Be(savedSearches.First());
            savedSearchAlert.Results.Should().NotBeNull();
            var results = savedSearchAlert.Results.ToList();
            results.Count.Should().Be(3);
            savedSearchAlert.BatchId.HasValue.Should().BeFalse();
            savedSearchAlert.SentDateTime.HasValue.Should().BeFalse();
        }
    }
}