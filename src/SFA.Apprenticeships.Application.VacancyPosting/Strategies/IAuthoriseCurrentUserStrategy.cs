namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Entities.Raa.Vacancies;

    public interface IAuthoriseCurrentUserStrategy
    {
        Vacancy AuthoriseCurrentUser(Vacancy vacancy);
    }
}