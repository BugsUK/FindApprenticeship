namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System.Text;
    using Builders;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
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
        [TestCase(WageClassification.Custom, true)]
        [TestCase(WageClassification.ApprenticeshipMinimum, true)]
        [TestCase(WageClassification.NationalMinimum, true)]
        public void WageClassificationRequired(WageClassification wageType, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = wageType }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Classification, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.Classification, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(0, true)]
        [TestCase(WageClassification.Custom, true)]
        [TestCase(WageClassification.ApprenticeshipMinimum, true)]
        [TestCase(WageClassification.NationalMinimum, true)]
        public void WageTypeNotRequiredIfTraineeship(WageClassification wageClassification, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = wageClassification },
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
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage.Classification, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage.Classification, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.Classification, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, WageClassification.Custom, false)]
        [TestCase(null, WageClassification.ApprenticeshipMinimum, true)]
        [TestCase(null, WageClassification.NationalMinimum, true)]
        [TestCase("", WageClassification.Custom, false)]
        [TestCase("", WageClassification.ApprenticeshipMinimum, true)]
        [TestCase("", WageClassification.NationalMinimum, true)]
        [TestCase(" ", WageClassification.Custom, false)]
        [TestCase(" ", WageClassification.ApprenticeshipMinimum, true)]
        [TestCase(" ", WageClassification.NationalMinimum, true)]
        [TestCase("Seven pounds an hour", WageClassification.Custom, false)]
        [TestCase("Seven pounds an hour", WageClassification.ApprenticeshipMinimum, true)]
        [TestCase("Seven pounds an hour", WageClassification.NationalMinimum, true)]
        [TestCase("<script>", WageClassification.Custom, false)]
        [TestCase("500", WageClassification.Custom, true)]
        [TestCase("500", WageClassification.ApprenticeshipMinimum, true)]
        [TestCase("500", WageClassification.NationalMinimum, true)]
        public void WageRequired(string wageString, WageClassification wageClassification, bool expectValid)
        {
            decimal? wage = null;
            decimal parsedWage;
            if (decimal.TryParse(wageString, out parsedWage))
            {
                wage = parsedWage;
            }
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = wageClassification, CustomType = wageClassification == WageClassification.Custom ? CustomWageType.Fixed : CustomWageType.NotApplicable, Amount = wage, Unit = wageClassification == WageClassification.Custom ? WageUnit.Weekly : WageUnit.NotApplicable, HoursPerWeek = 37.5m }
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

        [TestCase(null, WageClassification.Custom, true)]
        [TestCase(null, WageClassification.ApprenticeshipMinimum, true)]
        [TestCase(null, WageClassification.NationalMinimum, true)]
        [TestCase("", WageClassification.Custom, true)]
        [TestCase("", WageClassification.ApprenticeshipMinimum, true)]
        [TestCase("", WageClassification.NationalMinimum, true)]
        [TestCase(" ", WageClassification.Custom, true)]
        [TestCase(" ", WageClassification.ApprenticeshipMinimum, true)]
        [TestCase(" ", WageClassification.NationalMinimum, true)]
        [TestCase("Seven pounds an hour", WageClassification.Custom, true)]
        [TestCase("Seven pounds an hour", WageClassification.ApprenticeshipMinimum, true)]
        [TestCase("Seven pounds an hour", WageClassification.NationalMinimum, true)]
        [TestCase("<script>", WageClassification.Custom, true)]
        [TestCase("500", WageClassification.Custom, true)]
        [TestCase("500", WageClassification.ApprenticeshipMinimum, true)]
        [TestCase("500", WageClassification.NationalMinimum, true)]
        public void WageNotRequiredIfTraineeship(string wageString, WageClassification wageClassification, bool expectValid)
        {
            decimal? wage = null;
            decimal parsedWage;
            if (decimal.TryParse(wageString, out parsedWage))
            {
                wage = parsedWage;
            }
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = wageClassification, Amount = wage, Unit = wageClassification == WageClassification.Custom ? WageUnit.Weekly : WageUnit.NotApplicable, HoursPerWeek = 37.5m },
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

        [TestCase(CustomWageType.Fixed, true)]
        [TestCase(CustomWageType.Ranged, true)]
        [TestCase(CustomWageType.NotApplicable, false)]
        public void CustomTypeMustBeSetWhenCustomWageTypeIsSelected(CustomWageType selectedCustomType, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = WageClassification.Custom, CustomType = selectedCustomType}
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.CustomType, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.CustomType, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.CustomType, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(CustomWageType.Fixed, null, false)]
        [TestCase(CustomWageType.Fixed, 1, true)]
        public void AmountMustBeSetWhenFixedWageTypeIsSelected(CustomWageType customWageType, decimal? amount, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = WageClassification.Custom, CustomType = customWageType, Amount = amount}
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

        [TestCase(CustomWageType.Fixed, null, null, true)]
        [TestCase(CustomWageType.Fixed, 10, null, true)]
        [TestCase(CustomWageType.Fixed, 10, 20, true)]
        [TestCase(CustomWageType.Fixed, null, 20, true)]
        [TestCase(CustomWageType.Ranged, null, null, false)]
        [TestCase(CustomWageType.Ranged, 10, null, true)]
        [TestCase(CustomWageType.Ranged, 10, 20, true)]
        [TestCase(CustomWageType.Ranged, null, 20, false)]
        public void AmountLowerBoundMustBeSetWhenWageRangeTypeIsSelected(CustomWageType customWageType, decimal? amountLower, decimal? amountUpper, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = WageClassification.Custom, CustomType = customWageType, AmountLowerBound = amountLower, AmountUpperBound = amountUpper}
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.AmountLowerBound, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.AmountLowerBound, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(CustomWageType.Fixed, null, null, true)]
        [TestCase(CustomWageType.Fixed, 10, null, true)]
        [TestCase(CustomWageType.Fixed, 10, 20, true)]
        [TestCase(CustomWageType.Fixed, null, 20, true)]
        [TestCase(CustomWageType.Ranged, null, null, false)]
        [TestCase(CustomWageType.Ranged, 10, null, false)]
        [TestCase(CustomWageType.Ranged, 10, 20, true)]
        [TestCase(CustomWageType.Ranged, null, 20, true)]
        public void AmountUpperBoundMustBeSetWhenWageRangeTypeIsSelected(CustomWageType customWageType, decimal? amountLower, decimal? amountUpper, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = WageClassification.Custom, CustomType = customWageType, AmountLowerBound = amountLower, AmountUpperBound = amountUpper }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.AmountUpperBound, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.AmountUpperBound, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(1, 40, WageUnit.Weekly, false)]
        [TestCase(1, 40, WageUnit.Monthly, false)]
        [TestCase(1, 40, WageUnit.Annually, false)]
        [TestCase(1, 40, WageUnit.NotApplicable, true)]
        [TestCase(100, 40, WageUnit.Weekly, false)]
        [TestCase(100, 40, WageUnit.Monthly, false)]
        [TestCase(100, 40, WageUnit.Annually, false)]
        [TestCase(1000, 40, WageUnit.Weekly, true)]
        [TestCase(2000, 40, WageUnit.Monthly, true)]
        [TestCase(20000, 40, WageUnit.Annually, true)]
        public void AmountLowerMustBeGreaterThanMinimumWage(decimal? amount, decimal? hoursPerWeek, WageUnit rangeUnit, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = WageClassification.Custom, CustomType = CustomWageType.Ranged, AmountLowerBound = amount, HoursPerWeek = hoursPerWeek, RangeUnit = rangeUnit}
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.AmountLowerBound, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.AmountLowerBound, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountLowerBound, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(0, 1, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 2, true)]
        [TestCase(2, 1, false)]
        [TestCase(1, 1, false)]
        public void AmountUpperMustBeMoreThanAmountLower(decimal? amountLower, decimal? amountUpper, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = WageClassification.Custom, CustomType = CustomWageType.Ranged, AmountLowerBound = amountLower, AmountUpperBound = amountUpper }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.AmountUpperBound, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.AmountUpperBound, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.AmountUpperBound, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(WageClassification.PresetText, PresetText.NotApplicable, false)]
        [TestCase(WageClassification.PresetText, PresetText.CompetitiveSalary, true)]
        [TestCase(WageClassification.PresetText, PresetText.ToBeAgreedUponAppointment, true)]
        [TestCase(WageClassification.PresetText, PresetText.Unwaged, true)]
        [TestCase(WageClassification.Custom, PresetText.NotApplicable, true)]
        [TestCase(WageClassification.ApprenticeshipMinimum, PresetText.NotApplicable, true)]
        [TestCase(WageClassification.NationalMinimum, PresetText.NotApplicable, true)]
        [TestCase(WageClassification.LegacyText, PresetText.NotApplicable, true)]
        public void PresetTextMustBeSetWhenLegacyTextTypeIsSelected(WageClassification classification, PresetText presetText, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = classification, PresetText = presetText }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.PresetText, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.PresetText, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.PresetText, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.PresetText, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.PresetText, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.PresetText, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.PresetText, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.PresetText, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.PresetText, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.PresetText, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(PresetText.NotApplicable, null, true)]
        [TestCase(PresetText.NotApplicable, "valid string", true)]
        [TestCase(PresetText.CompetitiveSalary, null, false)]
        [TestCase(PresetText.CompetitiveSalary, "valid string", true)]
        [TestCase(PresetText.ToBeAgreedUponAppointment, null, false)]
        [TestCase(PresetText.ToBeAgreedUponAppointment, "valid string", true)]
        [TestCase(PresetText.Unwaged, null, false)]
        [TestCase(PresetText.Unwaged, "valid string", true)]
        public void ReasonForTypeMustBeSetWhenPresetTextTypeIsSelected(PresetText presetText, string reasonText, bool expectValid)
        {
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = WageClassification.PresetText, PresetText = presetText, WageTypeReason = reasonText }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.WageTypeReason, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.WageTypeReason, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(239, true)]
        [TestCase(240, true)]
        [TestCase(241, false)]
        public void ReasonForTypeMustBeBetween1And240characters(int textLength, bool expectValid)
        {
            var reasonText = new StringBuilder();

            for (int i = 1; i <= textLength; i++)
            {
                reasonText.Append('a');
            }

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel() { Classification = WageClassification.PresetText, PresetText = PresetText.CompetitiveSalary, WageTypeReason = reasonText.ToString()}
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.WageTypeReason, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, vm => vm.Wage.WageTypeReason, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Wage, vm => vm.FurtherVacancyDetailsViewModel.Wage.WageTypeReason, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }
    }
}