using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using System;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Common.Configuration;
    using Common.Constants;
    using Common.Providers;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests : TestsBase
    {
        [Test]
        public void Ok()
        {
            var mediator = GetMediator();
            var response = mediator.Index(null);

            response.AssertCode(TraineeshipSearchMediatorCodes.Index.Ok, true);

            var viewModel = response.ViewModel;

            viewModel.WithinDistance.Should().Be(40);
            viewModel.ResultsPerPage.Should().Be(5);
            viewModel.SortType.Should().Be(VacancySearchSortType.Distance);

            viewModel.Distances.Should().NotBeNull();
            viewModel.Distances.SelectedValue.Should().Be(null);

            viewModel.SortTypes.Should().NotBeNull();
            viewModel.SortTypes.Count().Should().BeGreaterThan(0);
            viewModel.SortTypes.SelectedValue.Should().Be(VacancySearchSortType.Distance);
        }

        [Test]
        public void PoputlateLocationWithUsersPostcode()
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails
                {
                    Address = new Address
                    {
                        Postcode = "CANDIDATE POSTCODE"
                    }
                }
            };
            var mockUserDataProvider = new Mock<IUserDataProvider>();
            var mockCandidateServiceProvider = new Mock<ICandidateServiceProvider>();
            mockCandidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);

            var mediator = GetMediator(mockUserDataProvider, mockCandidateServiceProvider);

            var response = mediator.Index(Guid.NewGuid());
            response.AssertCode(TraineeshipSearchMediatorCodes.Index.Ok, true);
            response.ViewModel.Location.Should().Be("CANDIDATE POSTCODE");

            mockUserDataProvider.Verify(x => x.Push(UserDataItemNames.LastSearchedLocation, "CANDIDATE POSTCODE"), Times.Once);
            mockCandidateServiceProvider.Verify(x => x.GetCandidate(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void PoputlateLocationCookieWithSearchedLocation()
        {
            var mockUserDataProvider = new Mock<IUserDataProvider>();
            var mockCandidateServiceProvider = new Mock<ICandidateServiceProvider>();

            mockUserDataProvider.Setup(x => x.Get(UserDataItemNames.LastSearchedLocation)).Returns("TEST COOKIE LOCATION");

            var mediator = GetMediator(mockUserDataProvider, mockCandidateServiceProvider);
            
            var response = mediator.Index(null);
            response.AssertCode(TraineeshipSearchMediatorCodes.Index.Ok, true);
            response.ViewModel.Location.Should().Be("TEST COOKIE LOCATION");
            mockCandidateServiceProvider.Verify(x => x.GetCandidate(It.IsAny<Guid>()), Times.Never);
        }

        private static ITraineeshipSearchMediator GetMediator(Mock<IUserDataProvider> mockUserDataProvider = null, Mock<ICandidateServiceProvider> mockCandidateServiceProvider = null)
        {
            var configurationService = new Mock<IConfigurationService>();

            configurationService.Setup(cm => cm.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration() {VacancyResultsPerPage = 5});

            var searchProvider = new Mock<ISearchProvider>();
            var userDataProvider = mockUserDataProvider ?? new Mock<IUserDataProvider>();
            var traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
            var candidateServiceProvider = mockCandidateServiceProvider ?? new Mock<ICandidateServiceProvider>();
            return GetMediator(configurationService.Object, searchProvider.Object, userDataProvider.Object, traineeshipVacancyProvider.Object, candidateServiceProvider.Object);
        }
    }
}