using System.Collections.Generic;

namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Vacancies.ProviderVacancies.Apprenticeship;

    public interface IApprenticeshipVacancyReadRepository : IReadRepository<ApprenticeshipVacancy>
    {
        ApprenticeshipVacancy Get(long vacancyReferenceNumber);

        List<ApprenticeshipVacancy> GetForProvider(string ukPrn);
    }

    public interface IApprenticeshipVacancyWriteRepository : IWriteRepository<ApprenticeshipVacancy>
    {
    }
}
