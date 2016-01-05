namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Builders;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
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
            var viewModel = new VacancySummaryViewModel();
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            var result = _validator.Validate(viewModel, ruleSet: RuleSet);
            var aggregateResults = _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSet);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().BeGreaterThan(5);
            aggregateResults.IsValid.Should().BeFalse();
            aggregateResults.Errors.Count.Should().BeGreaterThan(5);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("Mon-Fri", true)]
        public void WorkingWeekRequired(string workingWeek, bool expectValid)
        {
            var viewModel = new VacancySummaryViewModel
            {
                WorkingWeek = workingWeek
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
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WorkingWeek, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WorkingWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WorkingWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WorkingWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WorkingWeek, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WorkingWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WorkingWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WorkingWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
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
        public void HoursPerWeekRequired(string hoursPerWeekString, bool expectValid)
        {
            decimal? hoursPerWeek = null;
            decimal parsedHoursPerWeek;
            if (decimal.TryParse(hoursPerWeekString, out parsedHoursPerWeek))
            {
                hoursPerWeek = parsedHoursPerWeek;
            }
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = hoursPerWeek
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.HoursPerWeek, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.HoursPerWeek, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(0, false)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.ApprenticeshipMinimumWage, true)]
        [TestCase(WageType.NationalMinimumWage, true)]
        public void WageTypeRequired(WageType wageType, bool expectValid)
        {
            var viewModel = new VacancySummaryViewModel
            {
                WageType = wageType
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.WageType, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WageType, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WageType, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WageType, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WageType, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.WageType, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WageType, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WageType, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WageType, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.WageType, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, WageType.Custom, false)]
        [TestCase(null, WageType.ApprenticeshipMinimumWage, true)]
        [TestCase(null, WageType.NationalMinimumWage, true)]
        [TestCase("", WageType.Custom, false)]
        [TestCase("", WageType.ApprenticeshipMinimumWage, true)]
        [TestCase("", WageType.NationalMinimumWage, true)]
        [TestCase(" ", WageType.Custom, false)]
        [TestCase(" ", WageType.ApprenticeshipMinimumWage, true)]
        [TestCase(" ", WageType.NationalMinimumWage, true)]
        [TestCase("Seven pounds an hour", WageType.Custom, false)]
        [TestCase("Seven pounds an hour", WageType.ApprenticeshipMinimumWage, true)]
        [TestCase("Seven pounds an hour", WageType.NationalMinimumWage, true)]
        [TestCase("<script>", WageType.Custom, false)]
        [TestCase("500", WageType.Custom, true)]
        [TestCase("500", WageType.ApprenticeshipMinimumWage, true)]
        [TestCase("500", WageType.NationalMinimumWage, true)]
        public void WageRequired(string wageString, WageType wageType, bool expectValid)
        {
            decimal? wage = null;
            decimal parsedWage;
            if (decimal.TryParse(wageString, out parsedWage))
            {
                wage = parsedWage;
            }
            var viewModel = new VacancySummaryViewModel
            {
                HoursPerWeek = 37.5m,
                Wage = wage,
                WageType = wageType,
                WageUnit = wageType == WageType.Custom ? WageUnit.Weekly : WageUnit.NotApplicable
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, viewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Wage, vacancyViewModel, RuleSets.ErrorsAndWarnings);
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
            var viewModel = new VacancySummaryViewModel
            {
                Duration = duration
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
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Duration, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Duration, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Duration, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Duration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Duration, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Duration, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Duration, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.Duration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [Test]
        public void ClosingDateRequired()
        {
            var viewModel = new VacancySummaryViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel()
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _validator.ShouldHaveValidationErrorFor(vm => vm.VacancyDatesViewModel.ClosingDate, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Errors);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Warnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);
        }

        [Test]
        public void PossibleStartDateRequired()
        {
            var viewModel = new VacancySummaryViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    PossibleStartDate = new DateViewModel()
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _validator.ShouldHaveValidationErrorFor(vm => vm.VacancyDatesViewModel.PossibleStartDate, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Errors);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Warnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);
        }

        [Test]
        public void ClosingDateMustBeAValidDateRequired()
        {
            var viewModel = new VacancySummaryViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel
                    {
                        Day = 31,
                        Month = 11,
                        Year = 2015
                    }
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _validator.ShouldHaveValidationErrorFor(vm => vm.VacancyDatesViewModel.ClosingDate, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Errors);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.Warnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);
        }

        [Test]
        public void PossibleStartMustBeAValidDateDateRequired()
        {
            var viewModel = new VacancySummaryViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    PossibleStartDate = new DateViewModel
                    {
                        Day = 31,
                        Month = 11,
                        Year = 2015
                    }
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            _validator.ShouldHaveValidationErrorFor(vm => vm.VacancyDatesViewModel.PossibleStartDate, viewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Errors);
            _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.Warnings);
            _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate, vacancyViewModel, RuleSets.ErrorsAndWarnings);
        }

        [TestCase(16, false)]
        [TestCase(2016, true)]
        public void ClosingDateYearFormat(int year, bool expectValid)
        {
            var viewModel = new VacancySummaryViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel
                    {
                        /*Day = 1,
                        Month = 2,*/
                        Year = year
                    }
                }
            };
            var vacancyViewModel = new VacancyViewModelBuilder().With(viewModel).Build();

            _validator.Validate(viewModel, ruleSet: RuleSet);
            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyDatesViewModel.ClosingDate, vm => vm.VacancyDatesViewModel.ClosingDate.Year, viewModel, RuleSet);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.VacancyDatesViewModel.ClosingDate, vm => vm.VacancyDatesViewModel.ClosingDate.Year, viewModel, RuleSet);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate, vm => vm.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Year, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        [TestCase("Description", true)]
        public void LongDescriptionRequired(string longDescription, bool expectValid)
        {
            var viewModel = new VacancySummaryViewModel
            {
                LongDescription = longDescription
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
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.LongDescription, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.LongDescription, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.LongDescription, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.LongDescription, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.LongDescription, viewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.LongDescription, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.LongDescription, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.LongDescription, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancySummaryViewModel, vm => vm.VacancySummaryViewModel.LongDescription, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }
    }
}