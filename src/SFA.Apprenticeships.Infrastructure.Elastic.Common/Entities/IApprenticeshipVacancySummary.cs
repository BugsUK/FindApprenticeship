namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    public interface IApprenticeshipVacancySummary : IVacancySummary
    {
        string Wage { get; set; }

        string WorkingWeek { get; set; }
    }
}
