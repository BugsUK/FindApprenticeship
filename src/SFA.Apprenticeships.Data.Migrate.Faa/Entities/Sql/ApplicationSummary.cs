namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class ApplicationSummary
    {
        public int ApplicationId { get; set; }
        public string OutcomeReasonOther { get; set; }
        public Guid ApplicationGuid { get; set; }
    }
}