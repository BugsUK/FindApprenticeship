namespace SFA.DAS.RAA.Api.Validators
{
    using Apprenticeships.Domain.Entities.Vacancies;
    using Apprenticeships.Infrastructure.Presentation.Constants;
    using Controllers;
    using FluentValidation;
    using FluentValidation.Results;
    using Models;

    public class WageUpdateValidator : AbstractValidator<WageUpdate>
    {
        public const string CompareWithExisting = "CompareWithExisting";

        public WageUpdateValidator()
        {
            RuleSet(CompareWithExisting, () =>
            {
                Custom(WageTypeValidation);

                Custom(WageAmountRequiredValidation);
                Custom(WageAmountLowerBoundRequiredValidation);
                Custom(WageAmountUpperBoundRequiredValidation);
                Custom(WageUnitRequiredValidation);
                Custom(WageRangeBasicValidation);

                Custom(WageAmountValidation);
            });
        }

        private static ValidationFailure WageTypeValidation(WageUpdate wageUpdate)
        {
            if (!wageUpdate.Type.HasValue)
            {
                return null;
            }

            const string propertyName = "Type";
            if (wageUpdate.ExistingType == WageType.LegacyText)
            {
                return new ValidationFailure(propertyName, WageUpdateMessages.Type.CannotChangeLegacyTextType);
            }

            if (wageUpdate.ExistingType == WageType.LegacyWeekly)
            {
                if (wageUpdate.Type != WageType.LegacyWeekly && wageUpdate.Type != WageType.Custom && wageUpdate.Type != WageType.CustomRange)
                {
                    return new ValidationFailure(propertyName, WageUpdateMessages.Type.InvalidLegacyWeeklyTypeChange);
                }
            }

            if (wageUpdate.ExistingType == WageType.ApprenticeshipMinimum)
            {
                if (wageUpdate.Type != WageType.ApprenticeshipMinimum && wageUpdate.Type != WageType.NationalMinimum && wageUpdate.Type != WageType.Custom && wageUpdate.Type != WageType.CustomRange)
                {
                    return new ValidationFailure(propertyName, WageUpdateMessages.Type.InvalidApprenticeshipMinimumTypeChange);
                }
            }

            if (wageUpdate.ExistingType == WageType.NationalMinimum)
            {
                if (wageUpdate.Type != WageType.NationalMinimum && wageUpdate.Type != WageType.Custom && wageUpdate.Type != WageType.CustomRange)
                {
                    return new ValidationFailure(propertyName, WageUpdateMessages.Type.InvalidNationalMinimumTypeChange);
                }
            }

            if (wageUpdate.ExistingType == WageType.Custom || wageUpdate.ExistingType == WageType.CustomRange)
            {
                if (wageUpdate.Type != WageType.Custom && wageUpdate.Type != WageType.CustomRange)
                {
                    return new ValidationFailure(propertyName,
                        wageUpdate.ExistingType == WageType.Custom
                            ? WageUpdateMessages.Type.InvalidCustomTypeChange
                            : WageUpdateMessages.Type.InvalidCustomRangeTypeChange);
                }
            }

            if (wageUpdate.ExistingType == WageType.CompetitiveSalary)
            {
                return new ValidationFailure(propertyName, WageUpdateMessages.Type.InvalidCompetitiveSalaryTypeChange);
            }

            if (wageUpdate.ExistingType == WageType.ToBeAgreedUponAppointment)
            {
                return new ValidationFailure(propertyName, WageUpdateMessages.Type.InvalidToBeAgreedUponAppointmentTypeChange);
            }

            if (wageUpdate.ExistingType == WageType.Unwaged)
            {
                if (wageUpdate.Type != WageType.Unwaged && wageUpdate.Type != WageType.ApprenticeshipMinimum && wageUpdate.Type != WageType.NationalMinimum && wageUpdate.Type != WageType.Custom && wageUpdate.Type != WageType.CustomRange)
                {
                    return new ValidationFailure(propertyName, WageUpdateMessages.Type.InvalidUnwagedTypeChange);
                }
            }

            return null;
        }

        private static ValidationFailure WageAmountRequiredValidation(WageUpdate wageUpdate)
        {
            var wageType = wageUpdate.Type ?? wageUpdate.ExistingType;
            var amount = wageUpdate.Amount ?? wageUpdate.ExistingAmount;

            if (wageType == WageType.Custom)
            {
                if (!amount.HasValue)
                {
                    if (wageUpdate.ExistingType == WageType.Custom)
                    {
                        return new ValidationFailure("Amount", WageUpdateMessages.Amount.InvalidCustomAmount);
                    }
                    if (wageUpdate.ExistingType == WageType.CustomRange)
                    {
                        return new ValidationFailure("Amount", WageUpdateMessages.Amount.InvalidCustomRangeAmount);
                    }
                    return new ValidationFailure("Amount", WageUpdateMessages.Amount.MissingCustomAmount);
                }
            }

            return null;
        }

        private static ValidationFailure WageAmountLowerBoundRequiredValidation(WageUpdate wageUpdate)
        {
            var wageType = wageUpdate.Type ?? wageUpdate.ExistingType;
            var amountLowerBound = wageUpdate.AmountLowerBound ?? wageUpdate.ExistingAmountLowerBound;

            if (wageType == WageType.CustomRange)
            {
                if (!amountLowerBound.HasValue)
                {
                    if (wageUpdate.ExistingType == WageType.CustomRange)
                    {
                        return new ValidationFailure("AmountLowerBound", WageUpdateMessages.AmountLowerBound.InvalidCustomAmountLowerBound);
                    }
                    if (wageUpdate.ExistingType == WageType.Custom)
                    {
                        return new ValidationFailure("AmountLowerBound", WageUpdateMessages.AmountLowerBound.InvalidCustomRangeAmountLowerBound);
                    }
                    return new ValidationFailure("AmountLowerBound", WageUpdateMessages.AmountLowerBound.MissingCustomAmountLowerBound);
                }
            }

            return null;
        }

        private static ValidationFailure WageAmountUpperBoundRequiredValidation(WageUpdate wageUpdate)
        {
            var wageType = wageUpdate.Type ?? wageUpdate.ExistingType;
            var amountUpperBound = wageUpdate.AmountUpperBound ?? wageUpdate.ExistingAmountUpperBound;

            if (wageType == WageType.CustomRange)
            {
                if (!amountUpperBound.HasValue)
                {
                    return new ValidationFailure("AmountUpperBound", WageUpdateMessages.AmountUpperBound.MissingCustomAmountUpperBound);
                }
            }

            return null;
        }

        private static ValidationFailure WageUnitRequiredValidation(WageUpdate wageUpdate)
        {
            var wageType = wageUpdate.Type ?? wageUpdate.ExistingType;
            var wageUnit = wageUpdate.Unit ?? wageUpdate.ExistingUnit;

            if (wageType == WageType.Custom)
            {
                if (wageUnit == 0 || wageUnit == WageUnit.NotApplicable)
                {
                    return new ValidationFailure("Unit", WageUpdateMessages.Unit.MissingUnit);
                }
            }
            if (wageType == WageType.CustomRange)
            {
                if (wageUnit == 0 || wageUnit == WageUnit.NotApplicable)
                {
                    return new ValidationFailure("Unit", WageUpdateMessages.Unit.MissingUnit);
                }
            }

            return null;
        }

        private static ValidationFailure WageRangeBasicValidation(WageUpdate wageUpdate)
        {
            var wageType = wageUpdate.Type ?? wageUpdate.ExistingType;
            var amountLowerBound = wageUpdate.AmountLowerBound ?? wageUpdate.ExistingAmountLowerBound;
            var amountUpperBound = wageUpdate.AmountUpperBound ?? wageUpdate.ExistingAmountUpperBound;

            if (wageType == WageType.CustomRange)
            {
                if (amountLowerBound.HasValue && amountUpperBound.HasValue && amountUpperBound <= amountLowerBound)
                {
                    return new ValidationFailure("AmountUpperBound", WageUpdateMessages.AmountUpperBound.AmountUpperBoundShouldBeGreaterThanAmountLowerBound);
                }
            }

            return null;
        }

        private static ValidationFailure WageAmountValidation(WageUpdate wageUpdate)
        {
            var wageType = wageUpdate.Type ?? wageUpdate.ExistingType;
            var wageUnit = wageUpdate.Unit ?? wageUpdate.ExistingUnit;

            if (!wageUpdate.HoursPerWeek.HasValue)
            {
                return null;
            }

            if (wageType == WageType.Custom)
            {
                if (wageUpdate.Amount.HasValue)
                {
                    if (wageUpdate.ExistingType == WageType.ApprenticeshipMinimum)
                    {
                        var wageRange = wageUpdate.PossibleStartDate.GetWageRangeFor();

                        var newHourlyRate = Wages.GetHourRate(wageUpdate.Amount.Value, wageUnit, wageUpdate.HoursPerWeek.Value);

                        if (newHourlyRate < wageRange.ApprenticeMinimumWage)
                        {
                            return new ValidationFailure("Amount", string.Format(WageUpdateMessages.Amount.InvalidCustomAmountApprenticeMinimumWage, wageRange.ApprenticeMinimumWage * wageUpdate.HoursPerWeek.Value));
                        }
                    }

                    if (wageUpdate.ExistingType == WageType.NationalMinimum)
                    {
                        var wageRange = wageUpdate.PossibleStartDate.GetWageRangeFor();

                        var newHourlyRate = Wages.GetHourRate(wageUpdate.Amount.Value, wageUnit, wageUpdate.HoursPerWeek.Value);

                        if (newHourlyRate < wageRange.Over21NationalMinimumWage)
                        {
                            return new ValidationFailure("Amount", string.Format(WageUpdateMessages.Amount.InvalidCustomAmountNationalMinimumWage, wageRange.Over21NationalMinimumWage * wageUpdate.HoursPerWeek.Value));
                        }
                    }

                    if (wageUpdate.ExistingType == WageType.Custom && wageUpdate.ExistingAmount.HasValue)
                    {
                        var existingHourlyRate = Wages.GetHourRate(wageUpdate.ExistingAmount.Value, wageUpdate.ExistingUnit, wageUpdate.HoursPerWeek.Value);
                        var newHourlyRate = Wages.GetHourRate(wageUpdate.Amount.Value, wageUnit, wageUpdate.HoursPerWeek.Value);

                        if (newHourlyRate < existingHourlyRate)
                        {
                            return new ValidationFailure("Amount", WageUpdateMessages.Amount.InvalidCustomAmount);
                        }
                    }

                    if (wageUpdate.ExistingType == WageType.CustomRange && wageUpdate.ExistingAmountLowerBound.HasValue)
                    {
                        var existingHourlyRate = Wages.GetHourRate(wageUpdate.ExistingAmountLowerBound.Value, wageUpdate.ExistingUnit, wageUpdate.HoursPerWeek.Value);
                        var newHourlyRate = Wages.GetHourRate(wageUpdate.Amount.Value, wageUnit, wageUpdate.HoursPerWeek.Value);

                        if (newHourlyRate < existingHourlyRate)
                        {
                            return new ValidationFailure("Amount",
                                WageUpdateMessages.Amount.InvalidCustomRangeAmount);
                        }
                    }
                }
            }

            if (wageType == WageType.CustomRange)
            {
                if (wageUpdate.AmountLowerBound.HasValue)
                {
                    if (wageUpdate.ExistingType == WageType.ApprenticeshipMinimum)
                    {
                        var wageRange = wageUpdate.PossibleStartDate.GetWageRangeFor();

                        var newHourlyRate = Wages.GetHourRate(wageUpdate.AmountLowerBound.Value, wageUnit, wageUpdate.HoursPerWeek.Value);

                        if (newHourlyRate < wageRange.ApprenticeMinimumWage)
                        {
                            return new ValidationFailure("AmountLowerBound", string.Format(WageUpdateMessages.AmountLowerBound.InvalidCustomAmountLowerBoundApprenticeMinimumWage, wageRange.ApprenticeMinimumWage * wageUpdate.HoursPerWeek.Value));
                        }
                    }

                    if (wageUpdate.ExistingType == WageType.NationalMinimum)
                    {
                        var wageRange = wageUpdate.PossibleStartDate.GetWageRangeFor();

                        var newHourlyRate = Wages.GetHourRate(wageUpdate.AmountLowerBound.Value, wageUnit, wageUpdate.HoursPerWeek.Value);

                        if (newHourlyRate < wageRange.Over21NationalMinimumWage)
                        {
                            return new ValidationFailure("AmountLowerBound", string.Format(WageUpdateMessages.AmountLowerBound.InvalidCustomAmountLowerBoundNationalMinimumWage, wageRange.Over21NationalMinimumWage * wageUpdate.HoursPerWeek.Value));
                        }
                    }

                    if (wageUpdate.ExistingType == WageType.CustomRange && wageUpdate.ExistingAmountLowerBound.HasValue)
                    {
                        var existingHourlyRate = Wages.GetHourRate(wageUpdate.ExistingAmountLowerBound.Value, wageUpdate.ExistingUnit, wageUpdate.HoursPerWeek.Value);
                        var newHourlyRate = Wages.GetHourRate(wageUpdate.AmountLowerBound.Value, wageUnit, wageUpdate.HoursPerWeek.Value);

                        if (newHourlyRate < existingHourlyRate)
                        {
                            return new ValidationFailure("AmountLowerBound", WageUpdateMessages.AmountLowerBound.InvalidCustomAmountLowerBound);
                        }
                    }

                    if (wageUpdate.ExistingType == WageType.Custom && wageUpdate.ExistingAmount.HasValue)
                    {
                        var existingHourlyRate = Wages.GetHourRate(wageUpdate.ExistingAmount.Value, wageUpdate.ExistingUnit, wageUpdate.HoursPerWeek.Value);
                        var newHourlyRate = Wages.GetHourRate(wageUpdate.AmountLowerBound.Value, wageUnit, wageUpdate.HoursPerWeek.Value);

                        if (newHourlyRate < existingHourlyRate)
                        {
                            return new ValidationFailure("AmountLowerBound", WageUpdateMessages.AmountLowerBound.InvalidCustomRangeAmountLowerBound);
                        }
                    }
                }
            }

            return null;
        }
    }
}