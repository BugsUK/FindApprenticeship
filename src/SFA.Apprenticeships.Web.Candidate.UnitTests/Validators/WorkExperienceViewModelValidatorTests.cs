namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.Validators;
    using Candidate.ViewModels.Candidate;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class WorkExperienceViewModelValidatorTests
    {
        private WorkExperienceViewModel _viewModel;
        private WorkExperienceViewModelValidator _validator;

        [SetUp]
        public void SetUp()
        {
            // Start with a valid view model and break things.
            _viewModel = new WorkExperienceViewModel
            {
                Employer = "Acme Training Ltd",
                JobTitle = "Being Awesome",
                Description = "How to be awesome",
                FromYear = "2011",
                FromMonth = 1,
                ToYear = "2013",
                ToMonth = 6
            };

            _validator = new WorkExperienceViewModelValidator();
        }

        [Test]
        public void ShouldValidateValidViewModel()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Employer, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.JobTitle, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Description, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.FromMonth, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.FromYear, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.ToMonth, _viewModel);
            _validator.ShouldNotHaveValidationErrorFor(x => x.ToYear, _viewModel);
        }

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(200, true)]
        [TestCase(201, false)]
        public void ShouldValidateDescription(int? length, bool isValid)
        {
            _viewModel.Description = length.HasValue ? new string('X', length.Value) : default(string);

            if (isValid)
                _validator.ShouldNotHaveValidationErrorFor(x => x.Description, _viewModel);
            else
                _validator.ShouldHaveValidationErrorFor(x => x.Description, _viewModel);
        }

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(50, true)]
        [TestCase(51, false)]
        public void ShouldValidateEmployer(int? length, bool isValid)
        {
            _viewModel.Employer = length.HasValue ? new string('X', length.Value) : default(string);

            if (isValid)
                _validator.ShouldNotHaveValidationErrorFor(x => x.Employer, _viewModel);
            else
                _validator.ShouldHaveValidationErrorFor(x => x.Employer, _viewModel);
        }

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(50, true)]
        [TestCase(51, false)]
        public void ShouldValidateJobTitle(int? length, bool isValid)
        {
            _viewModel.JobTitle = length.HasValue ? new string('X', length.Value) : default(string);

            if (isValid)
                _validator.ShouldNotHaveValidationErrorFor(x => x.JobTitle, _viewModel);
            else
                _validator.ShouldHaveValidationErrorFor(x => x.JobTitle, _viewModel);
        }

        [Test]
        public void ShouldNotAllowNonWhitelistedCharacters()
        {
            _viewModel.Description = _viewModel.Employer = _viewModel.JobTitle = "<script>";

            _validator.ShouldHaveValidationErrorFor(x => x.Description, _viewModel);
            _validator.ShouldHaveValidationErrorFor(x => x.Employer, _viewModel);
            _validator.ShouldHaveValidationErrorFor(x => x.JobTitle, _viewModel);
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
        [TestCase("2015", "3499", false)]
        [TestCase(null, null, true)]
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