namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities
{
    using Sql;

    public class ApplicationWithSubVacancy
    {
        public Application Application { get; set; }
        public SubVacancy SubVacancy { get; set; } 
    }
}