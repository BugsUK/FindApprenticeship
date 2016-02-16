namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies
{
    public enum DurationType
    {
        // TODO: AG: consider 1-based enums per FAA with Unknown = 0.
        // TODO: AG: consider setting each value explicitly (e.g. if it is to appear in the database) to avoid 'casual insertions'.
        Unknown = 0, //was -1
        Weeks = 1, //was 0
        Months = 2, //was 1
        Years = 3 //was 2
    }
}
