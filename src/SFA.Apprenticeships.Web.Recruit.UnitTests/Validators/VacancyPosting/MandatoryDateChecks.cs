namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System;
    using Common.ViewModels;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    public class MandatoryDateChecks
    {
        private VacancySummaryViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelServerValidator();
        }

        [Test]
        public void ClosingDateTwoWeeksAway_PossibleStartDateAfterClosingDate()
        {
            var today = DateTime.UtcNow;
            today = new DateTime(today.Year, today.Month, today.Day);

            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(today.AddDays(14)),
                PossibleStartDate = new DateViewModel(today.AddDays(15))
            };

            _validator.Validate(viewModel);

            _validator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel);
        }

        [Test]
        public void ClosingDateLessThanTwoWeeksAway_PossibleStartDateBeforeClosingDate()
        {
            var today = DateTime.UtcNow;
            today = new DateTime(today.Year, today.Month, today.Day);

            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(today.AddDays(13)),
                PossibleStartDate = new DateViewModel(today.AddDays(12))
            };

            _validator.Validate(viewModel);

            //Assert. This rule will be a warning rather than being mandatory and so is not implemented by the VacancySummaryViewModelServerValidator
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel);
            _validator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel);
        }
    }
}