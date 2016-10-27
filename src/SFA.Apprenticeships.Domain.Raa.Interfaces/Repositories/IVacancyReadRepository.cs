namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.Vacancies;
    using Queries;
    using System;
    using System.Collections.Generic;

    public interface IVacancyReadRepository
    {
        Vacancy Get(int vacancyId);

        Vacancy GetByReferenceNumber(int vacancyReferenceNumber);

        Vacancy GetByVacancyGuid(Guid vacancyGuid);

        int CountWithStatus(params VacancyStatus[] desiredStatuses);

        int GetVacancyIdByReferenceNumber(int vacancyReferenceNumber);
    }
}
