namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Common.Constants;
    using Common.ViewModels.VacancySearch;
    using Constants.Pages;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class TraineeshipVacancyDetailsTests
    {
        [Test]
        public void VacancyStatusLiveTest()
        {
            var vacancyDetailViewModel = new TraineeshipVacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live
            };

            var traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
            traineeshipVacancyProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(traineeshipVacancyProvider).Build();

            var response = accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Available);
            response.Message.Should().BeNull();
        }

        [Test]
        public void VacancyStatusExpiredTest()
        {
            var vacancyDetailViewModel = new TraineeshipVacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Expired
            };

            var traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
            traineeshipVacancyProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(traineeshipVacancyProvider).Build();

            var response = accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Available);
            response.Message.Should().BeNull();
        }

        [Test]
        public void VacancyStatusUnavailableTest()
        {
            var vacancyDetailViewModel = new TraineeshipVacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Unavailable
            };

            var traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
            traineeshipVacancyProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(traineeshipVacancyProvider).Build();

            var response = accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Unavailable);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void VacancyNotFoundTest()
        {
            var traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
            traineeshipVacancyProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(default(TraineeshipVacancyDetailViewModel));
            var accountMediator = new AccountMediatorBuilder().With(traineeshipVacancyProvider).Build();

            var response = accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Unavailable);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void ErrorTest()
        {
            var vacancyDetailViewModel = new TraineeshipVacancyDetailViewModel
            {
                ViewModelMessage = "Has error"
            };

            var traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
            traineeshipVacancyProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(traineeshipVacancyProvider).Build();

            var response = accountMediator.TraineeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Error);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(vacancyDetailViewModel.ViewModelMessage);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }
    }
}