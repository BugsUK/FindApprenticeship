namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System;
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
    public class OptionalClosingAndPossibleStartDateTests
    {
        private const string RuleSet = RuleSets.Warnings;

        private VacancyDatesViewModelCommonValidator _commonValidator;
        private VacancyDatesViewModelServerCommonValidator _serverCommonValidator;
        private VacancyDatesViewModelServerWarningValidator _serverWarningValidator;
        private VacancyViewModelValidator _aggregateValidator;
        private WageViewModel _wageViewModel;

        [SetUp]
        public void SetUp()
        {
            _commonValidator = new VacancyDatesViewModelCommonValidator();
            _serverCommonValidator = new VacancyDatesViewModelServerCommonValidator();
            _serverWarningValidator = new VacancyDatesViewModelServerWarningValidator(null);
            _aggregateValidator = new VacancyViewModelValidator();
            _wageViewModel = new WageViewModel()
            {
                Type = WageType.Custom,
                Amount = null,
                AmountLowerBound = null,
                AmountUpperBound = null,
                Text = null,
                Unit = WageUnit.NotApplicable,
                HoursPerWeek = null
            };
        }

        [Test]
        public void ClosingDateInThePastWarning()
        {
            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(-7))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            _serverWarningValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _serverWarningValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSet);
        }

        [Test]
        public void PossibleStartDateInThePastWarning()
        {
            var viewModel = new VacancyDatesViewModel
            {
                PossibleStartDate = new DateViewModel(DateTime.Today.AddDays(-7))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            _serverWarningValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _serverWarningValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }
        
        [Test]
        public void ClosingDateDateMustBeTwoWeeksInTheFutureWarning()
        {
            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(13))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            _serverWarningValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _serverWarningValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSet);
        }
        
        [Test]
        public void PossibleStartDateMustBeAfterClosingDateWarning()
        {
            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(28)),
                PossibleStartDate = new DateViewModel(DateTime.Today.AddDays(21))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            _serverWarningValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _serverWarningValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }
        
        [Test]
        public void PossibleStartDateMustNotBeTheSameAsTheClosingDateWarning()
        {
            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(DateTime.Today.AddDays(28)),
                PossibleStartDate = new DateViewModel(DateTime.Today.AddDays(28))
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            _serverWarningValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _serverWarningValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }

        
        [Test]
        public void ClosingDateNotAValidDateNoException()
        {
            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel
                {
                    Day = 31,
                    Month = 2,
                    Year = 2015
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            _serverWarningValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _serverWarningValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSet);
        }
        
        [Test]
        public void PossibleStartNotAValidDateNoException()
        {
            var viewModel = new VacancyDatesViewModel
            {
                PossibleStartDate = new DateViewModel
                {
                    Day = 31,
                    Month = 2,
                    Year = 2015
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            _serverWarningValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _serverWarningValidator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }
        
        [Test]
        public void ClosingDateAndPossibleStartNotAValidDateNoException()
        {
            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel
                {
                    Day = 31,
                    Month = 2,
                    Year = 2015
                },
                PossibleStartDate = new DateViewModel
                {
                    Day = 31,
                    Month = 2,
                    Year = 2015
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            _serverWarningValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            _serverWarningValidator.ShouldNotHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _serverWarningValidator.ShouldNotHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
        }
        
        [Test]
        public void ClosingDateLessThanTwoWeeks()
        {
            var today = DateTime.Today;

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(today)
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            var result = _serverWarningValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _serverWarningValidator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
        }

        [Test]
        public void PossibleStartDateLessThanTwoWeeks()
        {
            var today = DateTime.Today;

            var viewModel = new VacancyDatesViewModel
            {
                PossibleStartDate = new DateViewModel(today)
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).With(_wageViewModel).Build();

            var result = _serverWarningValidator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _serverWarningValidator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSet);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel, vm => vm.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
        }
    }
}