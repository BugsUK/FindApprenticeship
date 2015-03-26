namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Common.Configuration;
    using Common.Providers;
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
            var response = mediator.Index();

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

        private static ITraineeshipSearchMediator GetMediator()
        {
            var configurationService = new Mock<IConfigurationService>();

            configurationService.Setup(cm => cm.Get<WebConfiguration>(WebConfiguration.ConfigurationName))
                .Returns(new WebConfiguration() {VacancyResultsPerPage = 5});

            var searchProvider = new Mock<ISearchProvider>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
            return GetMediator(configurationService.Object, searchProvider.Object, userDataProvider.Object, traineeshipVacancyProvider.Object);
        }
    }
}