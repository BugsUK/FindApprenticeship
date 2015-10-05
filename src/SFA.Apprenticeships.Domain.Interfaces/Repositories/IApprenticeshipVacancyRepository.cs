namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Vacancies.ProviderVacancies.Apprenticeship;

    public interface IApprenticeshipVacancyReadRepository : IReadRepository<ApprenticeshipVacancy>
    {
    }

    public interface IApprenticeshipVacancyWriteRepository : IWriteRepository<ApprenticeshipVacancy>
    {
    }
}
