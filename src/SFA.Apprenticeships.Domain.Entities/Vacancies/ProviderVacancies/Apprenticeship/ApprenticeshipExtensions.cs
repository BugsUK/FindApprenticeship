namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship
{
    using System;

    public static class ApprenticeshipExtensions
    {
        public static Apprenticeships.ApprenticeshipLevel GetApprenticeshipLevel(this ApprenticeshipLevel apprenticeshipLevel)
        {
            switch (apprenticeshipLevel)
            {
                case ApprenticeshipLevel.Unknown:
                    return Apprenticeships.ApprenticeshipLevel.Unknown;
                case ApprenticeshipLevel.Intermediate:
                    return Apprenticeships.ApprenticeshipLevel.Intermediate;
                case ApprenticeshipLevel.Advanced:
                    return Apprenticeships.ApprenticeshipLevel.Advanced;
                case ApprenticeshipLevel.Higher:
                    return Apprenticeships.ApprenticeshipLevel.Higher;
                case ApprenticeshipLevel.FoundationDegree:
                case ApprenticeshipLevel.Degree:
                case ApprenticeshipLevel.Masters:
                    //TODO: Check what degree should map to
                    return Apprenticeships.ApprenticeshipLevel.Higher;
                default:
                    throw new ArgumentException("Apprenticeship Level: " + apprenticeshipLevel + " was not recognized");
            }
        }

        public static LegacyWageType GetLegacyWageType(this WageType wageType)
        {
            return LegacyWageType.Text;
        }

        public static VacancyStatuses GetVacancyStatuses(this ProviderVacancyStatuses providerVacancyStatuses)
        {
            switch (providerVacancyStatuses)
            {
                case ProviderVacancyStatuses.Unknown:
                    return VacancyStatuses.Unknown;
                case ProviderVacancyStatuses.Draft:
                    return VacancyStatuses.Unavailable;
                case ProviderVacancyStatuses.PendingQA:
                    return VacancyStatuses.Unavailable;
                case ProviderVacancyStatuses.Live:
                    return VacancyStatuses.Live;
                case ProviderVacancyStatuses.ReservedForQA:
                    return VacancyStatuses.Unavailable;
                case ProviderVacancyStatuses.RejectedByQA:
                    return VacancyStatuses.Unavailable;
                default:
                    throw new ArgumentException("Provider Vacancy Status: " + providerVacancyStatuses + " was not recognized");
            }
        }
    }
}