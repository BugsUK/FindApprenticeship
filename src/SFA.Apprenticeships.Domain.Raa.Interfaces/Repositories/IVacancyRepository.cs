namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Raa.Vacancies;
    using Queries;

    public interface IVacancyReadRepository
    {
        Vacancy Get(int vacancyId);

        Vacancy GetByReferenceNumber(int vacancyReferenceNumber);

        Vacancy GetByVacancyGuid(Guid vacancyGuid);
        
        List<Vacancy> GetByIds(IEnumerable<int> vacancyIds);

        List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds);

        List<Vacancy> GetWithStatus(params VacancyStatus[] desiredStatuses);

        List<Vacancy> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount);
    }

    public interface IVacancyWriteRepository
    {
        Vacancy Create(Vacancy vacancy);

        void Delete(int vacancyId);

        Vacancy ReserveVacancyForQA(int vacancyReferenceNumber);

        void IncrementOfflineApplicationClickThrough(int vacancyReferenceNumber);

        Vacancy Update(Vacancy vacancy);
    }
}
