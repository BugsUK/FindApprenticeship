namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Builders;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Vacancies;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    [Parallelizable]
    public class VacancyQuestionsViewModelServerValidatorTests
    {
        private VacancyQuestionsViewModelServerValidator _validator;
        private VacancyViewModelValidator _aggregateValidator;
        private FurtherVacancyDetailsViewModel _furtherVacancyDetailsViewModel;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyQuestionsViewModelServerValidator();
            _aggregateValidator = new VacancyViewModelValidator();
            _furtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel() {Wage = new WageViewModel() { Type = WageType.Custom, Amount = null, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.NotApplicable, HoursPerWeek = null } };
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase(Samples.ValidFreeText, true)]
        [TestCase(Samples.ValidFreeHtmlText, false)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void FirstQuestionValidation(string firstQuestion, bool expectValid)
        {
            var viewModel = new VacancyQuestionsViewModel
            {
                FirstQuestion = firstQuestion
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_furtherVacancyDetailsViewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.FirstQuestion, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.FirstQuestion, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.FirstQuestion, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.FirstQuestion, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.FirstQuestion, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.FirstQuestion, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.FirstQuestion, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.FirstQuestion, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.FirstQuestion, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.FirstQuestion, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase(Samples.ValidFreeText, true)]
        [TestCase(Samples.ValidFreeHtmlText, false)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void SecondQuestionValidation(string secondQuestion, bool expectValid)
        {
            var viewModel = new VacancyQuestionsViewModel
            {
                SecondQuestion = secondQuestion
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_furtherVacancyDetailsViewModel).Build();

            _validator.Validate(viewModel);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.SecondQuestion, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.SecondQuestion, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.SecondQuestion, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.SecondQuestion, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.SecondQuestion, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.SecondQuestion, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.SecondQuestion, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.SecondQuestion, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.SecondQuestion, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyQuestionsViewModel, vm => vm.VacancyQuestionsViewModel.SecondQuestion, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }
    }
}