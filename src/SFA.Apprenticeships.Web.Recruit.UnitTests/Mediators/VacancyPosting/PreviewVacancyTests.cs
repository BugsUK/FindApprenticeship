namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System;
    using System.Linq;
    using Builders;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class PreviewVacancyTests : TestsBase
    {
        //IVacancyPostingMediator.GetPreviewVacancyViewModel 
        [Test]
        public void ClosingDateWarnings()
        {
            //Arrange
            var today = DateTime.Today;
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(today),
                    PossibleStartDate = new DateViewModel(today)
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.Code.Should().Be(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.FailedValidation);
            result.ValidationResult.Errors.Count(e => e.PropertyName == "FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate").Should().Be(2);
            result.ValidationResult.Errors.Count(e => e.PropertyName == "FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate").Should().Be(2);
        }

        [TestCase(VacancyStatus.Live)]
        [TestCase(VacancyStatus.Closed)]
        [TestCase(VacancyStatus.Completed)]
        [TestCase(VacancyStatus.Withdrawn)]
        public void CanHaveApplications_NoApplicationsRouteTest(VacancyStatus status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status, VacancyType.Apprenticeship);
            vacancyViewModel.ApplicationCount = 0;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok, VacancyViewModelMessages.NoApplications, UserMessageLevel.Info);
        }

        [TestCase(VacancyStatus.Live)]
        [TestCase(VacancyStatus.Closed)]
        [TestCase(VacancyStatus.Completed)]
        [TestCase(VacancyStatus.Withdrawn)]
        public void CanHaveApplications_OneApplicationRouteTest(VacancyStatus status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status, VacancyType.Apprenticeship);
            vacancyViewModel.ApplicationCount = 1;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertCode(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok);
        }

        [TestCase(VacancyStatus.Unknown)]
        [TestCase(VacancyStatus.Draft)]
        [TestCase(VacancyStatus.PendingQA)]
        [TestCase(VacancyStatus.ReservedForQA)]
        [TestCase(VacancyStatus.RejectedByQA)]
        public void CannotHaveApplications(VacancyStatus status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status, VacancyType.Apprenticeship);
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertCode(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok);
        }

        [TestCase(VacancyStatus.Live)]
        [TestCase(VacancyStatus.Closed)]
        [TestCase(VacancyStatus.Completed)]
        [TestCase(VacancyStatus.Withdrawn)]
        public void CanHaveClickThroughs_NoClickThroughsRouteTest(VacancyStatus status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status, VacancyType.Apprenticeship);
            vacancyViewModel.NewVacancyViewModel.OfflineVacancy = true;
            vacancyViewModel.OfflineApplicationClickThroughCount = 0;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok, VacancyViewModelMessages.NoClickThroughs, UserMessageLevel.Info);
        }

        [TestCase(VacancyStatus.Live)]
        [TestCase(VacancyStatus.Closed)]
        [TestCase(VacancyStatus.Completed)]
        [TestCase(VacancyStatus.Withdrawn)]
        public void CanHaveClickThroughs_OneClickThroughRouteTest(VacancyStatus status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status, VacancyType.Apprenticeship);
            vacancyViewModel.NewVacancyViewModel.OfflineVacancy = true;
            vacancyViewModel.OfflineApplicationClickThroughCount = 1;
            vacancyViewModel.ApplicationCount = 0;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertCode(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok);
        }
    }
}