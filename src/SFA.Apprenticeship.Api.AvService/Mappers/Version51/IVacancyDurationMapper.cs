namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;

    public interface IVacancyDurationMapper
    {
        string MapDurationToString(int? duration, DurationType durationType);
    }
}
