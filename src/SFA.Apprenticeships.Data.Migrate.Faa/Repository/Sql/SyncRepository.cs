namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;
    using System.Linq;
    using Infrastructure.Repositories.Sql.Common;

    public class SyncRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public SyncRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public DateTime? GetApplicationLastCreatedDate()
        {
            return _getOpenConnection.Query<DateTime?>("SELECT ApplicationLastCreatedDate FROM Sync.SyncParams").SingleOrDefault();
        }

        public DateTime? GetApplicationLastUpdatedDate()
        {
            return _getOpenConnection.Query<DateTime?>("SELECT ApplicationLastUpdatedDate FROM Sync.SyncParams").SingleOrDefault();
        }

        public void SetApplicationLastCreatedDate(DateTime applicationLastCreatedDate)
        {
            _getOpenConnection.MutatingQuery<int>($"UPDATE Sync.SyncParams SET ApplicationLastCreatedDate = @applicationLastCreatedDate", new { applicationLastCreatedDate });
        }

        public void SetApplicationLastUpdatedDate(DateTime applicationLastUpdatedDate)
        {
            _getOpenConnection.MutatingQuery<int>($"UPDATE Sync.SyncParams SET ApplicationLastUpdatedDate = @applicationLastUpdatedDate", new { applicationLastUpdatedDate });
        }
    }
}