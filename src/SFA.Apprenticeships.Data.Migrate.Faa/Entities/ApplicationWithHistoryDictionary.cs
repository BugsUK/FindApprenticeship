namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities
{
    using System.Collections.Generic;

    public class ApplicationWithHistoryDictionary
    {
        public IDictionary<string, object> Application { get; set; }
        public IList<IDictionary<string, object>> ApplicationHistory { get; set; }
    }
}