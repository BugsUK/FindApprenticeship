namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;

    public class ArchiveVacancyStrategy : IArchiveVacancyStrategy
    {
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IUpsertVacancyStrategy _upsertVacancyStrategy;

        public ArchiveVacancyStrategy(IVacancyWriteRepository vacancyWriteRepository, IUpsertVacancyStrategy upsertVacancyStrategy)
        {
            _vacancyWriteRepository = vacancyWriteRepository;
            _upsertVacancyStrategy = upsertVacancyStrategy;
        }

        public Vacancy ArchiveVacancy(Vacancy vacancy)
        {
            Condition.Requires(vacancy);

            vacancy.Status = VacancyStatus.Completed;

            return _upsertVacancyStrategy.UpsertVacancy(vacancy, v => _vacancyWriteRepository.Update(v));
        }
    }
}