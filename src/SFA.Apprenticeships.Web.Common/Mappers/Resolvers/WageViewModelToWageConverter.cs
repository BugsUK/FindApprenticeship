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
            return GetWageType(source.Classification, source.CustomType, source.PresetText);
        }

        public static WageType GetWageType(WageClassification classification, CustomWageType customType, PresetText presetText)
        {
            switch (classification)
            {
                case WageClassification.ApprenticeshipMinimum:
                    return WageType.ApprenticeshipMinimum;
                case WageClassification.Custom:
                    switch (customType)
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
                    switch (presetText)
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
            return GetWageUnit(source.Classification, source.CustomType, source.Unit, source.RangeUnit);
        }

        public static WageUnit GetWageUnit(WageClassification classification, CustomWageType customType, WageUnit unit, WageUnit rangeUnit)
        {
            if (classification == WageClassification.Custom && customType == CustomWageType.Ranged)
                return rangeUnit;

            return unit;
        }
    }
}
