namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    public interface IApprenticeshipVacancySummary : IVacancySummary
    {
        string WorkingWeek { get; set; }

        int WageType { get; set; }

        decimal? WageAmount { get; set; }

        string WageText { get; set; }

        int WageUnit { get; set; }

        decimal? HoursPerWeek { get; set; }
    }
}
