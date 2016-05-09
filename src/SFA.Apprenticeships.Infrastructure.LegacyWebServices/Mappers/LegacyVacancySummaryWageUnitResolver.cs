namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class LegacyVacancySummaryWageUnitResolver : ValueResolver<string, WageUnit>
    {
        protected override WageUnit ResolveCore(string wageType)
        {
            var wageTypeText = string.IsNullOrWhiteSpace(wageType) ? "" : wageType.ToLower();
            if (wageTypeText == "weekly")
            {
                return WageUnit.Weekly;
            }
            return WageUnit.NotApplicable;
        }
    }
}