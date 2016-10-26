namespace SFA.Apprenticeships.Web.Common.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using ViewModels;

    public sealed class WageToWageViewModelConverter : ITypeConverter<Wage, WageViewModel>
    {
        public WageViewModel Convert(ResolutionContext context)
        {
            var wage = (Wage) context.SourceValue;
            var result = new WageViewModel();

            result.HoursPerWeek = wage.HoursPerWeek;
            result.Amount = wage.Amount;
            result.AmountLowerBound = wage.AmountLowerBound;
            result.AmountUpperBound = wage.AmountUpperBound;
            result.Text = wage.Text;
            result.Unit = wage.Unit;
            result.RangeUnit = wage.Unit;
            result.WageTypeReason = wage.ReasonForType;

            switch (wage.Type)
            {
                case WageType.Custom:
                    result.Type = WageType.Custom;
                    result.CustomType = CustomWageType.Fixed;
                    result.PresetText = PresetText.NotApplicable;
                    break;
                case WageType.CustomRange:
                    result.Type = WageType.Custom;
                    result.CustomType = CustomWageType.Ranged;
                    result.PresetText = PresetText.NotApplicable;
                    break;
                case WageType.CompetitiveSalary:
                    result.Type = WageType.LegacyText;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.CompetitiveSalary;
                    break;
                case WageType.ToBeAgreedUponAppointment:
                    result.Type = WageType.LegacyText;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.ToBeAgreedUponAppointment;
                    break;
                case WageType.Unwaged:
                    result.Type = WageType.LegacyText;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.Unwaged;
                    break;
                case WageType.LegacyText:
                case WageType.LegacyWeekly:
                case WageType.ApprenticeshipMinimum:
                case WageType.NationalMinimum:
                    result.Type = wage.Type;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.NotApplicable;
                    break;
                default:
                    throw new InvalidOperationException("unknown wage type");
            }

            return result;
        }
    }
}
