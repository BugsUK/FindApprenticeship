namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities
{
    using Repository.Sql;
    using Sql;

    public class ApplicationWithSubVacancy
    {
        public Application Application { get; set; }
        public SchoolAttended SchoolAttended { get; set; }
        public SubVacancy SubVacancy { get; set; } 
    }
}