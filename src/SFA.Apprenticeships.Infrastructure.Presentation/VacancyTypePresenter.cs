namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Domain.Entities.Raa.Vacancies;

    public static class VacancyTypePresenter
    {
        public static string GetTitle(this VacancyType vacancyType, string title)
        {
            return title;
        }
    }
}