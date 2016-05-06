namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using VacancySummary = GatewayServiceProxy.VacancySummary;

    public class LegacyVacancySummaryWageUnitResolver : ValueResolver<VacancySummary, WageUnit>
    {
        protected override WageUnit ResolveCore(VacancySummary source)
        {
            var wageTypeText = string.IsNullOrWhiteSpace(source.WageType) ? "" : source.WageType.ToLower();
            if (wageTypeText == "weekly")
            {
                return WageUnit.Weekly;
            }
            return WageUnit.NotApplicable;
        }
    }
}