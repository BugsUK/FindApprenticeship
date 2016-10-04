namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;

    public class UpdateVacancyStrategy : IUpdateVacancyStrategy
    {
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IUpsertVacancyStrategy _upsertVacancyStrategy;

        public UpdateVacancyStrategy(IVacancyWriteRepository vacancyWriteRepository, IUpsertVacancyStrategy upsertVacancyStrategy)
        {
            _vacancyWriteRepository = vacancyWriteRepository;
            _upsertVacancyStrategy = upsertVacancyStrategy;
        }

        public Vacancy UpdateVacancy(Vacancy vacancy)
        {
            if (vacancy.Status == VacancyStatus.Completed)
            {
                var message = $"Vacancy {vacancy.VacancyReferenceNumber} can not be in Completed status on saving.";
                throw new CustomException(message, ErrorCodes.EntityStateError);
            }

            // Make sure the standard is null if the vacancy is a traineeship (RA-30)
            SetStandardAsNullIfTraineeship(vacancy);
            SetSectorAsNullIfApprenticeship(vacancy);

            return _upsertVacancyStrategy.UpsertVacancy(vacancy, v => _vacancyWriteRepository.Update(v));
        }

        public Vacancy UpdateVacancyWithNewProvider(Vacancy vacancy)
        {
            var updatedVacancy = _upsertVacancyStrategy.UpsertVacancyForAdmin(vacancy, v => _vacancyWriteRepository.Update(v));
            return updatedVacancy;
        }

        private static void SetSectorAsNullIfApprenticeship(VacancySummary vacancy)
        {
            if (vacancy.VacancyType == VacancyType.Apprenticeship)
            {
                vacancy.SectorCodeName = null;
            }
        }

        private static void SetStandardAsNullIfTraineeship(VacancySummary vacancy)
        {
            if (vacancy.VacancyType == VacancyType.Traineeship)
            {
                vacancy.StandardId = null;
            }
        }
    }
}