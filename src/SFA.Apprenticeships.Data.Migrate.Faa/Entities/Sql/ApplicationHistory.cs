namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class ApplicationHistory
    {
        public int ApplicationHistoryId { get; set; }
        public int ApplicationId { get; set; }
        public string UserName { get; set; }
        public DateTime ApplicationHistoryEventDate { get; set; }
        public int ApplicationHistoryEventTypeId { get; set; }
        public int ApplicationHistoryEventSubTypeId { get; set; }
        public string Comment { get; set; }
    }
}