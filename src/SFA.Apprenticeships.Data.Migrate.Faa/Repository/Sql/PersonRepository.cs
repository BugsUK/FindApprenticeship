namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Repositories.Sql.Common;

    public class PersonRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public PersonRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IDictionary<string, int> GetPersonIdsByEmails(IEnumerable<string> emails)
        {
            return _getOpenConnection.Query<KeyValuePair<string, int>>("SELECT Email as [Key], PersonId as Value FROM Person WHERE Email in @emails", new { emails }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}