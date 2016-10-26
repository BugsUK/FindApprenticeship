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
            result.Type = wage.Type;

            switch (wage.Type)
            {
                case WageType.Custom:
                    result.Classification = WageClassification.Custom;
                    result.CustomType = CustomWageType.Fixed;
                    result.PresetText = PresetText.NotApplicable;
                    break;
                case WageType.CustomRange:
                    result.Classification = WageClassification.Custom;
                    result.CustomType = CustomWageType.Ranged;
                    result.PresetText = PresetText.NotApplicable;
                    break;
                case WageType.CompetitiveSalary:
                    result.Classification = WageClassification.PresetText;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.CompetitiveSalary;
                    break;
                case WageType.ToBeAgreedUponAppointment:
                    result.Classification = WageClassification.PresetText;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.ToBeAgreedUponAppointment;
                    break;
                case WageType.Unwaged:
                    result.Classification = WageClassification.PresetText;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.Unwaged;
                    break;
                case WageType.LegacyText:
                    result.Classification = WageClassification.LegacyText;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.NotApplicable;
                    break;
                case WageType.LegacyWeekly:
                    result.Classification = WageClassification.Custom;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.NotApplicable;
                    break;
                case WageType.ApprenticeshipMinimum:
                    result.Classification = WageClassification.ApprenticeshipMinimum;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.NotApplicable;
                    break;
                case WageType.NationalMinimum:
                    result.Classification = WageClassification.NationalMinimum;
                    result.CustomType = CustomWageType.NotApplicable;
                    result.PresetText = PresetText.NotApplicable;
                    break;
                default:
                    throw new InvalidOperationException("Unknown WageType.");
            }

            return result;
        }
    }
}
