namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System;
    using System.Linq;
    using Builders;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    [TestFixture]
    [Parallelizable]
    public class PreviewVacancyTests : TestsBase
    {
        //IVacancyPostingMediator.GetPreviewVacancyViewModel 
        [Test]
        public void ClosingDateWarnings()
        {
            //Arrange
            var yesterday = DateTime.Today.AddDays(-1);

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(yesterday),
                    PossibleStartDate = new DateViewModel(yesterday)
                },
                WageObject = new WageViewModel(WageType.Custom, null, null, WageUnit.NotApplicable, null)
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.Code.Should().Be(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.FailedValidation);
            result.ValidationResult.Errors.Count(e => e.PropertyName == "FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate").Should().Be(2);
            result.ValidationResult.Errors.Count(e => e.PropertyName == "FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate").Should().Be(2);
        }

        [TestCase(VacancyStatus.Live, false)]
        [TestCase(VacancyStatus.Closed, false)]
        [TestCase(VacancyStatus.Completed, true)]
        [TestCase(VacancyStatus.Withdrawn, false)]
        public void CanHaveApplications_NoApplicationsRouteTest(VacancyStatus status, bool shouldHaveArchiveMessage)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status, VacancyType.Apprenticeship);
            vacancyViewModel.ApplicationCount = 0;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            if (shouldHaveArchiveMessage)
            {
                result.AssertMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok, VacancyViewModelMessages.VacancyHasBeenArchived+"<br/>"+ VacancyViewModelMessages.NoApplications, UserMessageLevel.Info);
            }
            else
            {
                result.AssertMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok, VacancyViewModelMessages.NoApplications, UserMessageLevel.Info);
            }
        }

        [TestCase(VacancyStatus.Live, true)]
        [TestCase(VacancyStatus.Closed, true)]
        [TestCase(VacancyStatus.Completed, false)]
        [TestCase(VacancyStatus.Withdrawn, true)]
        public void CanHaveApplications_OneApplicationRouteTest(VacancyStatus status, bool messageShouldBeNull)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status, VacancyType.Apprenticeship);
            vacancyViewModel.ApplicationCount = 1;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertCodeAndMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok, false, messageShouldBeNull);
        }

        [TestCase(VacancyStatus.Unknown)]
        [TestCase(VacancyStatus.Draft)]
        [TestCase(VacancyStatus.Submitted)]
        [TestCase(VacancyStatus.ReservedForQA)]
        [TestCase(VacancyStatus.Referred)]
        public void CannotHaveApplications(VacancyStatus status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status, VacancyType.Apprenticeship);
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertCodeAndMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok);
        }

        [TestCase(VacancyStatus.Live, false)]
        [TestCase(VacancyStatus.Closed, false)]
        [TestCase(VacancyStatus.Completed, true)]
        [TestCase(VacancyStatus.Withdrawn, false)]
        public void CanHaveClickThroughs_NoClickThroughsRouteTest(VacancyStatus status, bool shouldHaveArchiveMessage)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status, VacancyType.Apprenticeship);
            vacancyViewModel.NewVacancyViewModel.OfflineVacancy = true;
            vacancyViewModel.OfflineApplicationClickThroughCount = 0;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            if (shouldHaveArchiveMessage)
            {
                result.AssertMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok, VacancyViewModelMessages.VacancyHasBeenArchived + "<br/>" + VacancyViewModelMessages.NoClickThroughs, UserMessageLevel.Info);
            }
            else
            {
                result.AssertMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok, VacancyViewModelMessages.NoClickThroughs, UserMessageLevel.Info);
            }
            
        }

        [TestCase(VacancyStatus.Live, true)]
        [TestCase(VacancyStatus.Closed, true)]
        [TestCase(VacancyStatus.Completed, false)]
        [TestCase(VacancyStatus.Withdrawn, true)]
        public void CanHaveClickThroughs_OneClickThroughRouteTest(VacancyStatus status, bool messageShouldBeNull)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status, VacancyType.Apprenticeship);
            vacancyViewModel.NewVacancyViewModel.OfflineVacancy = true;
            vacancyViewModel.OfflineApplicationClickThroughCount = 1;
            vacancyViewModel.ApplicationCount = 0;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<int>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertCodeAndMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok, false, messageShouldBeNull);
        }
    }
}