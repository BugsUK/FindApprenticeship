namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Builders;
    using Castle.Core.Logging;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    [TestFixture]
    [Parallelizable]
    public class VacancySummaryViewModelServerValidatorTests
    {
        private const string RuleSet = RuleSets.ErrorsAndWarnings;

        private VacancySummaryViewModelServerValidator _validator;
        private VacancyViewModelValidator _aggregateValidator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelServerValidator();
            _aggregateValidator = new VacancyViewModelValidator();
        }

        [Test]
        public void DefaultShouldHaveValidationErrors()
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel(),
                VacancySource = VacancySource.Raa,
                Wage = new WageViewModel()
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            var result = _validator.Validate(viewModel, ruleSet: RuleSet);
            var aggregateResults = _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().BeGreaterThan(4);
            aggregateResults.IsValid.Should().BeFalse();
            aggregateResults.Errors.Count.Should().BeGreaterThan(4);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("Mon-Fri", true)]
        public void WorkingWeekRequired(string workingWeek, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                WorkingWeek = workingWeek,
                Wage = new WageViewModel()
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.WorkingWeek, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.WorkingWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.WorkingWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.WorkingWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.WorkingWeek, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.WorkingWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.WorkingWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.WorkingWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("Mon-Fri", false)]
        [TestCase("<script>", false)]
        [TestCase("15", false)]
        [TestCase("16", true)]
        [TestCase("30", true)]
        [TestCase("37.5", true)]
        [TestCase("40", true)]
        [TestCase("41", true)]
        [TestCase("-1", false)]
        [TestCase("0", false)]
        [TestCase("1", false)]
        public void HoursPerWeekRequiredIfApprenticeship(string hoursPerWeekString, bool expectValid)
        {
            decimal? hoursPerWeek = null;
            decimal parsedHoursPerWeek;
            if (decimal.TryParse(hoursPerWeekString, out parsedHoursPerWeek))
            {
                hoursPerWeek = parsedHoursPerWeek;
            }
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { HoursPerWeek = hoursPerWeek },
                VacancySource = VacancySource.Raa
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("15", false)]
        [TestCase("16", true)]
        [TestCase("30", true)]
        [TestCase("37.5", true)]
        [TestCase("40", true)]
        [TestCase("41", true)]
        [TestCase("-1", false)]
        [TestCase("0", false)]
        [TestCase("1", false)]
        public void HoursPerWeekNotRequiredIfTraineeship(string hoursPerWeekString, bool expectValid)
        {
            decimal? hoursPerWeek = null;
            decimal parsedHoursPerWeek;
            if (decimal.TryParse(hoursPerWeekString, out parsedHoursPerWeek))
            {
                hoursPerWeek = parsedHoursPerWeek;
            }
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { HoursPerWeek = hoursPerWeek },
                VacancyType = VacancyType.Traineeship
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.HoursPerWeek, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(0, false)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.ApprenticeshipMinimum, true)]
        [TestCase(WageType.NationalMinimum, true)]
        public void WageTypeRequired(WageType wageType, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Type = wageType }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Type, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Type, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(0, true)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.ApprenticeshipMinimum, true)]
        [TestCase(WageType.NationalMinimum, true)]
        public void WageTypeNotRequiredIfTraineeship(WageType wageType, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Type = wageType },
                VacancyType = VacancyType.Traineeship
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage.Type, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage.Type, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Type, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, WageType.Custom, false)]
        [TestCase(null, WageType.ApprenticeshipMinimum, true)]
        [TestCase(null, WageType.NationalMinimum, true)]
        [TestCase("", WageType.Custom, false)]
        [TestCase("", WageType.ApprenticeshipMinimum, true)]
        [TestCase("", WageType.NationalMinimum, true)]
        [TestCase(" ", WageType.Custom, false)]
        [TestCase(" ", WageType.ApprenticeshipMinimum, true)]
        [TestCase(" ", WageType.NationalMinimum, true)]
        [TestCase("Seven pounds an hour", WageType.Custom, false)]
        [TestCase("Seven pounds an hour", WageType.ApprenticeshipMinimum, true)]
        [TestCase("Seven pounds an hour", WageType.NationalMinimum, true)]
        [TestCase("<script>", WageType.Custom, false)]
        [TestCase("500", WageType.Custom, true)]
        [TestCase("500", WageType.ApprenticeshipMinimum, true)]
        [TestCase("500", WageType.NationalMinimum, true)]
        public void WageRequired(string wageString, WageType wageType, bool expectValid)
        {
            decimal? wage = null;
            decimal parsedWage;
            if (decimal.TryParse(wageString, out parsedWage))
            {
                wage = parsedWage;
            }
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Type = wageType, Amount = wage, Unit = wageType == WageType.Custom ? WageUnit.Weekly : WageUnit.NotApplicable, HoursPerWeek = 37.5m }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Amount, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Amount, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, WageType.Custom, true)]
        [TestCase(null, WageType.ApprenticeshipMinimum, true)]
        [TestCase(null, WageType.NationalMinimum, true)]
        [TestCase("", WageType.Custom, true)]
        [TestCase("", WageType.ApprenticeshipMinimum, true)]
        [TestCase("", WageType.NationalMinimum, true)]
        [TestCase(" ", WageType.Custom, true)]
        [TestCase(" ", WageType.ApprenticeshipMinimum, true)]
        [TestCase(" ", WageType.NationalMinimum, true)]
        [TestCase("Seven pounds an hour", WageType.Custom, true)]
        [TestCase("Seven pounds an hour", WageType.ApprenticeshipMinimum, true)]
        [TestCase("Seven pounds an hour", WageType.NationalMinimum, true)]
        [TestCase("<script>", WageType.Custom, true)]
        [TestCase("500", WageType.Custom, true)]
        [TestCase("500", WageType.ApprenticeshipMinimum, true)]
        [TestCase("500", WageType.NationalMinimum, true)]
        public void WageNotRequiredIfTraineeship(string wageString, WageType wageType, bool expectValid)
        {
            decimal? wage = null;
            decimal parsedWage;
            if (decimal.TryParse(wageString, out parsedWage))
            {
                wage = parsedWage;
            }
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Type = wageType, Amount = wage, Unit = wageType == WageType.Custom ? WageUnit.Weekly : WageUnit.NotApplicable, HoursPerWeek = 37.5m },
                VacancyType = VacancyType.Traineeship
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Amount, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Amount, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Amount, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("12 - 14 Months", false)]
        [TestCase("<script>", false)]
        [TestCase("11", true)]
        [TestCase("12", true)]
        [TestCase("14.5", false)]
        public void DurationRequired(string durationString, bool expectValid)
        {
            int? duration = null;
            int parsedDuration;
            if (int.TryParse(durationString, out parsedDuration))
            {
                duration = parsedDuration;
            }
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Duration = duration,
                VacancySource = VacancySource.Raa,
                Wage = new WageViewModel()
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        [TestCase("Description", true)]
        [TestCase(Samples.ValidFreeHtmlText, true)]
        [TestCase(Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(Samples.InvalidHtmlTextWithScript, false)]
        public void LongDescriptionRequired(string longDescription, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                LongDescription = longDescription,
                Wage = new WageViewModel()
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.LongDescription, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.LongDescription, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.LongDescription, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.LongDescription, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.LongDescription, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.LongDescription, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.LongDescription, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.LongDescription, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.LongDescription, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.LongDescription, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        //[TestCase(WageType.Custom, WageType.Custom, true)]
        //[TestCase(WageType.Custom, WageType.CustomRange, true)]
        //[TestCase(WageType.Custom, WageType.ApprenticeshipMinimum, false)]
        //[TestCase(WageType.Custom, WageType.CompetitiveSalary, false)]
        //[TestCase(WageType.Custom, WageType.LegacyText, false)]
        //[TestCase(WageType.Custom, WageType.LegacyWeekly, false)]
        //[TestCase(WageType.Custom, WageType.NationalMinimum, false)]
        //[TestCase(WageType.Custom, WageType.ToBeAgreedUponAppointment, false)]
        //[TestCase(WageType.Custom, WageType.Unwaged, false)]
        public void CustomTypeMustBeSetWhenCustomWageTypeIsSelected(WageType selectedType, WageType selectedCustomType, bool expectValid)
        {
            //var viewModel = new FurtherVacancyDetailsViewModel
            //{
            //    Wage = new WageViewModel() { Type = selectedType, CustomType = selectedCustomType, Amount = null, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.NotApplicable, HoursPerWeek = null }
            //};
            //var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            //var response = _validator.Validate(viewModel, ruleSet: RuleSet);
            //_aggregateValidator.Validate(vacancyViewModel);
            //var aggregateResponse = _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            //if (expectValid)
            //{
            //    _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.CustomType, viewModel, RuleSet);
            //    _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel);
            //    _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel, RuleSet);
            //}
            //else
            //{
            //    _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.CustomType, viewModel, RuleSet);
            //    _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel);
            //    _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel, RuleSet);

            //    var error = response.Errors.SingleOrDefault(e => e.PropertyName == "Wage.CustomType");
            //    error.Should().NotBeNull();
            //    error?.ErrorMessage.Should().Be("You must select a wage type");
            //    var aggregateError = aggregateResponse.Errors.SingleOrDefault(e => e.PropertyName == "FurtherVacancyDetailsViewModel.Wage.CustomType");
            //    aggregateError.Should().NotBeNull();
            //    aggregateError?.ErrorMessage.Should().Be("You must select a wage type");
            //}
        }

        public void AmountMustBeSetWhenFixedWageTypeIsSelected() { }

        public void AmountRangeMustBeSetWhenWageRangeTypeIsSelected() { }

        public void AmountLowerMustBeGreaterThanMinimumWage() { }

        public void PresetTextMustBeSetWhenLegacyTextTypeIsSelected() { }

        public void ReasonForTypeMustBeSetWhenPresetTextTypeIsSelected() { }
    }
}