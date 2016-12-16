namespace SFA.DAS.RAA.Api.Validators
{
    using Apprenticeships.Domain.Entities.Vacancies;
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
                RuleFor(w => w.Amount)
                    .GreaterThan(wu => wu.ExistingAmount)
                    .WithMessage("Amount must be greater than the existing amount.")
                    .When(wu => wu.ExistingAmount.HasValue);

                Custom(WageTypeValidation);
            });

            
            //RuleFor(v => v.ShortDescription).NotEmpty().WithMessage("Please enter a short description for the vacancy");
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
    }
}