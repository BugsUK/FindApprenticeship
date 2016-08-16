namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Entities.Raa.Vacancies;

    public interface IQaVacancyStrategies
    {
        Vacancy ReserveVacancyForQa(int vacancyReferenceNumber);

        void UnReserveVacancyForQa(int vacancyReferenceNumber);

    }
}