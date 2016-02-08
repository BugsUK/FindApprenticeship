using System;

namespace SFA.Apprenticeships.Application.Interfaces.VacancyPosting
{
    using System.Collections.Generic;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;

    public interface IVacancyPostingService
    {
        ApprenticeshipVacancy CreateApprenticeshipVacancy(ApprenticeshipVacancy vacancy);

        ApprenticeshipVacancy SaveApprenticeshipVacancy(ApprenticeshipVacancy vacancy);

        ApprenticeshipVacancy ShallowSaveApprenticeshipVacancy(ApprenticeshipVacancy vacancy);

        int GetNextVacancyReferenceNumber();

        ApprenticeshipVacancy GetVacancy(int vacancyReferenceNumber);

        ApprenticeshipVacancy GetVacancy(Guid vacancyGuid);

        List<ApprenticeshipVacancy> GetWithStatus(params ProviderVacancyStatuses[] desiredStatuses);

        List<ApprenticeshipVacancy> GetForProvider(string ukPrn, string providerSiteErn);

        ApprenticeshipVacancy ReserveVacancyForQA(int vacancyReferenceNumber);

        void ReplaceLocationInformation(Guid vacancyGuid, bool? isEmployerLocationMainApprenticeshipLocation, int? numberOfPositions,
            IEnumerable<VacancyLocationAddress> vacancyLocationAddresses, string locationAddressesComment,
            string additionalLocationInformation, string additionalLocationInformationComment);
    }
}
