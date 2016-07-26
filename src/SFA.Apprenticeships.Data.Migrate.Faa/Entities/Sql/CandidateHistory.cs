namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class CandidateHistory
    {
        public int CandidateHistoryId { get; set; }
        public int CandidateId { get; set; }
        public int CandidateHistoryEventTypeId { get; set; }
        public int? CandidateHistorySubEventTypeId { get; set; }
        public DateTime EventDate { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
    }
}