namespace SFA.DAS.RAA.Api.UnitTests.Validators
{
    using Api.Validators;
    using Apprenticeships.Domain.Entities.Vacancies;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using Models;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class WageUpdateValidatorTests
    {
        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, false)]
        [TestCase(WageType.ApprenticeshipMinimum, false)]
        [TestCase(WageType.NationalMinimum, false)]
        [TestCase(WageType.Custom, false)]
        [TestCase(WageType.CustomRange, false)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, false)]
        public void ValidLegacyTextWageTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.LegacyText,
                ExistingAmount = 100
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You cannot change the type of a LegacyText wage.");
            }
        }

        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, true)]
        [TestCase(WageType.ApprenticeshipMinimum, false)]
        [TestCase(WageType.NationalMinimum, false)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.CustomRange, true)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, false)]
        public void ValidLegacyWeeklyWageTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.LegacyWeekly,
                ExistingAmount = 100
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You can only change the type of a LegacyWeekly wage to Custom (fixed) or CustomRange (wage range).");
            }
        }

        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, false)]
        [TestCase(WageType.ApprenticeshipMinimum, true)]
        [TestCase(WageType.NationalMinimum, true)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.CustomRange, true)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, false)]
        public void ValidApprenticeshipMinimumWageTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.ApprenticeshipMinimum,
                ExistingAmount = 100
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You can only change the type of an ApprenticeshipMinimum wage to NationalMinimum, Custom (fixed) or CustomRange (wage range).");
            }
        }

        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, false)]
        [TestCase(WageType.ApprenticeshipMinimum, false)]
        [TestCase(WageType.NationalMinimum, true)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.CustomRange, true)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, false)]
        public void ValidNationalMinimumWageTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.NationalMinimum,
                ExistingAmount = 100
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You can only change the type of a NationalMinimum wage to Custom (fixed) or CustomRange (wage range).");
            }
        }

        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, false)]
        [TestCase(WageType.ApprenticeshipMinimum, false)]
        [TestCase(WageType.NationalMinimum, false)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.CustomRange, true)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, false)]
        public void ValidFixedWageTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.Custom,
                ExistingAmount = 100
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You can only change the type of a Custom (fixed) wage to CustomRange (wage range).");
            }
        }

        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, false)]
        [TestCase(WageType.ApprenticeshipMinimum, false)]
        [TestCase(WageType.NationalMinimum, false)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.CustomRange, true)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, false)]
        public void ValidWageRangeTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.CustomRange,
                ExistingAmount = 100
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You can only change the type of a CustomRange (wage range) wage to Custom (fixed).");
            }
        }

        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, false)]
        [TestCase(WageType.ApprenticeshipMinimum, false)]
        [TestCase(WageType.NationalMinimum, false)]
        [TestCase(WageType.Custom, false)]
        [TestCase(WageType.CustomRange, false)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, false)]
        public void ValidCompetitiveSalaryWageTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.CompetitiveSalary,
                ExistingAmount = 100
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You cannot change the type of a CompetitiveSalary wage.");
            }
        }

        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, false)]
        [TestCase(WageType.ApprenticeshipMinimum, false)]
        [TestCase(WageType.NationalMinimum, false)]
        [TestCase(WageType.Custom, false)]
        [TestCase(WageType.CustomRange, false)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, false)]
        public void ValidToBeAgreedUponAppointmentWageTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.ToBeAgreedUponAppointment,
                ExistingAmount = 100
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You cannot change the type of a ToBeAgreedUponAppointment wage.");
            }
        }

        [TestCase(WageType.LegacyText, false)]
        [TestCase(WageType.LegacyWeekly, false)]
        [TestCase(WageType.ApprenticeshipMinimum, true)]
        [TestCase(WageType.NationalMinimum, true)]
        [TestCase(WageType.Custom, true)]
        [TestCase(WageType.CustomRange, true)]
        [TestCase(WageType.CompetitiveSalary, false)]
        [TestCase(WageType.ToBeAgreedUponAppointment, false)]
        [TestCase(WageType.Unwaged, true)]
        public void ValidUnwagedWageTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingType = WageType.Unwaged,
                ExistingAmount = 100
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You can only change the type of an Unwaged wage to ApprenticeshipMinimum, NationalMinimum, Custom (fixed) or CustomRange (wage range).");
            }
        }
    }
}