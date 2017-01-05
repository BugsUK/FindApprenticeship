namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators
{
    using System;
    using Common.ViewModels;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.VacancyManagement;
    using Recruit.Validators;

    [TestFixture]
    [Parallelizable]
    public class EditWageViewModelValidatorTests
    {
        [Test]
        public void MustSelectAClassification()
        {
            var viewModel = new EditWageViewModel
            {
                Classification = WageClassification.NotApplicable,
                ExistingWage = new Wage
                {
                    Type = WageType.NationalMinimum
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(viewModel);

            validationResult.IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(wu => wu.Classification, viewModel).WithErrorMessage(VacancyViewModelMessages.WageClassification.RequiredErrorText);
        }

        [Test]
        public void MustSelectACustomType()
        {
            var viewModel = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.NotApplicable,
                ExistingWage = new Wage
                {
                    Type = WageType.NationalMinimum
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(viewModel);

            validationResult.IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(wu => wu.CustomType, viewModel).WithErrorMessage(VacancyViewModelMessages.CustomWageType.RequiredErrorText);
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
        public void ValidLegacyTextWageTypeChanges(WageType newWageType, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.LegacyText,
                    Amount = 100
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate).WithErrorMessage("You cannot change the type of a LegacyText wage.");
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
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Fixed,
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.LegacyWeekly,
                    Amount = 100
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate).WithErrorMessage("You can only change the type of a LegacyWeekly wage to Custom (fixed) or CustomRange (wage range).");
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
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.ApprenticeshipMinimum,
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.ApprenticeshipMinimum,
                    Amount = 100
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate).WithErrorMessage("You can only change the type of an ApprenticeshipMinimum wage to NationalMinimum, Custom (fixed) or CustomRange (wage range).");
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
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.NationalMinimum,
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.NationalMinimum,
                    Amount = 100
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate).WithErrorMessage("You can only change the type of a NationalMinimum wage to Custom (fixed) or CustomRange (wage range).");
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
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Fixed,
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.Custom,
                    Amount = 100
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate).WithErrorMessage("You can only change the type of a Custom (fixed) wage to CustomRange (wage range).");
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
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Ranged,
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.CustomRange,
                    Amount = 100
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate).WithErrorMessage("You can only change the type of a CustomRange (wage range) wage to Custom (fixed).");
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
            var wageUpdate = new EditWageViewModel
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.CompetitiveSalary,
                    Amount = 100
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate).WithErrorMessage("You cannot change the type of a CompetitiveSalary wage.");
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
            var wageUpdate = new EditWageViewModel
            {
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.ToBeAgreedUponAppointment,
                    Amount = 100
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate).WithErrorMessage("You cannot change the type of a ToBeAgreedUponAppointment wage.");
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
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.PresetText,
                Type = newWageType,
                Amount = 110,
                AmountLowerBound = 110,
                AmountUpperBound = 120,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage
                {
                    Type = WageType.Unwaged,
                    Amount = 100
                }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Type, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Type, wageUpdate).WithErrorMessage("You can only change the type of an Unwaged wage to ApprenticeshipMinimum, NationalMinimum, Custom (fixed) or CustomRange (wage range).");
            }
        }

        [Test]
        public void CustomAmountAndUnitMustBeSpecified()
        {
            var wageUpdate = new EditWageViewModel
            {
                Type = WageType.Custom,
                Amount = null,
                Unit = null,
                ExistingWage = new Wage { Type = WageType.ApprenticeshipMinimum }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate).WithErrorMessage("You must specify a valid amount.");
            validator.ShouldHaveValidationErrorFor(wu => wu.Unit, wageUpdate).WithErrorMessage("You must specify a valid wage unit.");
        }

        [Test]
        public void CustomUnitMustBeSpecified()
        {
            var wageUpdate = new EditWageViewModel
            {
                Type = WageType.Custom,
                Amount = 100,
                Unit = null,
                ExistingWage = new Wage { Type = WageType.NationalMinimum }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(wu => wu.Unit, wageUpdate).WithErrorMessage("You must specify a valid wage unit.");
        }

        [TestCase(WageType.Custom)]
        [TestCase(WageType.CustomRange)]
        public void CustomAmountMustBeSpecified(WageType existingType)
        {
            var wageUpdate = new EditWageViewModel
            {
                Type = WageType.Custom,
                Amount = null,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage { Type = existingType }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().BeFalse();
            if (existingType == WageType.Custom)
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate).WithErrorMessage("The new fixed wage must be higher than the original figure.");
            }
            if (existingType == WageType.CustomRange)
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate).WithErrorMessage("The new fixed wage must be higher than the orignal wage range minimum.");
            }
        }

        [TestCase(WageType.Custom)]
        [TestCase(WageType.CustomRange)]
        public void CustomRangeAmountLowerBoundMustBeSpecified(WageType existingType)
        {
            var wageUpdate = new EditWageViewModel
            {
                Type = WageType.CustomRange,
                AmountLowerBound = null,
                AmountUpperBound = 110,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage { Type = existingType }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().BeFalse();
            if (existingType == WageType.Custom)
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate).WithErrorMessage("The minimum amount must be higher than the original fixed wage.");
            }
            if (existingType == WageType.CustomRange)
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate).WithErrorMessage("The minimum amount must be higher than the original amount.");
            }
        }

        [Test]
        public void CustomRangeAmountsAndUnitMustBeSpecified()
        {
            var wageUpdate = new EditWageViewModel
            {
                Type = WageType.CustomRange,
                AmountLowerBound = null,
                AmountUpperBound = null,
                Unit = null,
                ExistingWage = new Wage { Type = WageType.ApprenticeshipMinimum }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate).WithErrorMessage("You must specify a valid minimum amount for the wage range.");
            validator.ShouldHaveValidationErrorFor(wu => wu.AmountUpperBound, wageUpdate).WithErrorMessage("You must specify a valid maximum amount for the wage range.");
            validator.ShouldHaveValidationErrorFor(wu => wu.Unit, wageUpdate).WithErrorMessage("You must specify a valid wage unit.");
        }

        [Test]
        public void CustomRangeUnitMustBeSpecified()
        {
            var wageUpdate = new EditWageViewModel
            {
                Type = WageType.CustomRange,
                AmountLowerBound = 100,
                AmountUpperBound = 120,
                Unit = null,
                ExistingWage = new Wage { Type = WageType.NationalMinimum }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().BeFalse();
            validator.ShouldHaveValidationErrorFor(wu => wu.Unit, wageUpdate).WithErrorMessage("You must specify a valid wage unit.");
        }

        [TestCase(100, 100, false)]
        [TestCase(100, 110, true)]
        [TestCase(110, 100, false)]
        public void CustomRangeAmountsMustBeDifferent(decimal newAmountLowerBound, decimal newAmountUpperBound, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Ranged,
                Type = WageType.CustomRange,
                AmountLowerBound = newAmountLowerBound,
                AmountUpperBound = newAmountUpperBound,
                Unit = WageUnit.Weekly,
                ExistingWage = new Wage { Type = WageType.CustomRange }
            };

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountUpperBound, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountUpperBound, wageUpdate).WithErrorMessage("The maximum amount for the wage range must be higher than the minimum amount.");
            }
        }

        [TestCase(99, false)]
        [TestCase(100, true)]
        [TestCase(101, true)]
        public void CustomWageAmountMustBeGreaterThanOrEqualToExisting(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Fixed,
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

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Amount, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate).WithErrorMessage("The new fixed wage must be higher than the original figure.");
            }
        }

        [TestCase(99, false)]
        [TestCase(100, true)]
        [TestCase(101, true)]
        public void CustomWageAmountMustBeGreaterThanOrEqualToExistingAmountLowerBound(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Ranged,
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

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Amount, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate).WithErrorMessage("The new fixed wage must be higher than the orignal wage range minimum.");
            }
        }

        [TestCase(99, false)]
        [TestCase(100, true)]
        [TestCase(101, true)]
        public void CustomRangeAmountLowerBoundMustBeGreaterThanOrEqualToExisting(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Ranged,
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

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate).WithErrorMessage("The minimum amount must be higher than the original amount.");
            }
        }

        [TestCase(99, false)]
        [TestCase(100, true)]
        [TestCase(101, true)]
        public void CustomRangeAmountLowerBoundMustBeGreaterThanOrEqualToExistingAmount(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Ranged,
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

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate).WithErrorMessage("The minimum amount must be higher than the original fixed wage.");
            }
        }

        [TestCase(138, false)]
        [TestCase(139, true)]
        [TestCase(140, true)]
        public void CustomWageAmountMustBeGreaterThanOrEqualToNationalMinimum(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Fixed,
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

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Amount, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate).WithErrorMessage("The new fixed wage must be higher than £139.00.");
            }
        }

        [TestCase(138, false)]
        [TestCase(139, true)]
        [TestCase(140, true)]
        public void CustomRangeAmountLowerBoundMustBeGreaterThanOrEqualToNationalMinimum(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Ranged,
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

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate).WithErrorMessage("The minimum amount must be higher than £139.00.");
            }
        }

        [TestCase(67, false)]
        [TestCase(68, true)]
        [TestCase(69, true)]
        public void CustomWageAmountMustBeGreaterThanOrEqualToApprenticeshipMinimum(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Fixed,
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

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Amount, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate).WithErrorMessage("The new fixed wage must be higher than £68.00.");
            }
        }

        [TestCase(67, false)]
        [TestCase(68, true)]
        [TestCase(69, true)]
        public void CustomRangeAmountLowerBoundMustBeGreaterThanOrEqualToApprenticeshipMinimum(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Ranged,
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

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate).WithErrorMessage("The minimum amount must be higher than £68.00.");
            }
        }

        [TestCase(67, false)]
        [TestCase(68, true)]
        [TestCase(69, true)]
        public void CustomWageAmountMustBeGreaterThanOrEqualToApprenticeshipMinimumWhenOriginallyUnwaged(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Fixed,
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

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.Amount, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.Amount, wageUpdate).WithErrorMessage("The new fixed wage must be higher than £68.00.");
            }
        }

        [TestCase(67, false)]
        [TestCase(68, true)]
        [TestCase(69, true)]
        public void CustomRangeAmountLowerBoundMustBeGreaterThanOrEqualToApprenticeshipMinimumWhenOriginallyUnwaged(decimal newAmount, bool expectedIsValid)
        {
            var wageUpdate = new EditWageViewModel
            {
                Classification = WageClassification.Custom,
                CustomType = CustomWageType.Ranged,
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

            var validator = new EditWageViewModelValidator();

            var validationResult = validator.Validate(wageUpdate);

            validationResult.IsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                validator.ShouldNotHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(wu => wu.AmountLowerBound, wageUpdate).WithErrorMessage("The minimum amount must be higher than £68.00.");
            }
        }
    }
}