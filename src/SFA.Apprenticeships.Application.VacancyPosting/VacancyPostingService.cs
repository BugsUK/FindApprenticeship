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
    using Domain.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.VacancyPosting;

    public class VacancyPostingService : IVacancyPostingService
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IReferenceNumberRepository _referenceNumberRepository;
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IVacancyLocationReadRepository _vacancyLocationReadRepository;
        private readonly IVacancyLocationWriteRepository _vacancyLocationWriteRepository;

        public VacancyPostingService(
            IVacancyReadRepository vacancyReadRepository,
            IVacancyWriteRepository vacancyWriteRepository,
            IReferenceNumberRepository referenceNumberRepository,
            IProviderUserReadRepository providerUserReadRepository, 
            IVacancyLocationReadRepository vacancyLocationReadRepository, IVacancyLocationWriteRepository vacancyLocationWriteRepository)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _referenceNumberRepository = referenceNumberRepository;
            _providerUserReadRepository = providerUserReadRepository;
            _vacancyLocationReadRepository = vacancyLocationReadRepository;
            _vacancyLocationWriteRepository = vacancyLocationWriteRepository;
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

            return SaveVacancy(vacancy);
        }

        public Vacancy SaveVacancy(Vacancy vacancy)
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

        public List<VacancyLocation> GetVacancyLocations(int vacancyId)
        {
            return _vacancyLocationReadRepository.GetForVacancyId(vacancyId);
        }

        public List<VacancyLocation> SaveVacancyLocations(List<VacancyLocation> vacancyLocations)
        {
            return _vacancyLocationWriteRepository.Save(vacancyLocations);
        }

        public void DeleteVacancyLocationsFor(int vacancyId)
        {
            _vacancyLocationWriteRepository.DeleteFor(vacancyId);
        }
    }
}