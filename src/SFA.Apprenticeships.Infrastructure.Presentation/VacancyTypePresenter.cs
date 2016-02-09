namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Domain.Entities.Vacancies;

    public static class VacancyTypePresenter
    {
        const string TraineeshipPrefix = "Traineeship in ";

        public static string GetTitle(this VacancyType vacancyType, string title)
        {
            if (vacancyType == VacancyType.Traineeship && !title.StartsWith(TraineeshipPrefix))
            {
                return TraineeshipPrefix + title;
            }

            return title;
        }
    }
}