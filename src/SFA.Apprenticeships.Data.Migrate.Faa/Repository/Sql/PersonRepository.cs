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
            var personIds = new Dictionary<string, int>();
            //There are duplicates in the person table so use the most recent based on id
            var keyValuePairs = _getOpenConnection.Query<KeyValuePair<string, int>>("SELECT Email as [Key], PersonId as Value FROM Person WHERE Email in @emails ORDER BY Value", new { emails });
            foreach (var keyValuePair in keyValuePairs)
            {
                personIds[keyValuePair.Key] = keyValuePair.Value;
            }
            return personIds;
        }
    }
}