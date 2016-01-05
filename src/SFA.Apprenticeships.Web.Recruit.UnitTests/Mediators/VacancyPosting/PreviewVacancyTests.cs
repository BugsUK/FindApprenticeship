namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System;
    using System.Linq;
    using Builders;
    using Common.ViewModels;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
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
            result.ValidationResult.Errors.Count(e => e.PropertyName == "VacancySummaryViewModel.ClosingDate").Should().Be(2);
            result.ValidationResult.Errors.Count(e => e.PropertyName == "VacancySummaryViewModel.PossibleStartDate").Should().Be(2);
        }
    }
}