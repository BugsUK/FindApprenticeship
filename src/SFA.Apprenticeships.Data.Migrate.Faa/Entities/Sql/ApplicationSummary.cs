namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    public class ApplicationSummary
    {
        public int ApplicationId { get; set; }
        public int ApplicationStatusTypeId { get; set; }
        public int UnsuccessfulReasonId { get; set; }
        public string OutcomeReasonOther { get; set; }
        public string AllocatedTo { get; set; }
    }
}