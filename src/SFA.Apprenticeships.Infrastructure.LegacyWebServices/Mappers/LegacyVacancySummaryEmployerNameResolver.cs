namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using GatewayServiceProxy;

    public class LegacyVacancySummaryEmployerNameResolver : ValueResolver<VacancySummary, string>
    {
        protected override string ResolveCore(VacancySummary source)
        {
            return source.EmployerAnonymous ? source.EmployerTradingName : source.EmployerName;
        }
    }
}
