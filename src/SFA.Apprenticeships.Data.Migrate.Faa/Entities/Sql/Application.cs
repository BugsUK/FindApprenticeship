namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class Application : ApplicationSummary
    {
        public int CandidateId { get; set; }
        public int VacancyId { get; set; }
        public int WithdrawnOrDeclinedReasonId { get; set; }
        public int UnsuccessfulReasonId { get; set; }
        public int NextActionId { get; set; }
        public string NextActionOther { get; set; }
        public int? CVAttachmentId { get; set; }
        public string BeingSupportedBy { get; set; }
        public DateTime? LockedForSupportUntil { get; set; }
        public bool? WithdrawalAcknowledged { get; set; }
        public Guid ApplicationGuid { get; set; }
    }
}