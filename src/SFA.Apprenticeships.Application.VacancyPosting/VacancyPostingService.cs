﻿namespace SFA.Apprenticeships.Application.VacancyPosting
{
    //TODO: rename project to SFA.Management.Application.VacancyPosting?
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories;
    using Infrastructure.Interfaces;
    using Interfaces.Providers;
    using Interfaces.VacancyPosting;

    public class VacancyPostingService : IVacancyPostingService
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IReferenceNumberRepository _referenceNumberRepository;
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IVacancyLocationReadRepository _vacancyLocationReadRepository;
        private readonly IVacancyLocationWriteRepository _vacancyLocationWriteRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProviderVacancyAuthorisationService _providerVacancyAuthorisationService;

        public VacancyPostingService(
            IVacancyReadRepository vacancyReadRepository,
            IVacancyWriteRepository vacancyWriteRepository,
            IReferenceNumberRepository referenceNumberRepository,
            IProviderUserReadRepository providerUserReadRepository,
            IVacancyLocationReadRepository vacancyLocationReadRepository,
            IVacancyLocationWriteRepository vacancyLocationWriteRepository,
            ICurrentUserService currentUserService,
            IProviderVacancyAuthorisationService providerVacancyAuthorisationService)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _referenceNumberRepository = referenceNumberRepository;
            _providerUserReadRepository = providerUserReadRepository;
            _vacancyLocationReadRepository = vacancyLocationReadRepository;
            _vacancyLocationWriteRepository = vacancyLocationWriteRepository;
            _currentUserService = currentUserService;
            _providerVacancyAuthorisationService = providerVacancyAuthorisationService;
        }

        public Vacancy CreateApprenticeshipVacancy(Vacancy vacancy)
        {
            Condition.Requires(vacancy);

            if (_currentUserService.IsInRole(Roles.Faa))
            {
                var username = _currentUserService.CurrentUserName;
                var vacancyManager = _providerUserReadRepository.GetByUsername(username);
                if (vacancyManager?.PreferredProviderSiteId != null)
                {
                    vacancy.VacancyManagerId = vacancyManager.PreferredProviderSiteId.Value;
                }
            }

            return SaveVacancy(vacancy);
        }

        public Vacancy SaveVacancy(Vacancy vacancy)
        {
            return UpsertVacancy(vacancy, v => _vacancyWriteRepository.Create(v));
        }

        public Vacancy UpdateVacancy(Vacancy vacancy)
        {
            return UpsertVacancy(vacancy, v => _vacancyWriteRepository.Update(v));
        }

        public int GetNextVacancyReferenceNumber()
        {
            return _referenceNumberRepository.GetNextVacancyReferenceNumber();
        }

        public Vacancy GetVacancyByReferenceNumber(int vacancyReferenceNumber)
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

        public List<VacancySummary> GetWithStatus(params VacancyStatus[] desiredStatuses)
        {
            return _vacancyReadRepository.GetWithStatus(0, 0, desiredStatuses);
        }

        public List<VacancySummary> GetByIds(IEnumerable<int> vacancyIds)
        {
            return _vacancyReadRepository.GetByIds(vacancyIds);
        }

        public List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds)
        {
            return _vacancyReadRepository.GetByOwnerPartyIds(ownerPartyIds);
        }

        public Vacancy ReserveVacancyForQA(int vacancyReferenceNumber)
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

        private Vacancy UpsertVacancy(Vacancy vacancy, Func<Vacancy, Vacancy> operation)
        {
            Condition.Requires(vacancy);

            if (vacancy.Status == VacancyStatus.Completed)
            {
                var message = $"Vacancy {vacancy.VacancyReferenceNumber} can not be in Completed status on saving.";
                throw new CustomException(message, ErrorCodes.EntityStateError);
            }

            if (_currentUserService.IsInRole(Roles.Faa))
            {
                var username = _currentUserService.CurrentUserName;
                var lastEditedBy = _providerUserReadRepository.GetByUsername(username);
                if (lastEditedBy != null)
                {
                    vacancy.LastEditedById = lastEditedBy.ProviderUserId;
                }
            }

            vacancy = operation(vacancy);

            return _vacancyReadRepository.Get(vacancy.VacancyId);
        }
    }
}
