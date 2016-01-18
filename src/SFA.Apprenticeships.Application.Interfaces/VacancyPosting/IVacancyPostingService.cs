using System;

namespace SFA.Apprenticeships.Application.Interfaces.VacancyPosting
{
    using System.Collections.Generic;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;

    public interface IVacancyPostingService
    {
        ApprenticeshipVacancy CreateApprenticeshipVacancy(ApprenticeshipVacancy vacancy);

        ApprenticeshipVacancy SaveApprenticeshipVacancy(ApprenticeshipVacancy vacancy);

        ApprenticeshipVacancy ShallowSaveApprenticeshipVacancy(ApprenticeshipVacancy vacancy);

        long GetNextVacancyReferenceNumber();

        ApprenticeshipVacancy GetVacancy(long vacancyReferenceNumber);

        ApprenticeshipVacancy GetVacancy(Guid vacancyGuid);

        List<ApprenticeshipVacancy> GetWithStatus(params ProviderVacancyStatuses[] desiredStatuses);

        List<ApprenticeshipVacancy> GetForProvider(string ukPrn, string providerSiteErn);

        ApprenticeshipVacancy ReserveVacancyForQA(long vacancyReferenceNumber);
    }
}
