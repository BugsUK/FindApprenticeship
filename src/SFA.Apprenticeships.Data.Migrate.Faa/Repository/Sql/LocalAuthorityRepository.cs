namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Repositories.Sql.Common;

    public class LocalAuthorityRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public LocalAuthorityRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IDictionary<int, int> GetLocalAuthorityCountyIds()
        {
            return _getOpenConnection.Query<KeyValuePair<int, int>>("SELECT LocalAuthorityId as [Key], CountyId as Value FROM LocalAuthority").ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}