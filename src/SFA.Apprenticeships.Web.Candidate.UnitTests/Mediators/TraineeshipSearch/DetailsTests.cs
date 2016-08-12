namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Configuration;
    using Common.Constants;
    using Common.Providers;
    using Common.UnitTests.Mediators;
    using Constants;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    [Parallelizable]
    public class DetailsTests : TestsBase
    {
        [SetUp]
        public void SetUp()
        {
            _userData = new Dictionary<string, string>();
        }

        private const string VacancyId = "1";
        private const string Distance = "42";
        private const string SearchReturnUrl = "http://www.example.com";

        private Dictionary<string, string> _userData;

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" 491802")]
        [TestCase("VAC000547307")]
        [TestCase("[[imgUrl]]")]
        [TestCase("separator.png")]
        public void GivenInvalidVacancyIdString_ThenVacancyNotFound(string vacancyId)
        {
            var mediator = GetMediator(null);
            var response = mediator.Details(vacancyId, null);

            response.AssertCode(TraineeshipSearchMediatorCodes.Details.VacancyNotFound, false);
        }

        private Mock<IUserDataProvider> GetUserDataProvider()
        {
            var userDataProvider = new Mock<IUserDataProvider>();

            userDataProvider.Setup(p => p.Pop(
                It.Is<string>(s => s == CandidateDataItemNames.VacancyDistance)))
                .Returns(Distance);

            userDataProvider.Setup(p => p.Pop(
                It.Is<string>(s => s == CandidateDataItemNames.LastViewedVacancy)))
                .Returns(VacancyType.Traineeship + "_" + VacancyId);

            userDataProvider.Setup(p => p.Push(
                It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((key, value) => _userData.Add(key, value));

            return userDataProvider;
        }

        private ITraineeshipSearchMediator GetMediator(TraineeshipVacancyDetailViewModel vacancyDetailViewModel)
        {
            var configurationManager = new Mock<IConfigurationService>();
            configurationManager.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {VacancyResultsPerPage = 5});
            var searchProvider = new Mock<ISearchProvider>();
            var traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();

            traineeshipVacancyProvider.Setup(
                p => p.GetVacancyDetailViewModel(It.IsAny<Guid?>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);

            var userDataProvider = GetUserDataProvider();

            return GetMediator(configurationManager.Object, searchProvider.Object, userDataProvider.Object,
                traineeshipVacancyProvider.Object, candidateServiceProvider.Object);
        }

        [Test]
        public void Ok()
        {
            var vacancyDetailViewModel = new TraineeshipVacancyDetailViewModel
            {
                Id = int.Parse(VacancyId),
                VacancyStatus = VacancyStatuses.Live
            };

            var mediator = GetMediator(vacancyDetailViewModel);
            var response = mediator.Details(VacancyId, null);

            response.AssertCode(TraineeshipSearchMediatorCodes.Details.Ok, true);

            response.ViewModel.Distance.Should().Be(Distance);

            _userData.ContainsKey(CandidateDataItemNames.VacancyDistance).Should().BeTrue();
            _userData[CandidateDataItemNames.VacancyDistance].Should().Be(Distance);

            _userData.ContainsKey(CandidateDataItemNames.LastViewedVacancy).Should().BeTrue();
            _userData[CandidateDataItemNames.LastViewedVacancy].Should()
                .Be(VacancyType.Traineeship + "_" + VacancyId.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void VacancyHasError()
        {
            const string message = "The vacancy has an error";

            var vacancyDetailViewModel = new TraineeshipVacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live,
                ViewModelMessage = message
            };

            var mediator = GetMediator(vacancyDetailViewModel);

            var response = mediator.Details(VacancyId, null);

            response.AssertMessage(TraineeshipSearchMediatorCodes.Details.VacancyHasError, message,
                UserMessageLevel.Warning, true);
        }

        [Test]
        public void VacancyNotFound()
        {
            var mediator = GetMediator(null);
            var response = mediator.Details(VacancyId, null);

            response.AssertCode(TraineeshipSearchMediatorCodes.Details.VacancyNotFound, false);
        }

        [Test]
        public void VacancyUnavailable()
        {
            var vacancyDetailViewModel = new TraineeshipVacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Unavailable
            };

            var mediator = GetMediator(vacancyDetailViewModel);
            var response = mediator.Details(VacancyId, null);

            response.AssertCode(TraineeshipSearchMediatorCodes.Details.VacancyNotFound, false);
        }
    }
}