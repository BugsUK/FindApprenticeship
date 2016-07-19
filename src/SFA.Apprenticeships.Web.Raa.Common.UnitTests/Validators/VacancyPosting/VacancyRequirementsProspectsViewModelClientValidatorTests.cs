namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Validators.VacancyPosting
{
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Common.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    public class VacancyRequirementsProspectsViewModelClientValidatorTests
    {
        private VacancyRequirementsProspectsViewModelClientValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyRequirementsProspectsViewModelClientValidator();
        }

        [Test]
        public void DefaultShouldNotHaveAnyValidationErrors()
        {
            var viewModel = new VacancyRequirementsProspectsViewModel();

            var result = _validator.Validate(viewModel);

            result.IsValid.Should().BeTrue();
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void DesiredSkillsInvalidCharacters(string desiredSkills, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                DesiredSkills = desiredSkills
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.DesiredSkills, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.DesiredSkills, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void FutureProspectsInvalidCharacters(string futureProspects, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                FutureProspects = futureProspects
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.FutureProspects, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.FutureProspects, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void PersonalQualitiesInvalidCharacters(string personalQualities, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                PersonalQualities = personalQualities
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.PersonalQualities, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.PersonalQualities, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void ThingsToConsiderInvalidCharacters(string thingsToConsider, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                ThingsToConsider = thingsToConsider
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.ThingsToConsider, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.ThingsToConsider, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void DesiredQualificationsInvalidCharacters(string desiredQualifications, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                DesiredQualifications = desiredQualifications
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.DesiredQualifications, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.DesiredQualifications, viewModel);
            }
        }
    }
}