﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Configuration;
    using Common.Constants;
    using Common.Providers;
    using Constants;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResultsTests : TestsBase
    {
        private const string ResultsPerPage = "5";
        private const string Distance = "42";
        private const string InvalidLocation = "InvalidLocation";

        private Dictionary<string, string> _userData;

        [SetUp]
        public void SetUp()
        {
            _userData = new Dictionary<string, string>();
        }

        [Test]
        public void LocationOk()
        {
            var mediator = GetMediator();

            var searchViewModel = new TraineeshipSearchViewModel
            {
                Location = "London"
            };

            var response = mediator.Results(searchViewModel);
            response.AssertCode(TraineeshipSearchMediatorCodes.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.Vacancies.Should().NotBeNullOrEmpty();

            var vacancies = viewModel.Vacancies.ToList();
            vacancies.Count.Should().Be(1);
        }

        [Test]
        public void LocationOk_ResultsPerPage()
        {
            var mediator = GetMediator();

            var searchViewModel = new TraineeshipSearchViewModel
            {
                Location = "London",
                ResultsPerPage = int.Parse(ResultsPerPage)
            };

            var response = mediator.Results(searchViewModel);
            response.AssertCode(TraineeshipSearchMediatorCodes.Results.Ok, true);

            var viewModel = response.ViewModel;
            
            viewModel.ResultsPerPageSelectList.Should().NotBeNull();
            viewModel.ResultsPerPageSelectList.Count().Should().BeGreaterThan(0);

            _userData.ContainsKey(UserDataItemNames.ResultsPerPage).Should().BeTrue();
            _userData[UserDataItemNames.ResultsPerPage].Should().Be(ResultsPerPage);
        }

        [Test]
        public void LocationOk_SortTypes()
        {
            var mediator = GetMediator();

            var searchViewModel = new TraineeshipSearchViewModel
            {
                Location = "London"
            };

            var response = mediator.Results(searchViewModel);
            response.AssertCode(TraineeshipSearchMediatorCodes.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.SortTypes.Should().NotBeNull();

            var sortTypes = viewModel.SortTypes.ToList();
            sortTypes.Count.Should().Be(3);
            sortTypes.Should().Contain(sli => sli.Value == VacancySearchSortType.ClosingDate.ToString());
            sortTypes.Should().Contain(sli => sli.Value == VacancySearchSortType.Distance.ToString());
            sortTypes.Should().Contain(sli => sli.Value == VacancySearchSortType.RecentlyAdded.ToString());
        }

        [Test]
        public void LocationOk_Distances()
        {
            var mediator = GetMediator();

            var searchViewModel = new TraineeshipSearchViewModel
            {
                Location = "London",
                WithinDistance = 40
            };

            var response = mediator.Results(searchViewModel);

            response.AssertCode(TraineeshipSearchMediatorCodes.Results.Ok, true);

            var viewModel = response.ViewModel;

            viewModel.Distances.Should().NotBeNull();
            viewModel.Distances.Count().Should().BeGreaterThan(0);
        }

        [Test]
        public void LocationOk_SuggestedLocations()
        {
            var mediator = GetMediator();

            var searchViewModel = new TraineeshipSearchViewModel
            {
                Location = "London"
            };

            var response = mediator.Results(searchViewModel);

            response.AssertCode(TraineeshipSearchMediatorCodes.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.LocationSearches.Should().NotBeNull();
            viewModel.LocationSearches.Count().Should().BeGreaterThan(0);
        }

        [Test]
        public void NoResults()
        {
            var mediator = GetMediator();

            var searchViewModel = new TraineeshipSearchViewModel
            {
                Location = "Middle of Nowhere"
            };

            var response = mediator.Results(searchViewModel);

            response.AssertCode(TraineeshipSearchMediatorCodes.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.Vacancies.Should().NotBeNull();

            var vacancies = viewModel.Vacancies.ToList();
            vacancies.Count.Should().Be(0);
        }

        [Test]
        public void NoSearchParameters()
        {
            var mediator = GetMediator();
            var searchViewModel = new TraineeshipSearchViewModel
            {
                Location = string.Empty
            };

            var response = mediator.Results(searchViewModel);

            response.AssertValidationResult(TraineeshipSearchMediatorCodes.Results.ValidationError, true);
        }

        [Test]
        public void LocationValidationError()
        {
            var mediator = GetMediator();

            var searchViewModel = new TraineeshipSearchViewModel
            {
                Location = InvalidLocation
            };

            var response = mediator.Results(searchViewModel);

            response.AssertCode(TraineeshipSearchMediatorCodes.Results.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.VacancySearch.ShouldBeEquivalentTo(searchViewModel);
        }

        [Test]
        public void SaveLocationSearchToCookie()
        {
            var mediator = GetMediator();
            
            var searchViewModel = new TraineeshipSearchViewModel
            {
                Location = "Entered loaction"
            };
            
            var response = mediator.Results(searchViewModel);
            _userData[UserDataItemNames.LastSearchedLocation].Should().Be(searchViewModel.Location);
        }

        private static Mock<ISearchProvider> GetSearchProvider()
        {
            var searchProvider = new Mock<ISearchProvider>();

            searchProvider.Setup(sp => sp.FindLocation(
                It.IsAny<string>())).
                Returns<string>(l => new LocationsViewModel(new[]
                {
                    new LocationViewModel { Name = l }, 
                    new LocationViewModel { Name = Guid.NewGuid().ToString() }
                }));

            searchProvider.Setup(sp => sp.FindLocation(
                InvalidLocation)).
                Returns<string>(l => new LocationsViewModel(new LocationViewModel[0]));

            return searchProvider;
        }

        private static Mock<ITraineeshipVacancyProvider> GetTraineeshipVacancyProvider()
        {
            var traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();

            var londonVacancies = new[]
            {
                new TraineeshipVacancySummaryViewModel {Description = "A London Vacancy"}
            };

            var emptyVacancies = new TraineeshipVacancySummaryViewModel[0];

            // This order is important. Moq will run though all matches and pick the last one.
            traineeshipVacancyProvider.Setup(sp => sp.FindVacancies(It.IsAny<TraineeshipSearchViewModel>())).Returns<TraineeshipSearchViewModel>(svm => new TraineeshipSearchResponseViewModel { Vacancies = emptyVacancies, VacancySearch = svm });
            traineeshipVacancyProvider.Setup(sp => sp.FindVacancies(It.Is<TraineeshipSearchViewModel>(svm => svm.Location == "London"))).Returns<TraineeshipSearchViewModel>(svm => new TraineeshipSearchResponseViewModel { Vacancies = londonVacancies, VacancySearch = svm });

            return traineeshipVacancyProvider;
        }

        private Mock<IUserDataProvider> GetUserDataProvider()
        {
            var userDataProvider = new Mock<IUserDataProvider>();

            userDataProvider.Setup(p => p.Pop(
                It.Is<string>(s => s == CandidateDataItemNames.VacancyDistance)))
                .Returns(Distance);

            userDataProvider.Setup(p => p.Push(
                It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((key, value) => _userData.Add(key, value));

            return userDataProvider;
        }

        private ITraineeshipSearchMediator GetMediator()
        {
            var searchProvider = GetSearchProvider();
            var traineeshipVacancyProvider = GetTraineeshipVacancyProvider();

            return GetMediator(searchProvider.Object, traineeshipVacancyProvider.Object);
        }

        private ITraineeshipSearchMediator GetMediator(ISearchProvider searchProvider, ITraineeshipVacancyProvider traineeshipVacancyProvider)
        {
            var configurationService = new Mock<IConfigurationService>();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();

            configurationService.Setup(x => x.Get<WebConfiguration>())
                .Returns(new WebConfiguration() { VacancyResultsPerPage = 5 });
            var userDataProvider = GetUserDataProvider();

            var mediator = GetMediator(configurationService.Object, searchProvider, userDataProvider.Object, traineeshipVacancyProvider, candidateServiceProvider.Object);
            return mediator;
        }
    }
}