namespace SFA.Apprenticeships.Web.Common.Mappers.Resolvers
{
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
            
            return new Wage(wageType, source.Amount, source.AmountLowerBound, source.AmountUpperBound, source.Text, wageUnit, source.HoursPerWeek, source.WageTypeReason);
        }

        private static WageType GetWageType(WageViewModel source)
        {
            return source.Type;
            //switch (source.Type)
            //{
            //case WageClassification.ApprenticeshipMinimum:
            //    return WageType.ApprenticeshipMinimum;
            //case WageClassification.NationalMinimum:
            //    return WageType.NationalMinimum;
            //case WageClassification.LegacyText:
            //    return WageType.LegacyText;
            //case WageClassification.PresetText:
            //    switch (source.PresetText)
            //    {
            //        case PresetText.CompetitiveSalary:
            //            return WageType.CompetitiveSalary;
            //        case PresetText.ToBeAgreedUponAppointment:
            //            return WageType.ToBeAgreedUponAppointment;
            //        case PresetText.Unwaged:
            //            return WageType.Unwaged;
            //        default:
            //            throw new InvalidEnumArgumentException();
            //    }
            //case WageClassification.Custom:
            //    switch (source.CustomType)
            //    {
            //        case CustomWageType.Fixed:
            //            return WageType.Custom;
            //        case CustomWageType.Ranged:
            //            return WageType.CustomRange;
            //        case CustomWageType.NotApplicable:
            //        default:
            //            throw new InvalidEnumArgumentException();
            //    }
            //default:
            //    throw new InvalidEnumArgumentException();
            //}
        }

        private static WageUnit GetWageUnit(WageViewModel source)
        {
            return source.Unit;
            //TODO: map unit
        }
    }
}
