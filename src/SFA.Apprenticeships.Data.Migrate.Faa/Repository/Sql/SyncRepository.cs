namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Sql
{
    using System;
    using System.Linq;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;

    public class SyncRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public SyncRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public SyncParams GetSyncParams()
        {
            var databaseSyncParams = _getOpenConnection.Query<DatabaseSyncParams>("SELECT ApprenticeshipLastCreatedDate, ApprenticeshipLastUpdatedDate, TraineeshipLastCreatedDate, TraineeshipLastUpdatedDate, CandidateLastCreatedDate, CandidateLastUpdatedDate FROM Sync.SyncParams").SingleOrDefault() ?? new DatabaseSyncParams();

            var syncParams = new SyncParams();
            
            //SQL doesn't store the "kind" property for DateTime and this is required for correct comparison to MongoDB DateTimes
            if (databaseSyncParams.ApprenticeshipLastCreatedDate.HasValue)
            {
                syncParams.ApprenticeshipLastCreatedDate = new DateTime(databaseSyncParams.ApprenticeshipLastCreatedDate.Value.Ticks, DateTimeKind.Utc);
            }
            if (databaseSyncParams.ApprenticeshipLastUpdatedDate.HasValue)
            {
                syncParams.ApprenticeshipLastUpdatedDate = new DateTime(databaseSyncParams.ApprenticeshipLastUpdatedDate.Value.Ticks, DateTimeKind.Utc);
            }
            if (databaseSyncParams.TraineeshipLastCreatedDate.HasValue)
            {
                syncParams.TraineeshipLastCreatedDate = new DateTime(databaseSyncParams.TraineeshipLastCreatedDate.Value.Ticks, DateTimeKind.Utc);
            }
            if (databaseSyncParams.TraineeshipLastUpdatedDate.HasValue)
            {
                syncParams.TraineeshipLastUpdatedDate = new DateTime(databaseSyncParams.TraineeshipLastUpdatedDate.Value.Ticks, DateTimeKind.Utc);
            }
            if (databaseSyncParams.CandidateLastCreatedDate.HasValue)
            {
                syncParams.CandidateLastCreatedDate = new DateTime(databaseSyncParams.CandidateLastCreatedDate.Value.Ticks, DateTimeKind.Utc);
            }
            if (databaseSyncParams.CandidateLastUpdatedDate.HasValue)
            {
                syncParams.CandidateLastUpdatedDate = new DateTime(databaseSyncParams.CandidateLastUpdatedDate.Value.Ticks, DateTimeKind.Utc);
            }

            return syncParams;
        }

        public void SetApprenticeshipSyncParams(SyncParams syncParams)
        {
            _getOpenConnection.MutatingQuery<int>(
                @"UPDATE Sync.SyncParams SET ApprenticeshipLastCreatedDate = @apprenticeshipLastCreatedDate, ApprenticeshipLastUpdatedDate = @apprenticeshipLastUpdatedDate",
                new
                {
                    apprenticeshipLastCreatedDate = syncParams.ApprenticeshipLastCreatedDate == DateTime.MinValue ? null : (DateTime?)syncParams.ApprenticeshipLastCreatedDate,
                    apprenticeshipLastUpdatedDate = syncParams.ApprenticeshipLastUpdatedDate == DateTime.MinValue ? null : (DateTime?)syncParams.ApprenticeshipLastUpdatedDate
                });
        }

        public void SetTraineeshipSyncParams(SyncParams syncParams)
        {
            _getOpenConnection.MutatingQuery<int>(
                @"UPDATE Sync.SyncParams SET TraineeshipLastCreatedDate = @traineeshipLastCreatedDate, TraineeshipLastUpdatedDate = @traineeshipLastUpdatedDate",
                new
                {
                    traineeshipLastCreatedDate = syncParams.TraineeshipLastCreatedDate == DateTime.MinValue ? null : (DateTime?)syncParams.TraineeshipLastCreatedDate,
                    traineeshipLastUpdatedDate = syncParams.TraineeshipLastUpdatedDate == DateTime.MinValue ? null : (DateTime?)syncParams.TraineeshipLastUpdatedDate
                });
        }

        public void SetCandidateSyncParams(SyncParams syncParams)
        {
            _getOpenConnection.MutatingQuery<int>(
                @"UPDATE Sync.SyncParams SET CandidateLastCreatedDate = @candidateLastCreatedDate, CandidateLastUpdatedDate = @candidateLastUpdatedDate",
                new
                {
                    candidateLastCreatedDate = syncParams.CandidateLastCreatedDate == DateTime.MinValue ? null : (DateTime?)syncParams.CandidateLastCreatedDate,
                    candidateLastUpdatedDate = syncParams.CandidateLastUpdatedDate == DateTime.MinValue ? null : (DateTime?)syncParams.CandidateLastUpdatedDate
                });
        }

        public class DatabaseSyncParams
        {
            public DateTime? ApprenticeshipLastCreatedDate { get; set; }
            public DateTime? ApprenticeshipLastUpdatedDate { get; set; }
            public DateTime? TraineeshipLastCreatedDate { get; set; }
            public DateTime? TraineeshipLastUpdatedDate { get; set; }
            public DateTime? CandidateLastCreatedDate { get; set; }
            public DateTime? CandidateLastUpdatedDate { get; set; }
        }
    }
}