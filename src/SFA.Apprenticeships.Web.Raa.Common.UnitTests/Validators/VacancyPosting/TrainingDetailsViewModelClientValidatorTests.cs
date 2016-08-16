namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Validators.VacancyPosting
{
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Common.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    [Parallelizable]
    public class TrainingDetailsViewModelClientValidatorTests
    {
        private TrainingDetailsViewModelClientValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new TrainingDetailsViewModelClientValidator();
        }

        [Test]
        public void DefaultShouldNotHaveAnyValidationErrors()
        {
            var viewModel = new TrainingDetailsViewModel();

            var result = _validator.Validate(viewModel);

            result.IsValid.Should().BeTrue();
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase(Samples.ValidFreeText, true)]
        [TestCase(Samples.InvalidFreeTextWithInput, false)]
        [TestCase(Samples.InvalidFreeTextWithObject, false)]
        [TestCase(Samples.InvalidFreeTextWithScript, false)]
        public void TrainingProvidedInvalidCharacters(string trainingProvided, bool expectValid)
        {
            var viewModel = new TrainingDetailsViewModel
            {
                TrainingProvided = trainingProvided,
                VacancySource = VacancySource.Raa
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingProvided, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.TrainingProvided, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void ContactNameInvalidCharacters(string contactName, bool expectValid)
        {
            var viewModel = new TrainingDetailsViewModel
            {
                ContactName = contactName,
                VacancySource = VacancySource.Raa
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.ContactName, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.ContactName, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        public void ContactNumberInvalidCharacters(string contactNumber, bool expectValid)
        {
            var viewModel = new TrainingDetailsViewModel
            {
                ContactNumber = contactNumber,
                VacancySource = VacancySource.Raa
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.ContactNumber, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.ContactNumber, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        public void ContactEmailInvalidCharacters(string contactEmail, bool expectValid)
        {
            var viewModel = new TrainingDetailsViewModel
            {
                ContactEmail = contactEmail,
                VacancySource = VacancySource.Raa
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.ContactEmail, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.ContactEmail, viewModel);
            }
        }
    }
}