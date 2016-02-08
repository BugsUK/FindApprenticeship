namespace SFA.Apprenticeships.Application.VacancyPosting
{
    //TODO: rename project to SFA.Management.Application.VacancyPosting?
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using CuttingEdge.Conditions;
    using Domain.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Repositories;
    using Interfaces.VacancyPosting;

    public class VacancyPostingService : IVacancyPostingService
    {
        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
        private readonly IApprenticeshipVacancyWriteRepository _apprenticeshipVacancyWriteRepository;
        private readonly IReferenceNumberRepository _referenceNumberRepository;
        private readonly IProviderUserReadRepository _providerUserReadRepository;

        public VacancyPostingService(
            IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository,
            IApprenticeshipVacancyWriteRepository apprenticeshipVacancyWriteRepository,
            IReferenceNumberRepository referenceNumberRepository,
            IProviderUserReadRepository providerUserReadRepository)
        {
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
            _apprenticeshipVacancyWriteRepository = apprenticeshipVacancyWriteRepository;
            _referenceNumberRepository = referenceNumberRepository;
            _providerUserReadRepository = providerUserReadRepository;
        }

        public ApprenticeshipVacancy CreateApprenticeshipVacancy(ApprenticeshipVacancy vacancy)
        {
            Condition.Requires(vacancy);

            if (Thread.CurrentPrincipal.IsInRole(Roles.Faa))
            {
                var username = Thread.CurrentPrincipal.Identity.Name;
                var vacancyManager = _providerUserReadRepository.Get(username);
                if (vacancyManager != null)
                {
                    vacancy.VacancyManagerId = vacancyManager.EntityId;
                }
            }

            return SaveApprenticeshipVacancy(vacancy);
        }

        public ApprenticeshipVacancy SaveApprenticeshipVacancy(ApprenticeshipVacancy vacancy)
        {
            Condition.Requires(vacancy);

            // currentApplication.AssertState("Save apprenticeship application", ApplicationStatuses.Draft);
            if (vacancy.Status == ProviderVacancyStatuses.Completed)
            {
                var message = $"Vacancy {vacancy.VacancyReferenceNumber} can not be in Completed status on saving.";
                throw new CustomException(message, ErrorCodes.EntityStateError);
            }

            if (Thread.CurrentPrincipal.IsInRole(Roles.Faa))
            {
                var username = Thread.CurrentPrincipal.Identity.Name;
                var lastEditedBy = _providerUserReadRepository.Get(username);
                if (lastEditedBy != null)
                {
                    vacancy.LastEditedById = lastEditedBy.EntityId;
                }
            }

            _apprenticeshipVacancyWriteRepository.DeepSave(vacancy);

            return _apprenticeshipVacancyReadRepository.Get(vacancy.EntityId);
        }

        public ApprenticeshipVacancy ShallowSaveApprenticeshipVacancy(ApprenticeshipVacancy vacancy)
        {
            Condition.Requires(vacancy);

            // currentApplication.AssertState("Save apprenticeship application", ApplicationStatuses.Draft);
            if (vacancy.Status == ProviderVacancyStatuses.Completed)
            {
                var message = $"Vacancy {vacancy.VacancyReferenceNumber} can not be in Completed status on saving.";
                throw new CustomException(message, ErrorCodes.EntityStateError);
            }

            if (Thread.CurrentPrincipal.IsInRole(Roles.Faa))
            {
                var username = Thread.CurrentPrincipal.Identity.Name;
                var lastEditedBy = _providerUserReadRepository.Get(username);
                if (lastEditedBy != null)
                {
                    vacancy.LastEditedById = lastEditedBy.EntityId;
                }
            }

            _apprenticeshipVacancyWriteRepository.ShallowSave(vacancy);

            return _apprenticeshipVacancyReadRepository.Get(vacancy.EntityId);
        }

        public int GetNextVacancyReferenceNumber()
        {
            return _referenceNumberRepository.GetNextVacancyReferenceNumber();
        }

        public ApprenticeshipVacancy GetVacancy(int vacancyReferenceNumber)
        {
            return _apprenticeshipVacancyReadRepository.Get(vacancyReferenceNumber);
        }

        public ApprenticeshipVacancy GetVacancy(Guid vacancyGuid)
        {
            return _apprenticeshipVacancyReadRepository.Get(vacancyGuid);
        }

        public List<ApprenticeshipVacancy> GetWithStatus(params ProviderVacancyStatuses[] desiredStatuses)
        {
            return _apprenticeshipVacancyReadRepository.GetWithStatus(desiredStatuses);
        }

        public List<ApprenticeshipVacancy> GetForProvider(string ukPrn, string providerSiteErn)
        {
            return _apprenticeshipVacancyReadRepository.GetForProvider(ukPrn, providerSiteErn);
        }

        public ApprenticeshipVacancy ReserveVacancyForQA(int vacancyReferenceNumber)
        {
            return _apprenticeshipVacancyWriteRepository.ReserveVacancyForQA(vacancyReferenceNumber);
        }

        public void ReplaceLocationInformation(Guid vacancyGuid, bool? isEmployerLocationMainApprenticeshipLocation, int? numberOfPositions,
            IEnumerable<VacancyLocationAddress> vacancyLocationAddresses, string locationAddressesComment, string additionalLocationInformation,
            string additionalLocationInformationComment)
        {
            _apprenticeshipVacancyWriteRepository.ReplaceLocationInformation(vacancyGuid,
                isEmployerLocationMainApprenticeshipLocation, numberOfPositions, vacancyLocationAddresses,
                locationAddressesComment, additionalLocationInformation, additionalLocationInformationComment);
        }
    }
}