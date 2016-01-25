namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System;
    using System.Linq;
    using Builders;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Common.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
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
            var viewModel = new VacancySummaryViewModel
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
            result.ValidationResult.Errors.Count(e => e.PropertyName == "VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate").Should().Be(2);
            result.ValidationResult.Errors.Count(e => e.PropertyName == "VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate").Should().Be(2);
        }

        [TestCase(ProviderVacancyStatuses.Live)]
        [TestCase(ProviderVacancyStatuses.Closed)]
        [TestCase(ProviderVacancyStatuses.Completed)]
        [TestCase(ProviderVacancyStatuses.Withdrawn)]
        public void CanHaveApplications_NoApplicationsRouteTest(ProviderVacancyStatuses status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status);
            vacancyViewModel.ApplicationCount = 0;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok, VacancyViewModelMessages.NoApplications, UserMessageLevel.Info);
        }

        [TestCase(ProviderVacancyStatuses.Live)]
        [TestCase(ProviderVacancyStatuses.Closed)]
        [TestCase(ProviderVacancyStatuses.Completed)]
        [TestCase(ProviderVacancyStatuses.Withdrawn)]
        public void CanHaveApplications_OneApplicationRouteTest(ProviderVacancyStatuses status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status);
            vacancyViewModel.ApplicationCount = 1;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertCode(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok);
        }

        [TestCase(ProviderVacancyStatuses.Unknown)]
        [TestCase(ProviderVacancyStatuses.Draft)]
        [TestCase(ProviderVacancyStatuses.PendingQA)]
        [TestCase(ProviderVacancyStatuses.ReservedForQA)]
        [TestCase(ProviderVacancyStatuses.RejectedByQA)]
        public void CannotHaveApplications(ProviderVacancyStatuses status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status);
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertCode(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok);
        }

        [TestCase(ProviderVacancyStatuses.Live)]
        [TestCase(ProviderVacancyStatuses.Closed)]
        [TestCase(ProviderVacancyStatuses.Completed)]
        [TestCase(ProviderVacancyStatuses.Withdrawn)]
        public void CanHaveClickThroughs_NoClickThroughsRouteTest(ProviderVacancyStatuses status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status);
            vacancyViewModel.NewVacancyViewModel.OfflineVacancy = true;
            vacancyViewModel.OfflineApplicationClickThroughCount = 0;
            VacancyPostingProvider.Setup(p => p.GetVacancy(It.IsAny<long>())).Returns(vacancyViewModel);
            var mediator = GetMediator();

            //Act
            var result = mediator.GetPreviewVacancyViewModel(0);

            //Assert
            result.AssertMessage(VacancyPostingMediatorCodes.GetPreviewVacancyViewModel.Ok, VacancyViewModelMessages.NoClickThroughs, UserMessageLevel.Info);
        }

        [TestCase(ProviderVacancyStatuses.Live)]
        [TestCase(ProviderVacancyStatuses.Closed)]
        [TestCase(ProviderVacancyStatuses.Completed)]
        [TestCase(ProviderVacancyStatuses.Withdrawn)]
        public void CanHaveClickThroughs_OneClickThroughRouteTest(ProviderVacancyStatuses status)
        {
            //Arrange
            var vacancyViewModel = new VacancyViewModelBuilder().BuildValid(status);
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