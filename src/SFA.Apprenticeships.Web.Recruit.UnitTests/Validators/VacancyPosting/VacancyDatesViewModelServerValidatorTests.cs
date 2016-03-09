namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Builders;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class VacancyDatesViewModelServerValidatorTests
    {
        private const string RuleSet = RuleSets.ErrorsAndWarnings;

        private VacancyDatesViewModelServerCommonValidator _serverCommonValidator;
        private VacancyDatesViewModelServerValidator _serverValidator;
        private VacancyDatesViewModelCommonValidator _commonValidator;
        private VacancyViewModelValidator _aggregateValidator;

        [SetUp]
        public void SetUp()
        {
            _serverCommonValidator = new VacancyDatesViewModelServerCommonValidator();
            _serverValidator = new VacancyDatesViewModelServerValidator();
            _commonValidator = new VacancyDatesViewModelCommonValidator();
            _aggregateValidator = new VacancyViewModelValidator();
        }

        [Test]
        public void VacancyViewModelValidatorDefaultShouldHaveValidationErrors()
        {
            var viewModel = new VacancyDatesViewModel();
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            var aggregateResults = _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            aggregateResults.IsValid.Should().BeFalse();
            aggregateResults.Errors.Count.Should().BeGreaterThan(2);
        }

        [Test]
        public void CommonValidatorDefaultShouldHaveValidationErrors()
        {
            var viewModel = new VacancyDatesViewModel();

            var commonResult = _commonValidator.Validate(viewModel, ruleSet: RuleSet);

            commonResult.IsValid.Should().BeTrue();
        }

        [Test]
        public void ServerCommonValidatorDefaultShouldHaveValidationErrors()
        {
            var viewModel = new VacancyDatesViewModel();

            var serverCommonResult = _serverCommonValidator.Validate(viewModel, ruleSet: RuleSet);

            serverCommonResult.IsValid.Should().BeFalse();
            serverCommonResult.Errors.Count.Should().Be(2);
        }

        [Test]
        public void ServerValidatorDefaultShouldHaveValidationErrors()
        {
            var viewModel = new VacancyDatesViewModel();

            var serverResults = _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            serverResults.IsValid.Should().BeFalse();
            serverResults.Errors.Count.Should().Be(2);
        }

        [Test]
        public void ClosingDateRequired()
        {
            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel()
            };

            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _serverCommonValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);
            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            _serverCommonValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Errors);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Warnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSets.Errors);
            _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSets.Warnings);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSets.ErrorsAndWarnings);
        }

        [Test]
        public void PossibleStartDateRequired()
        {
            var viewModel = new VacancyDatesViewModel
            {
                PossibleStartDate = new DateViewModel()
            };

            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _serverCommonValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);
            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            _serverCommonValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Errors);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Warnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSets.Errors);
            _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSets.Warnings);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSets.ErrorsAndWarnings);
        }

        [Test]
        public void ClosingDateMustBeAValidDateRequired()
        {
            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel
                {
                    Day = 31,
                    Month = 11,
                    Year = 2015
                }
            };

            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _serverCommonValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);
            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            _serverCommonValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Errors);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Warnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);

            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSets.Errors);
            _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSets.Warnings);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSets.ErrorsAndWarnings);
        }

        [Test]
        public void PossibleStartMustBeAValidDateDateRequired()
        {
            var viewModel = new VacancyDatesViewModel
            {
                PossibleStartDate = new DateViewModel
                {
                    Day = 31,
                    Month = 11,
                    Year = 2015
                }
            };

            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _serverCommonValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);
            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            _serverCommonValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Errors);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Warnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);

            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSets.Errors);
            _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSets.Warnings);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSets.ErrorsAndWarnings);
        }

        [TestCase(16, false)]
        [TestCase(2016, true)]
        public void ClosingDateYearFormat(int year, bool expectValid)
        {
            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel
                {
                    /*Day = 1,
                    Month = 2,*/
                    Year = year
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _serverCommonValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);
            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _serverCommonValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, vm => vm.ClosingDate.Year, viewModel, RuleSet);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.ErrorsAndWarnings);

                _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate.Year, viewModel);
                _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate.Year, viewModel, RuleSets.Errors);
                _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate.Year, viewModel, RuleSets.Warnings);
                _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate.Year, viewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _serverCommonValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, vm => vm.ClosingDate.Year, viewModel, RuleSet);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.ErrorsAndWarnings);

                _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, vm => vm.ClosingDate.Year, viewModel);
                _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, vm => vm.ClosingDate.Year, viewModel, RuleSets.Errors);
                _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, vm => vm.ClosingDate.Year, viewModel, RuleSets.Warnings);
                _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, vm => vm.ClosingDate.Year, viewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [Test]
        public void ClosingDateCommentShouldNotAllowInvalidCharacters()
        {
            var viewModel = new VacancyDatesViewModel
            {
                ClosingDateComment = "<script>"
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _commonValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);
            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            _commonValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDateComment, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDateComment, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDateComment, vacancyViewModel, RuleSets.Errors);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDateComment,  vacancyViewModel, RuleSets.Warnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDateComment, vacancyViewModel, RuleSets.ErrorsAndWarnings);
        }

        [Test]
        public void PossibleStartDateCommentShouldNotAllowInvalidCharacters()
        {
            var viewModel = new VacancyDatesViewModel
            {
                PossibleStartDateComment = "<script>"
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _commonValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _commonValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDateComment, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDateComment, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDateComment, vacancyViewModel, RuleSets.Errors);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDateComment, vacancyViewModel, RuleSets.Warnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDateComment, vacancyViewModel, RuleSets.ErrorsAndWarnings);
        }
    }
}