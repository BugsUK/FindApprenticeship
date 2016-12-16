namespace SFA.DAS.RAA.Api.Validators
{
    using System;
    using Apprenticeships.Domain.Entities.Vacancies;
    using Apprenticeships.Infrastructure.Presentation.Constants;
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
                return new ValidationFailure(propertyName, "You cannot change the type of a LegacyText wage.");
            }

            if (wageUpdate.ExistingType == WageType.LegacyWeekly)
            {
                if (wageUpdate.Type != WageType.LegacyWeekly && wageUpdate.Type != WageType.Custom && wageUpdate.Type != WageType.CustomRange)
                {
                    return new ValidationFailure(propertyName, "You can only change the type of a LegacyWeekly wage to Custom (fixed) or CustomRange (wage range).");
                }
            }

            if (wageUpdate.ExistingType == WageType.ApprenticeshipMinimum)
            {
                if (wageUpdate.Type != WageType.ApprenticeshipMinimum && wageUpdate.Type != WageType.NationalMinimum && wageUpdate.Type != WageType.Custom && wageUpdate.Type != WageType.CustomRange)
                {
                    return new ValidationFailure(propertyName, "You can only change the type of an ApprenticeshipMinimum wage to NationalMinimum, Custom (fixed) or CustomRange (wage range).");
                }
            }

            if (wageUpdate.ExistingType == WageType.NationalMinimum)
            {
                if (wageUpdate.Type != WageType.NationalMinimum && wageUpdate.Type != WageType.Custom && wageUpdate.Type != WageType.CustomRange)
                {
                    return new ValidationFailure(propertyName, "You can only change the type of a NationalMinimum wage to Custom (fixed) or CustomRange (wage range).");
                }
            }

            if (wageUpdate.ExistingType == WageType.Custom || wageUpdate.ExistingType == WageType.CustomRange)
            {
                if (wageUpdate.Type != WageType.Custom && wageUpdate.Type != WageType.CustomRange)
                {
                    return new ValidationFailure(propertyName,
                        wageUpdate.ExistingType == WageType.Custom
                            ? "You can only change the type of a Custom (fixed) wage to CustomRange (wage range)."
                            : "You can only change the type of a CustomRange (wage range) wage to Custom (fixed).");
                }
            }

            if (wageUpdate.ExistingType == WageType.CompetitiveSalary)
            {
                return new ValidationFailure(propertyName, "You cannot change the type of a CompetitiveSalary wage.");
            }

            if (wageUpdate.ExistingType == WageType.ToBeAgreedUponAppointment)
            {
                return new ValidationFailure(propertyName, "You cannot change the type of a ToBeAgreedUponAppointment wage.");
            }

            if (wageUpdate.ExistingType == WageType.Unwaged)
            {
                if (wageUpdate.Type != WageType.Unwaged && wageUpdate.Type != WageType.ApprenticeshipMinimum && wageUpdate.Type != WageType.NationalMinimum && wageUpdate.Type != WageType.Custom && wageUpdate.Type != WageType.CustomRange)
                {
                    return new ValidationFailure(propertyName, "You can only change the type of an Unwaged wage to ApprenticeshipMinimum, NationalMinimum, Custom (fixed) or CustomRange (wage range).");
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
                        return new ValidationFailure("Amount", "The new fixed wage must be higher than the original figure.");
                    }
                    if (wageUpdate.ExistingType == WageType.CustomRange)
                    {
                        return new ValidationFailure("Amount", "The new fixed wage must be higher than the orignal wage range minimum.");
                    }
                    return new ValidationFailure("Amount", "You must specify a valid amount.");
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
                        return new ValidationFailure("AmountLowerBound", "The minimum amount must be higher than the original amount.");
                    }
                    if (wageUpdate.ExistingType == WageType.Custom)
                    {
                        return new ValidationFailure("AmountLowerBound", "The minimum amount must be higher than the original fixed wage.");
                    }
                    return new ValidationFailure("AmountLowerBound", "You must specify a valid minimum amount for the wage range.");
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
                    return new ValidationFailure("AmountUpperBound", "You must specify a valid maximum amount for the wage range.");
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
                    return new ValidationFailure("Unit", "You must specify a valid wage unit.");
                }
            }
            if (wageType == WageType.CustomRange)
            {
                if (wageUnit == 0 || wageUnit == WageUnit.NotApplicable)
                {
                    return new ValidationFailure("Unit", "You must specify a valid wage unit.");
                }
            }

            return null;
        }

        private static ValidationFailure WageAmountValidation(WageUpdate wageUpdate)
        {
            var wageType = wageUpdate.Type ?? wageUpdate.ExistingType;
            var amountLowerBound = wageUpdate.AmountLowerBound ?? wageUpdate.ExistingAmountLowerBound;
            var amountUpperBound = wageUpdate.AmountUpperBound ?? wageUpdate.ExistingAmountUpperBound;
            var wageUnit = wageUpdate.Unit ?? wageUpdate.ExistingUnit;

            if (wageType == WageType.Custom && wageUpdate.HoursPerWeek.HasValue)
            {
                if (wageUpdate.Amount.HasValue)
                {
                    if (wageUpdate.ExistingType == WageType.Custom && wageUpdate.ExistingAmount.HasValue)
                    {
                        var existingHourlyRate = Wages.GetHourRate(wageUpdate.ExistingAmount.Value, wageUpdate.ExistingUnit, wageUpdate.HoursPerWeek.Value);
                        var newHourlyRate = Wages.GetHourRate(wageUpdate.Amount.Value, wageUnit, wageUpdate.HoursPerWeek.Value);

                        if (newHourlyRate < existingHourlyRate)
                        {
                            return new ValidationFailure("Amount", "The new fixed wage must be higher than the original figure.");
                        }
                    }
                }

                //DateTime possibleStartDate;
                //var wageRange = viewModel.VacancyDatesViewModel.GetWageRangeForPossibleStartDate(out possibleStartDate); */
            }

            if (wageType == WageType.CustomRange)
            {
                if (amountLowerBound.HasValue && amountUpperBound.HasValue && amountUpperBound <= amountLowerBound)
                {
                    return new ValidationFailure("AmountUpperBound", "The maximum amount for the wage range must be higher than the minimum amount.");
                }
            }

            return null;
        }
    }
}