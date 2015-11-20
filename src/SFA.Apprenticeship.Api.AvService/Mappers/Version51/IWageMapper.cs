namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;

    public interface IWageMapper
    {
        string MapToText(WageType wageType, WageUnit wageUnit, decimal wage);
    }
}
