namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Common.Validators;
    using Common.ViewModels.Candidate;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    public class TrainingCourseViewModelValidatorTests
    {
        private TrainingCourseViewModel _viewModel;
        private TrainingCourseViewModelValidator _validator;

        [SetUp]
        public void SetUp()
        {
            // Start with a valid view model and break things.
            _viewModel = new TrainingCourseViewModel
            {
                Provider = "Acme Training Ltd",
                Title = "Being Awesome",
                FromYear = "2011",
                FromMonth = 1,
                ToYear = "2013",
                ToMonth = 6
            };

            _validator = new TrainingCourseViewModelValidator();
        }

        [Test]
        public void ShouldValidateValidViewModel()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Provider, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Title, _viewModel);
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
        public void ShouldValidateTitle(int? length, bool isValid)
        {
            _viewModel.Title = length.HasValue ? new string('X', length.Value) : default(string);

            if (isValid)
                _validator.ShouldNotHaveValidationErrorFor(x => x.Title, _viewModel);
            else
                _validator.ShouldHaveValidationErrorFor(x => x.Title, _viewModel);
        }

        [Test]
        public void ShouldNotAllowNonWhitelistedCharacters()
        {
            _viewModel.Title = _viewModel.Provider = "<script>";

            _validator.ShouldHaveValidationErrorFor(x => x.Provider, _viewModel);
            _validator.ShouldHaveValidationErrorFor(x => x.Title, _viewModel);
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
        [TestCase("2015", null, false)]
        [TestCase("2015", "", false)]
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