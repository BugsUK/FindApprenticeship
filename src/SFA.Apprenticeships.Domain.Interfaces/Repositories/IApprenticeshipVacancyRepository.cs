using System;
using System.Collections.Generic;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;

namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Vacancies.ProviderVacancies.Apprenticeship;

    public interface IApprenticeshipVacancyReadRepository : IReadRepository<ApprenticeshipVacancy>
    {
        ApprenticeshipVacancy Get(long vacancyReferenceNumber);

        List<ApprenticeshipVacancy> GetForProvider(string ukPrn);

        List<ApprenticeshipVacancy> GetForProvider(string ukPrn, List<ProviderVacancyStatuses> desiredStatuses);

        List<ApprenticeshipVacancy> GetWithStatus(List<ProviderVacancyStatuses> desiredStatuses);
    }

    public interface IApprenticeshipVacancyWriteRepository : IWriteRepository<ApprenticeshipVacancy>
    {
    }
}
