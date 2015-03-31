namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using GatewayServiceProxy;

    public class LegacyVacancySummaryWageResolver : ValueResolver<VacancySummary, string>
    {
        protected override string ResolveCore(VacancySummary source)
        {
            return !string.IsNullOrWhiteSpace(source.WageType) && source.WageType.ToLower() == "text"
                ? source.WageText
                : string.Format("£{0:N2}", source.WeeklyWage);
        }
    }
}
