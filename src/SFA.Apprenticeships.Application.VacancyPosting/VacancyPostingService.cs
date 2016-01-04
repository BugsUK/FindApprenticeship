namespace SFA.Apprenticeships.Application.VacancyPosting
{
    //TODO: rename project to SFA.Management.Application.VacancyPosting?
    using System;
    using System.Threading;
    using CuttingEdge.Conditions;
    using Domain.Entities;
    using Domain.Entities.Users;
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

            if (Thread.CurrentPrincipal.IsInRole(Roles.Faa))
            {
                var username = Thread.CurrentPrincipal.Identity.Name;
                var lastEditedBy = _providerUserReadRepository.Get(username);
                if (lastEditedBy != null)
                {
                    vacancy.LastEditedById = lastEditedBy.EntityId;
                }
            }

            _apprenticeshipVacancyWriteRepository.Save(vacancy);

            return _apprenticeshipVacancyReadRepository.Get(vacancy.EntityId);
        }

        public long GetNextVacancyReferenceNumber()
        {
            return _referenceNumberRepository.GetNextVacancyReferenceNumber();
        }

        public ApprenticeshipVacancy GetVacancy(long vacancyReferenceNumber)
        {
            return _apprenticeshipVacancyReadRepository.Get(vacancyReferenceNumber);
        }

        public ApprenticeshipVacancy GetVacancy(Guid vacancyGuid)
        {
            return _apprenticeshipVacancyReadRepository.Get(vacancyGuid);
        }
    }
}