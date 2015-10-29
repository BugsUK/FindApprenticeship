namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    public class VacancyRequirementsProspectsViewModelServerValidatorTests
    {
        private VacancyRequirementsProspectsViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyRequirementsProspectsViewModelServerValidator();
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        [TestCase("Desired Skill", true)]
        public void DesiredSkillsRequired(string desiredSkills, bool expectValid)
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

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        [TestCase("Future Prospects", true)]
        public void FutureProspectsRequired(string futureProspects, bool expectValid)
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

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        [TestCase("Personal Qualities", true)]
        public void PersonalQualitiesRequired(string personalQualities, bool expectValid)
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
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        [TestCase("Things To Consider", true)]
        public void ThingsToConsiderNotRequired(string thingsToConsider, bool expectValid)
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

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        [TestCase("Desired Qualifications", true)]
        public void DesiredQualificationsRequired(string desiredQualifications, bool expectValid)
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