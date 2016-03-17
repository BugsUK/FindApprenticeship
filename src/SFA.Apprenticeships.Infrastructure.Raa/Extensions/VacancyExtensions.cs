namespace SFA.Apprenticeships.Infrastructure.Raa.Extensions
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using ApprenticeshipLevel = Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLevel;

    //TODO: This is only used by FAA for conversions - move to that project when found
    public static class VacancyExtensions
    {
        public static ApprenticeshipLevel GetApprenticeshipLevel(
            this Domain.Entities.Raa.Vacancies.ApprenticeshipLevel apprenticeshipLevel)
        {
            switch (apprenticeshipLevel)
            {
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Unknown:
                    return ApprenticeshipLevel.Unknown;
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Intermediate:
                    return ApprenticeshipLevel.Intermediate;
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Advanced:
                    return ApprenticeshipLevel.Advanced;
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Higher:
                    return ApprenticeshipLevel.Higher;
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.FoundationDegree:
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Degree:
                case Domain.Entities.Raa.Vacancies.ApprenticeshipLevel.Masters:
                    //TODO: Check what degree should map to
                    return ApprenticeshipLevel.Higher;
                default:
                    throw new ArgumentException("Apprenticeship Level: " + apprenticeshipLevel + " was not recognized");
            }
        }

        public static LegacyWageType GetLegacyWageType(this WageType wageType)
        {
            return LegacyWageType.LegacyText;
        }

        public static VacancyStatuses GetVacancyStatuses(this VacancyStatus vacancyStatuses)
        {
            switch (vacancyStatuses)
            {
                case VacancyStatus.Unknown:
                    return VacancyStatuses.Unknown;
                case VacancyStatus.Draft:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Submitted:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Live:
                    return VacancyStatuses.Live;
                case VacancyStatus.ReservedForQA:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Referred:
                    return VacancyStatuses.Unavailable;
                case VacancyStatus.Closed:
                    return VacancyStatuses.Expired;
                default:
                    throw new ArgumentException("Provider Vacancy Status: " + vacancyStatuses + " was not recognized");
            }
        }
    }
}