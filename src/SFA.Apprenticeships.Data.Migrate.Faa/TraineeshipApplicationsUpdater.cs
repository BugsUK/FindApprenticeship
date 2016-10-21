namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using Repository.Sql;

    public class TraineeshipApplicationsUpdater : IVacancyApplicationsUpdater
    {
        private readonly SyncRepository _syncRepository;
        
        public TraineeshipApplicationsUpdater(SyncRepository syncRepository)
        {
            _syncRepository = syncRepository;
        }

        public string CollectionName => "traineeships";
        public bool IsValidForIncrementalSync => _syncRepository.GetSyncParams().IsValidForTraineeshipIncrementalSync;
        public DateTime VacancyApplicationLastCreatedDate => _syncRepository.GetSyncParams().TraineeshipLastCreatedDate;
        public DateTime VacancyApplicationLastUpdatedDate => _syncRepository.GetSyncParams().TraineeshipLastUpdatedDate;
        public void UpdateLastCreatedSyncDate(DateTime maxDateCreated)
        {
            var syncParams = _syncRepository.GetSyncParams();
            syncParams.TraineeshipLastCreatedDate = maxDateCreated > syncParams.TraineeshipLastCreatedDate ? maxDateCreated : syncParams.TraineeshipLastCreatedDate;
            _syncRepository.SetTraineeshipSyncParams(syncParams);
        }
        public void UpdateLastUpdatedSyncDate(DateTime maxDateUpdated)
        {
            var syncParams = _syncRepository.GetSyncParams();
            syncParams.TraineeshipLastUpdatedDate = maxDateUpdated > syncParams.TraineeshipLastUpdatedDate ? maxDateUpdated : syncParams.TraineeshipLastUpdatedDate;
            _syncRepository.SetTraineeshipSyncParams(syncParams);
        }
    }
}