namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities
{
    using System.Collections.Generic;
    using Sql;

    public class ApplicationWithHistory
    {
        public ApplicationWithSubVacancy ApplicationWithSubVacancy { get; set; }
        public IList<ApplicationHistory> ApplicationHistory { get; set; }
    }
}