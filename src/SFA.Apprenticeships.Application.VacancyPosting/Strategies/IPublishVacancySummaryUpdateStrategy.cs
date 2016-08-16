namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Entities.Raa.Vacancies;

    public interface IPublishVacancySummaryUpdateStrategy
    {
        void PublishVacancySummaryUpdate(Vacancy vacancy);
    }
}