namespace SFA.Apprenticeships.Application.Interfaces.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;

    public interface IVacancyPostingService
    {
        Vacancy CreateApprenticeshipVacancy(Vacancy vacancy);

        Vacancy SaveApprenticeshipVacancy(Vacancy vacancy);

        Vacancy ShallowSaveApprenticeshipVacancy(Vacancy vacancy);

        long GetNextVacancyReferenceNumber();

        Vacancy GetVacancy(int vacancyId);

        Vacancy GetVacancy(long vacancyReferenceNumber);

        Vacancy GetVacancy(Guid vacancyGuid);
        
        List<Vacancy> GetWithStatus(params VacancyStatus[] desiredStatuses);

        List<Vacancy> GetByIds(IEnumerable<int> vacancyIds);

        List<Vacancy> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds);

        Vacancy ReserveVacancyForQA(long vacancyReferenceNumber);

        void ReplaceLocationInformation(long vacancyReferenceNumber, bool? isEmployerLocationMainApprenticeshipLocation, int? numberOfPositions,
            IEnumerable<VacancyLocation> vacancyLocationAddresses, string locationAddressesComment,
            string additionalLocationInformation, string additionalLocationInformationComment);

        List<VacancyLocation> GetLocationAddresses(int vacancyId);

        void IncrementOfflineApplicationClickThrough(long vacancyReferenceNumber);
    }
}
