namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Application.Interfaces.Vacancies;
    using Candidate.Validators;
    using Candidate.ViewModels.VacancySearch;
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ApprenticeshipSearchViewModelServerValidatorTests
    {
        [TestCase("")]
        [TestCase("b")]
        [TestCase("cv")]
        [TestCase(null)]
        public void LocationValidationFailedTests(string location)
        {
            var validator = new ApprenticeshipSearchViewModelServerValidator();
            var viewModel = new ApprenticeshipSearchViewModel {Location = location};
            validator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [TestCase("b1")]
        [TestCase("cv1")]
        [TestCase("london")]
        public void LocationValidationSuccessfulTests(string location)
        {
            var validator = new ApprenticeshipSearchViewModelServerValidator();
            var viewModel = new ApprenticeshipSearchViewModel {Location = location};
            validator.ShouldNotHaveValidationErrorFor(x => x.Location, viewModel);
        }

        [TestCase("VRN000123456")]
        [TestCase("chef")]
        [TestCase("12345")]
        public void LocationRequiredIfKeywordIsNotVacancyReferenceNumberAndFieldIsAll(string keywords)
        {
            var validator = new ApprenticeshipSearchViewModelServerValidator();
            var viewModel = new ApprenticeshipSearchViewModel
            {
                Keywords = keywords,
                SearchField = "All"
            };

            validator.Validate(viewModel).IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
            validator.ShouldNotHaveValidationErrorFor(x => x.Keywords, viewModel);
        }

        [TestCase("VAC000123456")]
        [TestCase("000123456")]
        [TestCase("123456")]
        public void LocationNotRequiredIfKeywordIsVacancyReferenceNumberAndFieldIsAll(string keywords)
        {
            var validator = new ApprenticeshipSearchViewModelServerValidator();
            var viewModel = new ApprenticeshipSearchViewModel
            {
                Keywords = keywords,
                SearchField = "All"
            };

            validator.Validate(viewModel).IsValid.Should().BeTrue();
            validator.ShouldNotHaveValidationErrorFor(x => x.Location, viewModel);
            validator.ShouldNotHaveValidationErrorFor(x => x.Keywords, viewModel);
        }

        [TestCase("VAC000123456")]
        [TestCase("000123456")]
        [TestCase("123456")]
        [TestCase("VRN000123456")]
        [TestCase("chef")]
        [TestCase("12345")]
        public void LocationNotRequiredIfSearchFieldIsVacancyReferenceNumber(string keywords)
        {
            var validator = new ApprenticeshipSearchViewModelServerValidator();
            var viewModel = new ApprenticeshipSearchViewModel
            {
                Keywords = keywords,
                SearchField = ApprenticeshipSearchField.ReferenceNumber.ToString()
            };

            validator.Validate(viewModel).IsValid.Should().BeTrue();
            validator.ShouldNotHaveValidationErrorFor(x => x.Location, viewModel);
            validator.ShouldNotHaveValidationErrorFor(x => x.Keywords, viewModel);
        }

        [TestCase("JobTitle")]
        [TestCase("Description")]
        [TestCase("Employer")]
        public void LocationRequiredIfKeywordIsVacancyReferenceNumberButFieldIsntValidForVrn(string searchField)
        {
            var validator = new ApprenticeshipSearchViewModelServerValidator();
            var viewModel = new ApprenticeshipSearchViewModel
            {
                Keywords = "VAC000123456",
                SearchField = searchField
            };

            validator.Validate(viewModel).IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(x => x.Location, viewModel);
            validator.ShouldNotHaveValidationErrorFor(x => x.Keywords, viewModel);
        }
    }
}