namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Raa.Vacancies;
    using Queries;

    public interface IVacancyReadRepository
    {
        Vacancy Get(int vacancyId);

        Vacancy GetByReferenceNumber(long vacancyReferenceNumber);

        Vacancy GetByVacancyGuid(Guid vacancyGuid);

        List<Vacancy> GetByIds(IEnumerable<int> vacancyIds);

        List<Vacancy> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds);

        List<Vacancy> GetWithStatus(params VacancyStatus[] desiredStatuses);

        List<Vacancy> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount);
    }

    public interface IVacancyWriteRepository
    {
        Vacancy Save(Vacancy vacancy);

        void Delete(int vacancyId);

        Vacancy ReserveVacancyForQA(long vacancyReferenceNumber);

        void IncrementOfflineApplicationClickThrough(long vacancyReferenceNumber);
    }
}
