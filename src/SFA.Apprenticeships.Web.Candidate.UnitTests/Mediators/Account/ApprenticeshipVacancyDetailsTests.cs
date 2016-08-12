namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ApprenticeshipVacancyDetailsTests
    {
        [Test]
        public void VacancyStatusLiveTest()
        {
            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live
            };

            var apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            apprenticeshipVacancyProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipVacancyProvider).Build();

            var response = accountMediator.ApprenticeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Available);
            response.Message.Should().BeNull();
        }

        [Test]
        public void VacancyStatusExpiredTest()
        {
            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Expired
            };

            var apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            apprenticeshipVacancyProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipVacancyProvider).Build();

            var response = accountMediator.ApprenticeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Available);
            response.Message.Should().BeNull();
        }

        [Test]
        public void VacancyStatusUnavailableTest()
        {
            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Unavailable
            };

            var apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            apprenticeshipVacancyProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipVacancyProvider).Build();

            var response = accountMediator.ApprenticeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Unavailable);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void VacancyNotFoundTest()
        {
            var apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            apprenticeshipVacancyProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(default(ApprenticeshipVacancyDetailViewModel));
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipVacancyProvider).Build();

            var response = accountMediator.ApprenticeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Unavailable);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void ErrorTest()
        {
            var vacancyDetailViewModel = new ApprenticeshipVacancyDetailViewModel
            {
                ViewModelMessage = "Has error"
            };

            var apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            apprenticeshipVacancyProvider.Setup(x => x.GetVacancyDetailViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(vacancyDetailViewModel);
            var accountMediator = new AccountMediatorBuilder().With(apprenticeshipVacancyProvider).Build();

            var response = accountMediator.ApprenticeshipVacancyDetails(Guid.NewGuid(), 42);

            response.Code.Should().Be(AccountMediatorCodes.VacancyDetails.Error);
            response.Message.Should().NotBeNull();
            response.Message.Text.Should().Be(vacancyDetailViewModel.ViewModelMessage);
            response.Message.Level.Should().Be(UserMessageLevel.Error);
        }
    }
}