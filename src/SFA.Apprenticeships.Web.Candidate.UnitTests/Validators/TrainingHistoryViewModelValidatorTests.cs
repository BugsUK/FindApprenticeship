﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.Validators;
    using Candidate.ViewModels.Candidate;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    public class TrainingHistoryViewModelValidatorTests
    {
        private TrainingHistoryViewModel _viewModel;
        private TrainingHistoryViewModelValidator _validator;

        [SetUp]
        public void SetUp()
        {
            // Start with a valid view model and break things.
            _viewModel = new TrainingHistoryViewModel
            {
                Provider = "Acme Training Ltd",
                CourseTitle = "Being Awesome",
                FromYear = "2011",
                FromMonth = 1,
                ToYear = "2013",
                ToMonth = 6
            };

            _validator = new TrainingHistoryViewModelValidator();
        }

        [Test]
        public void ShouldValidateValidViewModel()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Provider, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.CourseTitle, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.FromMonth, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.FromYear, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.ToMonth, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.ToYear, _viewModel);
        }

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(50, true)]
        [TestCase(51, false)]
        public void ShouldValidateProvider(int? length, bool isValid)
        {
            _viewModel.Provider = length.HasValue ? new string('X', length.Value) : default(string);

            if (isValid)
                _validator.ShouldNotHaveValidationErrorFor(x => x.Provider, _viewModel);
            else
                _validator.ShouldHaveValidationErrorFor(x => x.Provider, _viewModel);
        }

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(50, true)]
        [TestCase(51, false)]
        public void ShouldValidateCourseTitle(int? length, bool isValid)
        {
            _viewModel.CourseTitle = length.HasValue ? new string('X', length.Value) : default(string);

            if (isValid)
                _validator.ShouldNotHaveValidationErrorFor(x => x.CourseTitle, _viewModel);
            else
                _validator.ShouldHaveValidationErrorFor(x => x.CourseTitle, _viewModel);
        }

        [Test]
        public void ShouldNotAllowNonWhitelistedCharacters()
        {
            _viewModel.CourseTitle = _viewModel.Provider = "<script>";

            _validator.ShouldHaveValidationErrorFor(x => x.Provider, _viewModel);
            _validator.ShouldHaveValidationErrorFor(x => x.CourseTitle, _viewModel);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("3499", false)]
        [TestCase("1900", false)]
        [TestCase("201>", false)]
        [TestCase("2015", true)]
        public void ShouldValidateFromYear(string fromYear, bool isValid)
        {
            _viewModel.FromYear = fromYear;

            if (isValid)
                _validator.ShouldNotHaveValidationErrorFor(x => x.FromYear, _viewModel);
            else
                _validator.ShouldHaveValidationErrorFor(x => x.FromYear, _viewModel);
        }

        [TestCase(null, "2015", false)]
        [TestCase("", "2015", false)]
        [TestCase("1900", "1900", false)]
        [TestCase("2015", "201>", false)]
        [TestCase(null, null, true)]
        [TestCase("2015", "3499", true)]
        [TestCase("2014", "2015", true)]
        [TestCase("2015", "2015", true)]
        public void ShouldValidateToYear(string fromYear, string toYear, bool isValid)
        {
            _viewModel.FromYear = fromYear;
            _viewModel.ToYear = toYear;

            if (isValid)
                _validator.ShouldNotHaveValidationErrorFor(x => x.ToYear, _viewModel);
            else
                _validator.ShouldHaveValidationErrorFor(x => x.ToYear, _viewModel);
        }
    }
}