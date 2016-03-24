namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System;

    public interface IVacancySummary
    {
        int Id { get; }

        string Title { get; set; }

        DateTime ClosingDate { get; set; }

        DateTime PostedDate { get; set; }

        string EmployerName { get; set; }

        string ProviderName { get; set; }

        string Description { get; set; }

        int NumberOfPositions { get; set; }

        GeoPoint Location { get; set; }

        string VacancyReference { get; set; }

        string Category { get; set; }
    }
}
