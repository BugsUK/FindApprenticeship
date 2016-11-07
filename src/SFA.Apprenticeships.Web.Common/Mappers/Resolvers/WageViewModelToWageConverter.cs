namespace SFA.Apprenticeships.Web.Common.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using ViewModels;

    public sealed class WageViewModelToWageConverter : ITypeConverter<WageViewModel, Wage>
    {
        public Wage Convert(ResolutionContext context)
        {
            var source = (WageViewModel)context.SourceValue;
            var wageType = GetWageType(source);
            var wageUnit = GetWageUnit(source);

            return new Wage(wageType, source.Amount, source.AmountLowerBound, source.AmountUpperBound, source.Text,
                wageUnit, source.HoursPerWeek, source.WageTypeReason);
        }

        private static WageType GetWageType(WageViewModel source)
        {
            switch (source.Classification)
            {
                case WageClassification.ApprenticeshipMinimum:
                    return WageType.ApprenticeshipMinimum;
                case WageClassification.Custom:
                    switch (source.CustomType)
                    {
                        case CustomWageType.Fixed:
                            return WageType.Custom;
                        case CustomWageType.Ranged:
                            return WageType.CustomRange;
                        case CustomWageType.NotApplicable:
                            return WageType.Custom;
                        default:
                            throw new InvalidOperationException();
                    }
                case WageClassification.NationalMinimum:
                    return WageType.NationalMinimum;
                case WageClassification.PresetText:
                    switch (source.PresetText)
                    {
                        case PresetText.CompetitiveSalary:
                            return WageType.CompetitiveSalary;
                        case PresetText.ToBeAgreedUponAppointment:
                            return WageType.ToBeAgreedUponAppointment;
                        case PresetText.Unwaged:
                            return WageType.Unwaged;
                        default:
                            throw new InvalidOperationException();
                    }
                case WageClassification.LegacyText:
                    return WageType.LegacyText;
                case WageClassification.NotApplicable:
                    return WageType.Custom;
                default:
                    throw new InvalidOperationException("Cannot determine WageType from this WageClassification.");
            }
        }

        private static WageUnit GetWageUnit(WageViewModel source)
        {
            if (source.Classification == WageClassification.Custom && source.CustomType == CustomWageType.Ranged)
                return source.RangeUnit;

            return source.Unit;
        }
    }
}
