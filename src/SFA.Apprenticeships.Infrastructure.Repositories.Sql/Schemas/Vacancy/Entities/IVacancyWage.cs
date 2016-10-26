namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    public interface IVacancyWage
    {
        string WageText { get; set; }
        int WageType { get; set; }
        int? WageUnitId { get; set; }
        decimal? WeeklyWage { get; set; }
        decimal? HoursPerWeek { get; set; }
    }
}