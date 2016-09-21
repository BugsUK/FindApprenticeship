namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities
{
    using Mongo;
    using Repository.Sql;
    using Sql;

    public class ApplicationWithSubVacancy
    {
        public Application Application { get; set; }
        public SchoolAttended SchoolAttended { get; set; }
        public SubVacancy SubVacancy { get; set; }
        public bool UpdateNotes { get; set; }
        public ApplicationStatuses? UpdateStatusTo { get; set; }
        //public string UpdateUnsuccessfulReasonTo { get; set; }
    }
}