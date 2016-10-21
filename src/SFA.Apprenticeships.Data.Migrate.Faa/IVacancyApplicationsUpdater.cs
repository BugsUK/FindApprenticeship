namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;

    public interface IVacancyApplicationsUpdater
    {
        string CollectionName { get; }
        bool IsValidForIncrementalSync { get; }
        DateTime VacancyApplicationLastCreatedDate { get; }
        DateTime VacancyApplicationLastUpdatedDate { get; }
        void UpdateLastCreatedSyncDate(DateTime maxDateCreated);
        void UpdateLastUpdatedSyncDate(DateTime maxDateUpdated);
    }
}