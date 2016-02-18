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

        List<Vacancy> GetForProvider(int providerId, int providerSiteId);

        Vacancy ReserveVacancyForQA(long vacancyReferenceNumber);

        void ReplaceLocationInformation(long vacancyReferenceNumber, bool? isEmployerLocationMainApprenticeshipLocation, int? numberOfPositions,
            IEnumerable<VacancyLocationAddress> vacancyLocationAddresses, string locationAddressesComment,
            string additionalLocationInformation, string additionalLocationInformationComment);

        List<VacancyLocationAddress> GetLocationAddresses(int vacancyId);
    }
}
