namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;

    public class QaVacancyStrategies : IQaVacancyStrategies
    {
        private readonly IVacancyWriteRepository _vacancyWriteRepository;

        public QaVacancyStrategies(IVacancyWriteRepository vacancyWriteRepository)
        {
            _vacancyWriteRepository = vacancyWriteRepository;
        }

        public Vacancy ReserveVacancyForQa(int vacancyReferenceNumber)
        {
            return _vacancyWriteRepository.ReserveVacancyForQA(vacancyReferenceNumber);
        }

        public void UnReserveVacancyForQa(int vacancyReferenceNumber)
        {
            _vacancyWriteRepository.UnReserveVacancyForQa(vacancyReferenceNumber);
        }
    }
}