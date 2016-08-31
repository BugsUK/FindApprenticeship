namespace SFA.Apprenticeships.Infrastructure.Processes.Extensions
{
    using Application.Applications.Entities;

    using SFA.Apprenticeships.Application.Interfaces.Caching;

    public static class VacancyStatusSummaryExtensions
    {
        public static string CacheKey(this VacancyStatusSummary vacancyStatusSummary)
        {
            return vacancyStatusSummary != null
                ? string.Format("VacancyStatusSummary_CacheKey_{0}_{1}_{2}",
                    vacancyStatusSummary.LegacyVacancyId, vacancyStatusSummary.VacancyStatus, vacancyStatusSummary.ClosingDate.ToString("yyyy-MM-dd"))
                : null;
        }

        public static CacheDuration CacheDuration(this VacancyStatusSummary vacancyStatusSummary)
        {
            //todo: 1.6: move to config setting
            return Application.Interfaces.Caching.CacheDuration.ThirtyMinutes;
        }
    }
}
