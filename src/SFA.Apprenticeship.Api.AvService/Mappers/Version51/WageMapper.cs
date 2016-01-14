namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Vacancies;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;

    public class WageMapper : IWageMapper
    {
        public string MapToText(WageType wageType, WageUnit wageUnit, decimal wage)
        {
            // TODO: incomplete impl.
            return $"£{wage:N2}";
        }
    }
}
