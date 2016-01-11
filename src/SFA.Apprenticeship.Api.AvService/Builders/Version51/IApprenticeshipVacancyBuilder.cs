namespace SFA.Apprenticeship.Api.AvService.Builders.Version51
{
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using DataContracts.Version51;

    public interface IApprenticeshipVacancyBuilder
    {
        ApprenticeshipVacancy ToApprenticeshipVacancy(VacancyUploadData vacancyUploadData);
    }
}
