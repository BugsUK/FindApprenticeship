namespace SFA.DAS.RAA.Api.UnitTests.Validators
{
    using System;
    using Api.Validators;
    using Apprenticeships.Domain.Entities.Vacancies;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using Models;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class WageUpdateValidatorAmountTests
    {
        [Test]
        public void CustomAmountAndUnitMustBeSpecified()
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = null,
                Unit = null,
                ExistingWage = new Wage { Type = WageType.ApprenticeshipMinimum }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You must specify a valid amount.");
            validator.ShouldHaveValidationErrorFor(wu => wu.Unit, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You must specify a valid wage unit.");
        }

        [Test]
        public void CustomUnitMustBeSpecified()
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = 100,
                Unit = null,
                ExistingWage = new Wage { Type = WageType.NationalMinimum }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(wu => wu.Unit, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You must specify a valid wage unit.");
        }

        [TestCase(WageType.Custom)]
        [TestCase(WageType.CustomRange)]
        public void CustomAmountMustBeSpecified(WageType existingType)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = null,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage { Type = existingType }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().BeFalse();
            if (existingType == WageType.Custom)
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The new fixed wage must be higher than the original figure.");
            }
            if (existingType == WageType.CustomRange)
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The new fixed wage must be higher than the orignal wage range minimum.");
            }
        }

        [TestCase(WageType.Custom)]
        [TestCase(WageType.CustomRange)]
        public void CustomRangeAmountLowerBoundMustBeSpecified(WageType existingType)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.CustomRange,
                AmountLowerBound = null,
                AmountUpperBound = 110,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage { Type = existingType }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().BeFalse();
            if (existingType == WageType.Custom)
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The minimum amount must be higher than the original fixed wage.");
            }
            if (existingType == WageType.CustomRange)
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The minimum amount must be higher than the original amount.");
            }
        }

        [Test]
        public void CustomRangeAmountsAndUnitMustBeSpecified()
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.CustomRange,
                AmountLowerBound = null,
                AmountUpperBound = null,
                Unit = null,
                ExistingWage = new Wage { Type = WageType.ApprenticeshipMinimum }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You must specify a valid minimum amount for the wage range.");
            validator.ShouldHaveValidationErrorFor(wu => wu.AmountUpperBound, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You must specify a valid maximum amount for the wage range.");
            validator.ShouldHaveValidationErrorFor(wu => wu.Unit, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You must specify a valid wage unit.");
        }

        [Test]
        public void CustomRangeUnitMustBeSpecified()
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.CustomRange,
                AmountLowerBound = 100,
                AmountUpperBound = 120,
                Unit = null,
                ExistingWage = new Wage { Type = WageType.NationalMinimum }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(wu => wu.Unit, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("You must specify a valid wage unit.");
        }

        [TestCase(100, 100, false)]
        [TestCase(100, 110, true)]
        [TestCase(110, 100, false)]
        public void CustomRangeAmountsMustBeDifferent(decimal newAmountLowerBound, decimal newAmountUpperBound, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.CustomRange,
                AmountLowerBound = newAmountLowerBound,
                AmountUpperBound = newAmountUpperBound,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage { Type = WageType.CustomRange }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountUpperBound, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountUpperBound, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The maximum amount for the wage range must be higher than the minimum amount.");
            }
        }

        [TestCase(99, false)]
        [TestCase(100, true)]
        [TestCase(101, true)]
        public void CustomWageAmountMustBeGreaterThanOrEqualToExisting(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = newAmount,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.Custom,
                    Amount = 100,
                    Unit = WageUnit.Weekly,
                    HoursPerWeek = 20
                }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The new fixed wage must be higher than the original figure.");
            }
        }

        [TestCase(99, false)]
        [TestCase(100, true)]
        [TestCase(101, true)]
        public void CustomWageAmountMustBeGreaterThanOrEqualToExistingAmountLowerBound(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = newAmount,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.CustomRange,
                    AmountLowerBound = 100,
                    AmountUpperBound = 110,
                    Unit = WageUnit.Weekly,
                    HoursPerWeek = 20
                }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The new fixed wage must be higher than the orignal wage range minimum.");
            }
        }

        [TestCase(99, false)]
        [TestCase(100, true)]
        [TestCase(101, true)]
        public void CustomRangeAmountLowerBoundMustBeGreaterThanOrEqualToExisting(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.CustomRange,
                AmountLowerBound = newAmount,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.CustomRange,
                    AmountLowerBound = 100,
                    AmountUpperBound = 110,
                    Unit = WageUnit.Weekly,
                    HoursPerWeek = 20
                }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The minimum amount must be higher than the original amount.");
            }
        }

        [TestCase(99, false)]
        [TestCase(100, true)]
        [TestCase(101, true)]
        public void CustomRangeAmountLowerBoundMustBeGreaterThanOrEqualToExistingAmount(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.CustomRange,
                AmountLowerBound = newAmount,
                AmountUpperBound = 110,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.Custom,
                    Amount = 100,
                    Unit = WageUnit.Weekly,
                    HoursPerWeek = 20
                }
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The minimum amount must be higher than the original fixed wage.");
            }
        }

        [TestCase(138, false)]
        [TestCase(139, true)]
        [TestCase(140, true)]
        public void CustomWageAmountMustBeGreaterThanOrEqualToNationalMinimum(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = newAmount,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.NationalMinimum,
                    HoursPerWeek = 20
                },
                PossibleStartDate = new DateTime(2016, 12, 1)
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The new fixed wage must be higher than £139.00.");
            }
        }

        [TestCase(138, false)]
        [TestCase(139, true)]
        [TestCase(140, true)]
        public void CustomRangeAmountLowerBoundMustBeGreaterThanOrEqualToNationalMinimum(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.CustomRange,
                AmountLowerBound = newAmount,
                AmountUpperBound = 150,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.NationalMinimum,
                    HoursPerWeek = 20
                },
                PossibleStartDate = new DateTime(2016, 12, 1)
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The minimum amount must be higher than £139.00.");
            }
        }

        [TestCase(67, false)]
        [TestCase(68, true)]
        [TestCase(69, true)]
        public void CustomWageAmountMustBeGreaterThanOrEqualToApprenticeshipMinimum(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = newAmount,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.ApprenticeshipMinimum,
                    HoursPerWeek = 20
                },
                PossibleStartDate = new DateTime(2016, 12, 1)
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The new fixed wage must be higher than £68.00.");
            }
        }

        [TestCase(67, false)]
        [TestCase(68, true)]
        [TestCase(69, true)]
        public void CustomRangeAmountLowerBoundMustBeGreaterThanOrEqualToApprenticeshipMinimum(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.CustomRange,
                AmountLowerBound = newAmount,
                AmountUpperBound = 150,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.ApprenticeshipMinimum,
                    HoursPerWeek = 20
                },
                PossibleStartDate = new DateTime(2016, 12, 1)
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The minimum amount must be higher than £68.00.");
            }
        }

        [TestCase(67, false)]
        [TestCase(68, true)]
        [TestCase(69, true)]
        public void CustomWageAmountMustBeGreaterThanOrEqualToApprenticeshipMinimumWhenOriginallyUnwaged(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.Custom,
                Amount = newAmount,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.Unwaged,
                    HoursPerWeek = 20
                },
                PossibleStartDate = new DateTime(2016, 12, 1)
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The new fixed wage must be higher than £68.00.");
            }
        }

        [TestCase(67, false)]
        [TestCase(68, true)]
        [TestCase(69, true)]
        public void CustomRangeAmountLowerBoundMustBeGreaterThanOrEqualToApprenticeshipMinimumWhenOriginallyUnwaged(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new WageUpdate
            {
                Type = WageType.CustomRange,
                AmountLowerBound = newAmount,
                AmountUpperBound = 150,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.ApprenticeshipMinimum,
                    HoursPerWeek = 20
                },
                PossibleStartDate = new DateTime(2016, 12, 1)
            };

            var validator = new WageUpdateValidator();

            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate, WageUpdateValidator.CompareWithExisting).WithErrorMessage("The minimum amount must be higher than £68.00.");
            }
        }
    }
}