namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    public interface IVacancyWage
    {
        decimal? WageLowerBound { get; set; }
        string WageText { get; set; }
        int WageType { get; set; }
        string WageTypeReason { get; set; }
        int? WageUnitId { get; set; }
        decimal? WageUpperBound { get; set; }
        decimal? WeeklyWage { get; set; }
        decimal? HoursPerWeek { get; set; }
    }
}