namespace SFA.Apprenticeships.Infrastructure.Raa.Extensions
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using VacancyStatus = Domain.Entities.Raa.Vacancies.VacancyStatus;

    //TODO: This is only used by FAA for conversions - move to that project when found
    public static class VacancyExtensions
    {
        public static Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLevel GetApprenticeshipLevel(this ApprenticeshipLevel apprenticeshipLevel)
        {
            switch (apprenticeshipLevel)
            {
                case ApprenticeshipLevel.Unknown:
                    return Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLevel.Unknown;
                case ApprenticeshipLevel.Intermediate:
                    return Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLevel.Intermediate;
                case ApprenticeshipLevel.Advanced:
                    return Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLevel.Advanced;
                case ApprenticeshipLevel.Higher:
                    return Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLevel.Higher;
                case ApprenticeshipLevel.FoundationDegree:
                case ApprenticeshipLevel.Degree:
                case ApprenticeshipLevel.Masters:
                    //TODO: Check what degree should map to
                    return Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLevel.Higher;
                default:
                    throw new ArgumentException("Apprenticeship Level: " + apprenticeshipLevel + " was not recognized");
            }
        }

        public static LegacyWageType GetLegacyWageType(this WageType wageType)
        {
            return LegacyWageType.Text;
        }

        public static Domain.Entities.Vacancies.VacancyStatuses GetVacancyStatuses(this VacancyStatus VacancyStatuses)
        {
            switch (VacancyStatuses)
            {
                case Domain.Entities.Raa.Vacancies.VacancyStatus.Unknown:
                    return Domain.Entities.Vacancies.VacancyStatuses.Unknown;
                case Domain.Entities.Raa.Vacancies.VacancyStatus.Draft:
                    return Domain.Entities.Vacancies.VacancyStatuses.Unavailable;
                case Domain.Entities.Raa.Vacancies.VacancyStatus.PendingQA:
                    return Domain.Entities.Vacancies.VacancyStatuses.Unavailable;
                case Domain.Entities.Raa.Vacancies.VacancyStatus.Live:
                    return Domain.Entities.Vacancies.VacancyStatuses.Live;
                case Domain.Entities.Raa.Vacancies.VacancyStatus.ReservedForQA:
                    return Domain.Entities.Vacancies.VacancyStatuses.Unavailable;
                case Domain.Entities.Raa.Vacancies.VacancyStatus.RejectedByQA:
                    return Domain.Entities.Vacancies.VacancyStatuses.Unavailable;
                default:
                    throw new ArgumentException("Provider Vacancy Status: " + VacancyStatuses + " was not recognized");
            }
        }
    }
}