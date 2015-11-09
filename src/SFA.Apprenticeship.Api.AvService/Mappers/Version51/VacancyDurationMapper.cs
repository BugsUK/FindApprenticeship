namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;

    public static class VacancyDurationMapper
    {
        public static string MapDurationToString(int? duration, DurationType durationType)
        {
            if (!duration.HasValue)
            {
                // TODO: what if duration null?
                return string.Empty;
            }

            string unit;

            switch (durationType)
            {
                case DurationType.Weeks:
                    unit = "week";
                    break;
                case DurationType.Months:
                    unit = "month";
                    break;
                case DurationType.Years:
                    unit = "year";
                    break;
                default:
                    // TODO: what if durationType is unknown?
                    return string.Empty;
            }

            return $"{duration.Value} {unit}{Pluralise(duration.Value)}";
        }

        private static string Pluralise(int duration)
        {
            return duration == 1 ? string.Empty : "s";
        }
    }
}
