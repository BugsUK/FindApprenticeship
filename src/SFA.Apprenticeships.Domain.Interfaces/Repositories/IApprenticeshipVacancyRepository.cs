﻿namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Locations;
    using Entities.Vacancies.ProviderVacancies;
    using Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Queries;

    public interface IApprenticeshipVacancyReadRepository : IReadRepository<ApprenticeshipVacancy>
    {
        ApprenticeshipVacancy Get(long vacancyReferenceNumber);

        List<ApprenticeshipVacancy> GetForProvider(string ukPrn, string providerSiteErn);

        List<ApprenticeshipVacancy> GetWithStatus(params ProviderVacancyStatuses[] desiredStatuses);

        List<ApprenticeshipVacancy> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount);
    }

    public interface IApprenticeshipVacancyWriteRepository
    {
        ApprenticeshipVacancy ReserveVacancyForQA(long vacancyReferenceNumber);

        ApprenticeshipVacancy ShallowSave(ApprenticeshipVacancy entity);

        ApprenticeshipVacancy DeepSave(ApprenticeshipVacancy entity);

        ApprenticeshipVacancy ReplaceLocationInformation(Guid vacancyGuid, bool? isEmployerLocationMainApprenticeshipLocation,
            int? numberOfPositions, IEnumerable<VacancyLocationAddress> vacancyLocationAddresses,
            string locationAddressesComment, string additionalLocationInformation,
            string additionalLocationInformationComment);
    }
}