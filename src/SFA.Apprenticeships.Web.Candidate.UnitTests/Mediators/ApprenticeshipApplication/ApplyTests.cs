using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Builders;
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.Models.Application;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ApplyTests
    {
        private const int ValidVacancyId = 1;
        private const int InvalidVacancyId = 99999;

        [Test]
        public void VacancyNotFound()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.ExpiredOrWithdrawn, VacancyDetail = new ApprenticeshipVacancyDetailViewModel() });
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.Apply(Guid.NewGuid(), InvalidVacancyId.ToString());

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Apply.VacancyNotFound, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning, false);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" 491802")]
        [TestCase("VAC000547307")]
        [TestCase("[[imgUrl]]")]
        [TestCase("separator.png")]
        public void GivenInvalidVacancyIdString_ThenVacancyNotFound(string vacancyId)
        {
            var mediator = new ApprenticeshipApplicationMediatorBuilder().Build();
            var response = mediator.Apply(Guid.NewGuid(), vacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.VacancyNotFound, false);
        }

        [Test]
        public void HasError()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new ApprenticeshipApplicationViewModel("Vacancy has error"));
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.Apply(Guid.NewGuid(), InvalidVacancyId.ToString());

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.HasError, false);
        }

        [Test]
        public void Ok()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().WithVacancyStatus(VacancyStatuses.Live).Build());
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.Apply(Guid.NewGuid(), ValidVacancyId.ToString());

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.Ok, true);
        }

        [Test]
        public void VacancyExpired()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().WithVacancyStatus(VacancyStatuses.Expired).Build());
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.Apply(Guid.NewGuid(), ValidVacancyId.ToString());

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Apply.VacancyNotFound, MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable, UserMessageLevel.Warning, false);
        }

        [Test]
        public void IncorrectState()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().WithStatus(ApplicationStatuses.Submitted).Build());
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.Apply(Guid.NewGuid(), ValidVacancyId.ToString());

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Apply.IncorrectState, MyApplicationsPageMessages.ApplicationInIncorrectState, UserMessageLevel.Info, false);
        }

        [Test]
        public void OfflineVacancy()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().ApplyViaEmployerWebsite(true).Build());
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.Apply(Guid.NewGuid(), ValidVacancyId.ToString());

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.OfflineVacancy, false);
        }

        [Test]
        public void DoNotCreateWhenFound()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().WithVacancyStatus(VacancyStatuses.Live).Build());
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.Apply(Guid.NewGuid(), ValidVacancyId.ToString());

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.Ok, true);

            apprenticeshipApplicationProvider.Verify(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId), Times.Once);
            apprenticeshipApplicationProvider.Verify(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId), Times.Never);
        }

        [Test]
        public void CreateWhenNull()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns((ApprenticeshipApplicationViewModel) null);
            apprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().WithVacancyStatus(VacancyStatuses.Live).Build());
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.Apply(Guid.NewGuid(), ValidVacancyId.ToString());

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.Ok, true);

            apprenticeshipApplicationProvider.Verify(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId), Times.Once);
            apprenticeshipApplicationProvider.Verify(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId), Times.Once);
        }

        [Test]
        public void CreateWhenApplicationNotFound()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().HasError(ApplicationViewModelStatus.ApplicationNotFound, MyApplicationsPageMessages.ApplicationNotFound).Build());
            apprenticeshipApplicationProvider.Setup(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModelBuilder().WithVacancyStatus(VacancyStatuses.Live).Build());
            var mediator = new ApprenticeshipApplicationMediatorBuilder().With(apprenticeshipApplicationProvider).Build();

            var response = mediator.Apply(Guid.NewGuid(), ValidVacancyId.ToString());

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Apply.Ok, true);

            apprenticeshipApplicationProvider.Verify(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId), Times.Once);
            apprenticeshipApplicationProvider.Verify(p => p.CreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId), Times.Once);
        }
    }
}