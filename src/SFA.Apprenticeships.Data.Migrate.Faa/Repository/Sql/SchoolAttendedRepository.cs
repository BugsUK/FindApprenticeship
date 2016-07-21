namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Repositories.Sql.Common;

    public class SchoolAttendedRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public SchoolAttendedRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IDictionary<int, int> GetSchoolAttendedIdsByApplicationIds(IEnumerable<int> applicationIds)
        {
            var schoolAttendedIds = _getOpenConnection.Query<SchoolAttendedIds>("SELECT SchoolAttendedId, ApplicationId FROM SchoolAttended WHERE ApplicationId in @applicationIds", new { applicationIds });
            return schoolAttendedIds.ToDictionary(sa => sa.ApplicationId, sa => sa.SchoolAttendedId);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class SchoolAttendedIds
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public int SchoolAttendedId { get; set; }
            public int ApplicationId { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}