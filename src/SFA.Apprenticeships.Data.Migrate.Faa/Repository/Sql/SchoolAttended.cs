namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;

    public class SchoolAttended
    {
        public int SchoolAttendedId { get; set; }
        public int CandidateId { get; set; }
        public int? SchoolId { get; set; }
        public string OtherSchoolName { get; set; }
        public string OtherSchoolTown { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ApplicationId { get; set; }
    }
}