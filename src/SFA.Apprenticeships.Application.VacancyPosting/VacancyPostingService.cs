using System;

namespace SFA.Apprenticeships.Application.VacancyPosting
{
    using CuttingEdge.Conditions;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Repositories;
    using Interfaces.VacancyPosting;

    public class VacancyPostingService : IVacancyPostingService
    {
        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
        private readonly IApprenticeshipVacancyWriteRepository _apprenticeshipVacancyWriteRepository;
        private readonly IReferenceNumberRepository _referenceNumberRepository;

        public VacancyPostingService(
            IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository,
            IApprenticeshipVacancyWriteRepository apprenticeshipVacancyWriteRepository,
            IReferenceNumberRepository referenceNumberRepository)
        {
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
            _apprenticeshipVacancyWriteRepository = apprenticeshipVacancyWriteRepository;
            _referenceNumberRepository = referenceNumberRepository;
        }

        public ApprenticeshipVacancy SaveApprenticeshipVacancy(ApprenticeshipVacancy vacancy)
        {
            Condition.Requires(vacancy);

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
