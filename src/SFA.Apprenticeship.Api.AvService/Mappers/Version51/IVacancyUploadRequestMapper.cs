namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using MessageContracts.Version51;

    public interface IVacancyUploadRequestMapper
    {
        ApprenticeshipVacancy ToApprenticeshipVacancy(VacancyUploadRequest request);
    }
}
