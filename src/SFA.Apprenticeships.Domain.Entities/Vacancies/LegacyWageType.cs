namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    // TODO: AG: rename to WageType.
    public enum LegacyWageType
    {
        // Wages are not applicable for traineeships.
        NotApplicable = -1,
        // Starts at zero to support direct mapping to legacy system.
        LegacyText = 0,
        LegacyWeekly = 1,
        ApprenticeshipMinimum = 2,
        NationalMinimum = 3,
        Custom = 4
    }
}
