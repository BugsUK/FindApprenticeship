namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using GatewayServiceProxy;

    public class LegacyVacancySummaryEmployerNameResolver : ValueResolver<VacancySummary, string>
    {
        protected override string ResolveCore(VacancySummary source)
        {
            return source.EmployerAnonymous ? string.Empty : source.EmployerName;
        }
    }
}
