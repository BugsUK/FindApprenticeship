namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Interfaces.Service;

    public interface IDeleteValidationStrategy : IServiceStrategy<VacancySummary>
    {
    }
}