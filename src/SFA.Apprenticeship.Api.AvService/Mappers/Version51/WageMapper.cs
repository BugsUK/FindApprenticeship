namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;

    public static class WageMapper
    {
        public static string MapToText(WageType wageType, WageUnit wageUnit, decimal wage)
        {
            // TODO: incomplete impl.
            return $"£{wage:N2}";
        }
    }
}
