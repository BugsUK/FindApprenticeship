namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies
{
    public enum DurationType
    {
        // TODO: AG: consider 1-based enums per FAA with Unknown = 0.
        // TODO: AG: consider setting each value explicitly (e.g. if it is to appear in the database) to avoid 'casual insertions'.
        Unknown = -1,
        Weeks = 0,
        Months = 1,
        Years = 2
    }
}