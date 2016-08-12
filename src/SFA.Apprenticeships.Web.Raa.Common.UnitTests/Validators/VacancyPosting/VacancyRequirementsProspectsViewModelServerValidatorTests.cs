namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Validators.VacancyPosting
{
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Builders;
    using Common.Validators.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using ViewModels.Vacancy;
    using Web.Common.UnitTests.Validators;
    using Web.Common.Validators;

    [TestFixture]
    [Parallelizable]
    public class VacancyRequirementsProspectsViewModelServerValidatorTests
    {
        private VacancyRequirementsProspectsViewModelServerValidator _validator;
        private VacancyViewModelValidator _aggregateValidator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyRequirementsProspectsViewModelServerValidator();
            _aggregateValidator = new VacancyViewModelValidator();
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase(Samples.ValidFreeHtmlText, true)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void DesiredSkillsValidation(string desiredSkills, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                DesiredSkills = desiredSkills,
                VacancySource = VacancySource.Raa
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.DesiredSkills, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.DesiredSkills, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void FutureProspectsRequired(string futureProspects, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                FutureProspects = futureProspects,
                VacancySource = VacancySource.Raa
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.FutureProspects, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.FutureProspects, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void PersonalQualitiesRequired(string personalQualities, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                PersonalQualities = personalQualities,
                VacancySource = VacancySource.Raa
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.PersonalQualities, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.PersonalQualities, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
		[TestCase(Samples.ValidFreeHtmlText, true)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void ThingsToConsiderNotRequired(string thingsToConsider, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                ThingsToConsider = thingsToConsider
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.ThingsToConsider, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.ThingsToConsider, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.ThingsToConsider, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.ThingsToConsider, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.ThingsToConsider, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.ThingsToConsider, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.ThingsToConsider, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.ThingsToConsider, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.ThingsToConsider, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.ThingsToConsider, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        
        [TestCase(Samples.ValidFreeHtmlText, true)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void ApprenticeshipDesiredQualificationsRequired(string desiredQualifications, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                DesiredQualifications = desiredQualifications,
                VacancyType = VacancyType.Apprenticeship,
                VacancySource = VacancySource.Raa
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.DesiredQualifications, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.DesiredQualifications, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase(Samples.ValidFreeHtmlText, true)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void TraineeshipDesiredQualificationsRequired(string desiredQualifications, bool expectValid)
        {
            var viewModel = new VacancyRequirementsProspectsViewModel
            {
                DesiredQualifications = desiredQualifications,
                VacancyType = VacancyType.Traineeship,
                VacancySource = VacancySource.Raa
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.DesiredQualifications, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.DesiredQualifications, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel, vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }
    }
}