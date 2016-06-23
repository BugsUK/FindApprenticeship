namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class ApplicationHistorySummary
    {
        public int ApplicationHistoryId { get; set; }
        public int ApplicationId { get; set; }
        public DateTime ApplicationHistoryEventDate { get; set; }
        public int ApplicationHistoryEventSubTypeId { get; set; }
    }
}