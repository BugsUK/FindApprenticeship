namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    public enum LegacyWageType
    {
        // NOTE: enum starts at zero to support direct mapping to legacy system.
        LegacyText = 0,
        LegacyWeekly = 1,
        ApprenticeshipMinimum = 2,
        NationalMinimum = 3,
        Custom = 4
    }
}
