using System;
using System.Collections.Generic;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;

namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Queries;

    public interface IApprenticeshipVacancyReadRepository : IReadRepository<ApprenticeshipVacancy>
    {
        ApprenticeshipVacancy Get(long vacancyReferenceNumber);

        List<ApprenticeshipVacancy> GetForProvider(string ukPrn);

        List<ApprenticeshipVacancy> GetForProvider(string ukPrn, string providerSiteErn);

        List<ApprenticeshipVacancy> GetForProvider(string ukPrn, params ProviderVacancyStatuses[] desiredStatuses);

        List<ApprenticeshipVacancy> GetWithStatus(params ProviderVacancyStatuses[] desiredStatuses);

        List<ApprenticeshipVacancy> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount);
    }

    public interface IApprenticeshipVacancyWriteRepository : IWriteRepository<ApprenticeshipVacancy>
    {
        ApprenticeshipVacancy ReserveVacancyForQA(long vacancyReferenceNumber);
    }
}
