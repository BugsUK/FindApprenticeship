namespace SFA.Apprenticeships.Application.Interfaces.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;

    public interface IVacancyPostingService
    {
        Vacancy CreateApprenticeshipVacancy(Vacancy vacancy);

        Vacancy SaveVacancy(Vacancy vacancy);

        long GetNextVacancyReferenceNumber();

        Vacancy GetVacancy(int vacancyId);

        Vacancy GetVacancy(long vacancyReferenceNumber);

        Vacancy GetVacancy(Guid vacancyGuid);
        
        List<Vacancy> GetWithStatus(params VacancyStatus[] desiredStatuses);

        List<Vacancy> GetByIds(IEnumerable<int> vacancyIds);

        List<Vacancy> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds);

        Vacancy ReserveVacancyForQA(long vacancyReferenceNumber);

        List<VacancyLocation> GetVacancyLocations(int vacancyId);

        List<VacancyLocation> SaveVacancyLocations(List<VacancyLocation> vacancyLocations);
        void DeleteVacancyLocationsFor(int vacancyId);
    }
}
