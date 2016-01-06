namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using DataContracts.Version51;

    public interface IVacancyUploadRequestMapper
    {
        ApprenticeshipVacancy ToNewApprenticeshipVacancy(VacancyUploadData vacancyUploadData);
    }
}
