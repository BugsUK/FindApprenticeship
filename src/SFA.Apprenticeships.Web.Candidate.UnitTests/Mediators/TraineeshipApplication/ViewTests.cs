using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using System;
    using Builders;
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Common.Constants;
    using Common.Models.Application;
    using Constants.Pages;
    using Domain.Entities.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ViewTests
    {
        private const int TestVacancyId = 5;

        [TestCase(VacancyStatuses.Live)]
        [TestCase(VacancyStatuses.Expired)]
        public void Ok(VacancyStatuses vacancyStatus)
        {
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new Mock<ITraineeshipApplicationProvider>();

            traineeshipApplicationProvider
                .Setup(p => p.GetApplicationViewModelEx(candidateId, TestVacancyId))
                .Returns(new TraineeshipApplicationViewModelBuilder()
                .WithVacancyStatus(vacancyStatus)
                .Build());

            var mediator = new TraineeshipApplicationMediatorBuilder()
                .With(traineeshipApplicationProvider)
                .Build();

            var response = mediator.View(candidateId, TestVacancyId);

            response.AssertCode(TraineeshipApplicationMediatorCodes.View.Ok, true);
        }

        [Test]
        public void ApplicationNotFound()
        {
            var viewModel = new TraineeshipApplicationViewModelBuilder()
                .HasError(ApplicationViewModelStatus.ApplicationNotFound, MyApplicationsPageMessages.ApplicationNotFound)
                .Build();

            var traineeshipApplicationProvider = new Mock<ITraineeshipApplicationProvider>();

            traineeshipApplicationProvider
                .Setup(p => p.GetApplicationViewModelEx(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(viewModel);

            var mediator = new TraineeshipApplicationMediatorBuilder()
                .With(traineeshipApplicationProvider)
                .Build();

            var response = mediator.View(Guid.NewGuid(), TestVacancyId);

            response.AssertCode(TraineeshipApplicationMediatorCodes.View.ApplicationNotFound, true);
        }

        [Test]
        public void HasError()
        {
            var viewModel = new TraineeshipApplicationViewModelBuilder()
                .HasError(ApplicationViewModelStatus.Error, ApplicationPageMessages.ViewApplicationFailed)
                .Build();

            var traineeshipApplicationProvider = new Mock<ITraineeshipApplicationProvider>();

            traineeshipApplicationProvider
                .Setup(p => p.GetApplicationViewModelEx(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(viewModel);

            var mediator = new TraineeshipApplicationMediatorBuilder()
                .With(traineeshipApplicationProvider)
                .Build();

            var response = mediator.View(Guid.NewGuid(), TestVacancyId);

            response.AssertMessage(
                TraineeshipApplicationMediatorCodes.View.Error,
                ApplicationPageMessages.ViewApplicationFailed,
                UserMessageLevel.Warning,
                false);
        }
    }
}