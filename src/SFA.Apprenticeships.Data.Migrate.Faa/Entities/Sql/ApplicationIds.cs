namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class ApplicationIds
    {
        public int ApplicationId { get; set; }
        public int CandidateId { get; set; }
        public int VacancyId { get; set; }
        public Guid ApplicationGuid { get; set; }
    }
}