namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Builders;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    [Parallelizable]
    public class VacancyDatesViewModelServerValidatorTests
    {
        private const string RuleSet = RuleSets.ErrorsAndWarnings;

        private VacancyDatesViewModelServerCommonValidator _serverCommonValidator;
        private VacancyDatesViewModelServerValidator _serverValidator;
        private VacancyDatesViewModelCommonValidator _commonValidator;
        private VacancyViewModelValidator _vacancyValidator;
        private WageViewModel _wageViewModel;

        [SetUp]
        public void SetUp()
        {
            _serverCommonValidator = new VacancyDatesViewModelServerCommonValidator();
            _serverValidator = new VacancyDatesViewModelServerValidator();
            _commonValidator = new VacancyDatesViewModelCommonValidator();
            _vacancyValidator = new VacancyViewModelValidator();
            _wageViewModel = new WageViewModel(WageType.Custom, null, null, WageUnit.NotApplicable, null);
        }

        [Test]
        public void VacancyViewModelValidatorDefaultShouldHaveMultipleValidationErrors()
        {
            var datesViewModel = new VacancyDatesViewModel();
            var vacancyViewModel = new VacancyViewModelBuilder().With(datesViewModel).With(_wageViewModel).Build();

            var result = _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().BeGreaterThan(1);
        }

        [Test]
        public void CommonValidatorDefaultShouldNotHaveValidationErrors()
        {
            var datesViewModel = new VacancyDatesViewModel();

            var result = _commonValidator.Validate(datesViewModel, ruleSet: RuleSet);

            result.IsValid.Should().BeTrue();
            result.Errors.Count.Should().Be(0);
        }

        [Test]
        public void ServerCommonValidatorDefaultShouldHaveTwoValidationErrors()
        {
            var datesViewModel = new VacancyDatesViewModel();

            var result = _serverCommonValidator.Validate(datesViewModel, ruleSet: RuleSet);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(2);
        }

        [Test]
        public void ServerValidatorDefaultShouldHaveValidationErrors()
        {
            var datesViewModel = new VacancyDatesViewModel();

            var result = _serverValidator.Validate(datesViewModel, ruleSet: RuleSet);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(2);
        }

        [Test]
        public void ClosingDateRequired()
        {
            var datesViewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel()
            };

            var vacancyViewModel = new VacancyViewModelBuilder().With(_wageViewModel).With(datesViewModel).Build();

            _serverCommonValidator.Validate(datesViewModel, ruleSet: RuleSet);

            _vacancyValidator.Validate(vacancyViewModel);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _serverValidator.Validate(datesViewModel, ruleSet: RuleSet);

            _serverCommonValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, datesViewModel);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Errors);

            _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Warnings);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);

            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, datesViewModel);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, datesViewModel, RuleSets.Errors);
            _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, datesViewModel, RuleSets.Warnings);
            _serverValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, datesViewModel, RuleSets.ErrorsAndWarnings);
        }

        [Test]
        public void PossibleStartDateRequired()
        {
            var viewModel = new VacancyDatesViewModel
            {
                PossibleStartDate = new DateViewModel()
            };

            var vacancyViewModel = new VacancyViewModelBuilder().With(_wageViewModel).With(viewModel).Build();

            _serverCommonValidator.Validate(viewModel, ruleSet: RuleSet);

            _vacancyValidator.Validate(vacancyViewModel);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            _serverCommonValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Errors);

            _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Warnings);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);

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
                // VacancyStatus = VacancyStatus.Live,
                ClosingDate = new DateViewModel
                {
                    Day = 31,
                    Month = 11,
                    Year = 2015
                }
            };

            var vacancyViewModel = new VacancyViewModelBuilder().With(_wageViewModel).With(viewModel).Build();

            _serverCommonValidator.Validate(viewModel, ruleSet: RuleSet);

            _vacancyValidator.Validate(vacancyViewModel);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            _serverCommonValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Errors);

            _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Warnings);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);

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

            var vacancyViewModel = new VacancyViewModelBuilder().With(_wageViewModel).With(viewModel).Build();

            _serverCommonValidator.Validate(viewModel, ruleSet: RuleSet);

            _vacancyValidator.Validate(vacancyViewModel);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            _serverCommonValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Errors);

            _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Warnings);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);

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
                    Year = year
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(_wageViewModel).With(viewModel).Build();

            _serverCommonValidator.Validate(viewModel, ruleSet: RuleSet);

            _vacancyValidator.Validate(vacancyViewModel);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _serverCommonValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, vm => vm.ClosingDate.Year, viewModel, RuleSet);

                _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel);

                _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Errors);

                _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Warnings);

                _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.ErrorsAndWarnings);

                _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate.Year, viewModel);
                _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate.Year, viewModel, RuleSets.Errors);
                _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate.Year, viewModel, RuleSets.Warnings);
                _serverValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate.Year, viewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _serverCommonValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, vm => vm.ClosingDate.Year, viewModel, RuleSet);

                _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel);

                _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Errors);

                _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Warnings);

                _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.ErrorsAndWarnings);

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

            var vacancyViewModel = new VacancyViewModelBuilder().With(_wageViewModel).With(viewModel).Build();

            _commonValidator.Validate(viewModel, ruleSet: RuleSet);
            _vacancyValidator.Validate(vacancyViewModel);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);
            _serverValidator.Validate(viewModel, ruleSet: RuleSet);

            _commonValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDateComment, viewModel, RuleSet);
            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDateComment, vacancyViewModel);
            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDateComment, vacancyViewModel, RuleSets.Errors);
            _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDateComment,  vacancyViewModel, RuleSets.Warnings);
            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDateComment, vacancyViewModel, RuleSets.ErrorsAndWarnings);
        }

        [Test]
        public void PossibleStartDateCommentShouldNotAllowInvalidCharacters()
        {
            var viewModel = new VacancyDatesViewModel
            {
                PossibleStartDateComment = "<script>"
            };

            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            _commonValidator.Validate(viewModel, ruleSet: RuleSet);

            _vacancyValidator.Validate(vacancyViewModel);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _vacancyValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _commonValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDateComment, viewModel, RuleSet);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDateComment, vacancyViewModel);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDateComment, vacancyViewModel, RuleSets.Errors);

            _vacancyValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDateComment, vacancyViewModel, RuleSets.Warnings);

            _vacancyValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDateComment, vacancyViewModel, RuleSets.ErrorsAndWarnings);
        }
    }
}