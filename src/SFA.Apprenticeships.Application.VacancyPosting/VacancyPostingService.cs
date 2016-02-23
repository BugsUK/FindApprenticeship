namespace SFA.Apprenticeships.Application.VacancyPosting
{
    //TODO: rename project to SFA.Management.Application.VacancyPosting?
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using CuttingEdge.Conditions;
    using Domain.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.VacancyPosting;

    public class VacancyPostingService : IVacancyPostingService
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IReferenceNumberRepository _referenceNumberRepository;
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IVacancyLocationReadRepository _vacancyLocationReadRepository;

        public VacancyPostingService(
            IVacancyReadRepository vacancyReadRepository,
            IVacancyWriteRepository vacancyWriteRepository,
            IReferenceNumberRepository referenceNumberRepository,
            IProviderUserReadRepository providerUserReadRepository, 
            IVacancyLocationReadRepository vacancyLocationReadRepository)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _referenceNumberRepository = referenceNumberRepository;
            _providerUserReadRepository = providerUserReadRepository;
            _vacancyLocationReadRepository = vacancyLocationReadRepository;
        }

        public Vacancy CreateApprenticeshipVacancy(Vacancy vacancy)
        {
            Condition.Requires(vacancy);

            if (Thread.CurrentPrincipal.IsInRole(Roles.Faa))
            {
                var username = Thread.CurrentPrincipal.Identity.Name;
                var vacancyManager = _providerUserReadRepository.Get(username);
                if (vacancyManager != null)
                {
                    vacancy.VacancyManagerId = vacancyManager.ProviderUserId;
                }
            }

            return SaveApprenticeshipVacancy(vacancy);
        }

        public Vacancy SaveApprenticeshipVacancy(Vacancy vacancy)
        {
            Condition.Requires(vacancy);

            // currentApplication.AssertState("Save apprenticeship application", ApplicationStatuses.Draft);
            if (vacancy.Status == VacancyStatus.Completed)
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
                    vacancy.LastEditedById = lastEditedBy.ProviderUserId;
                }
            }

            _vacancyWriteRepository.Save(vacancy);

            return _vacancyReadRepository.Get(vacancy.VacancyId);
        }

        public Vacancy ShallowSaveApprenticeshipVacancy(Vacancy vacancy)
        {
            Condition.Requires(vacancy);

            // currentApplication.AssertState("Save apprenticeship application", ApplicationStatuses.Draft);
            if (vacancy.Status == VacancyStatus.Completed)
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
                    vacancy.LastEditedById = lastEditedBy.ProviderUserId;
                }
            }

            _vacancyWriteRepository.ShallowSave(vacancy);

            return _vacancyReadRepository.Get(vacancy.VacancyId);
        }

        public long GetNextVacancyReferenceNumber()
        {
            return _referenceNumberRepository.GetNextVacancyReferenceNumber();
        }

        public Vacancy GetVacancy(long vacancyReferenceNumber)
        {
            return _vacancyReadRepository.GetByReferenceNumber(vacancyReferenceNumber);
        }

        public Vacancy GetVacancy(Guid vacancyGuid)
        {
            return _vacancyReadRepository.GetByVacancyGuid(vacancyGuid);
        }

        public Vacancy GetVacancy(int vacancyId)
        {
            return _vacancyReadRepository.Get(vacancyId);
        }

        public List<Vacancy> GetWithStatus(params VacancyStatus[] desiredStatuses)
        {
            return _vacancyReadRepository.GetWithStatus(desiredStatuses);
        }

        public List<Vacancy> GetByIds(IEnumerable<int> vacancyIds)
        {
            return _vacancyReadRepository.GetByIds(vacancyIds);
        }

        public List<Vacancy> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds)
        {
            return _vacancyReadRepository.GetByOwnerPartyIds(ownerPartyIds);
        }

        public Vacancy ReserveVacancyForQA(long vacancyReferenceNumber)
        {
            return _vacancyWriteRepository.ReserveVacancyForQA(vacancyReferenceNumber);
        }

        public void ReplaceLocationInformation(long vacancyReferenceNumber, bool? isEmployerLocationMainApprenticeshipLocation, int? numberOfPositions,
            IEnumerable<VacancyLocation> vacancyLocationAddresses, string locationAddressesComment, string additionalLocationInformation,
            string additionalLocationInformationComment)
        {
            _vacancyWriteRepository.ReplaceLocationInformation(vacancyReferenceNumber,
                isEmployerLocationMainApprenticeshipLocation, numberOfPositions, vacancyLocationAddresses,
                locationAddressesComment, additionalLocationInformation, additionalLocationInformationComment);
        }

        public List<VacancyLocation> GetLocationAddresses(int vacancyId)
        {
            return _vacancyLocationReadRepository.GetForVacancyId(vacancyId);
        }

        public void IncrementOfflineApplicationClickThrough(long vacancyReferenceNumber)
        {
            _vacancyWriteRepository.IncrementOfflineApplicationClickThrough(vacancyReferenceNumber);
        }
    }
}