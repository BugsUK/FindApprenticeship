namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Domain.Entities.Vacancies.ProviderVacancies;

    public static class DurationPresenter
    {
        public static string GetDisplayText(this Duration duration)
        {
            switch (duration.Type)
            {
                case DurationType.Weeks:
                    return duration.Length + " weeks";
                case DurationType.Months:
                    return duration.Length + " months";
                case DurationType.Years:
                    return duration.Length + " years";
                default:
                    return string.Empty;
            }
        }
    }
}