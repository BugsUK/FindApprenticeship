namespace SFA.Apprenticeships.Web.Common.UnitTests.Validators
{
    using System;
    using Common.Validators;
    using Common.ViewModels;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    public class DateViewModelServerValidatorTests
    {
        [Test]
        public void MustBeAValidDateClient()
        {
            var viewModel = new DateViewModel();
            var viewModelValidator = new DateViewModelClientValidator();

            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Day, viewModel);
        }

        [Test]
        public void MustBeAValidDateServer()
        {
            var viewModel = new DateViewModel();
            var viewModelValidator = new DateViewModelServerValidator();

            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Day, viewModel);
        }

        [TestCase(null, false)]
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(31, true)]
        [TestCase(32, false)]
        public void DayRangeCheck(int? day, bool expectValid)
        {
            var viewModel = new DateViewModel {Day = day};
            var viewModelValidator = new DateViewModelClientValidator();

            if (expectValid)
            {
                viewModelValidator.ShouldNotHaveValidationErrorFor(x => x.Day, viewModel);
            }
            else
            {
                viewModelValidator.ShouldHaveValidationErrorFor(x => x.Day, viewModel);
            }
        }

        [TestCase(null, false)]
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(12, true)]
        [TestCase(13, false)]
        public void MonthRangeCheck(int? month, bool expectValid)
        {
            var viewModel = new DateViewModel {Month = month};
            var viewModelValidator = new DateViewModelClientValidator();

            if (expectValid)
            {
                viewModelValidator.ShouldNotHaveValidationErrorFor(x => x.Month, viewModel);
            }
            else
            {
                viewModelValidator.ShouldHaveValidationErrorFor(x => x.Month, viewModel);
            }
        }

        [TestCase(null)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2000)]
        public void YearRangeCheckFailed(int? year)
        {
            var viewModel = new DateViewModel {Year = year};
            var viewModelValidator = new DateViewModelClientValidator();

            viewModelValidator.ShouldHaveValidationErrorFor(x => x.Year, viewModel);
        }

        [TestCase(-1, false)]
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(10, true)]
        [TestCase(11, false)]
        public void YearRangeCheck(int yearOffset, bool expectValid)
        {
            var viewModel = new DateViewModel {Year = DateTime.Now.Year + yearOffset};
            var viewModelValidator = new DateViewModelClientValidator();

            if (expectValid)
            {
                viewModelValidator.ShouldNotHaveValidationErrorFor(x => x.Year, viewModel);
            }
            else
            {
                viewModelValidator.ShouldHaveValidationErrorFor(x => x.Year, viewModel);
            }
        }
    }
}