namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Interfaces.Queries;
    using DataContracts.Version51;

    public interface IApprenticeshipVacancyQueryMapper
    {
        ApprenticeshipVacancyQuery MapToApprenticeshipVacancyQuery(VacancySearchData criteria);
    }
}
