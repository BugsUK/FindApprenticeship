namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Interfaces.Service;

    public interface IDeleteVacancyStrategy : IServiceStrategy<VacancySummary>
    {
    }
}