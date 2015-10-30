namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Recruit.Validators.Vacancy;
    using ViewModels.Vacancy;

    [TestFixture]
    public class VacancySummaryViewModelServerValidatorTests
    {
        private const string RuleSet = RuleSets.Errors;

        private VacancySummaryViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancySummaryViewModelServerValidator();
        }

        [Test]
        public void DefaultShouldHaveValidationErrors()
        {
            var viewModel = new VacancySummaryViewModel();

            var result = _validator.Validate(viewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            result.IsValid.Should().BeFalse();
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

            _validator.Validate(viewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.WorkingWeek, viewModel, RuleSet);
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
        [TestCase("41", false)]
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

            _validator.Validate(viewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.HoursPerWeek, viewModel, RuleSet);
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

            _validator.Validate(viewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.WageType, viewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.WageType, viewModel, RuleSet);
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

            _validator.Validate(viewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Wage, viewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Wage, viewModel, RuleSet);
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

            _validator.Validate(viewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.Duration, viewModel, RuleSet);
            }
        }

        [Test]
        public void ClosingDateRequired()
        {
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel()
            };

            _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.ClosingDate, viewModel, RuleSet);
        }

        [Test]
        public void PossibleStartDateRequired()
        {
            var viewModel = new VacancySummaryViewModel
            {
                PossibleStartDate = new DateViewModel()
            };

            _validator.Validate(viewModel, ruleSet: RuleSet);

            _validator.ShouldHaveValidationErrorFor(vm => vm.PossibleStartDate, viewModel, RuleSet);
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

            _validator.Validate(viewModel, ruleSet: RuleSet);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.LongDescription, viewModel, RuleSet);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.LongDescription, viewModel, RuleSet);
            }
        }
    }
}