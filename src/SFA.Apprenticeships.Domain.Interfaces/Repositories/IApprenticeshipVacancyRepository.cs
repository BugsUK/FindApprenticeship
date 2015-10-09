namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Vacancies.ProviderVacancies.Apprenticeship;

    public interface IApprenticeshipVacancyReadRepository : IReadRepository<ApprenticeshipVacancy>
    {
        ApprenticeshipVacancy Get(long vacancyReferenceNumber);
    }

    public interface IApprenticeshipVacancyWriteRepository : IWriteRepository<ApprenticeshipVacancy>
    {
    }
}
