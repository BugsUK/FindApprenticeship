namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using Repository.Sql;

    public interface IVacancyApplicationsUpdater
    {
        string CollectionName { get; }
        bool IsValidForIncrementalSync { get; }
        bool LoadAllCandidatesBeforeProcessing { get; }
        DateTime VacancyApplicationLastCreatedDate { get; }
        DateTime VacancyApplicationLastUpdatedDate { get; }
        void UpdateSyncDates(DateTime maxDateCreated, DateTime maxDateUpdated);
    }

    public class ApprenticeshipApplicationsUpdater : IVacancyApplicationsUpdater
    {
        private readonly SyncRepository _syncRepository;
        
        public ApprenticeshipApplicationsUpdater(SyncRepository syncRepository)
        {
            _syncRepository = syncRepository;
        }

        public string CollectionName => "apprenticeships";
        public bool IsValidForIncrementalSync => _syncRepository.GetSyncParams().IsValidForApprenticeshipIncrementalSync;
        public bool LoadAllCandidatesBeforeProcessing => true;
        public DateTime VacancyApplicationLastCreatedDate => _syncRepository.GetSyncParams().ApprenticeshipLastCreatedDate;
        public DateTime VacancyApplicationLastUpdatedDate => _syncRepository.GetSyncParams().ApprenticeshipLastUpdatedDate;
        public void UpdateSyncDates(DateTime maxDateCreated, DateTime maxDateUpdated)
        {
            var syncParams = _syncRepository.GetSyncParams();
            syncParams.ApprenticeshipLastCreatedDate = maxDateCreated > syncParams.ApprenticeshipLastCreatedDate ? maxDateCreated : syncParams.ApprenticeshipLastCreatedDate;
            syncParams.ApprenticeshipLastUpdatedDate = maxDateUpdated > syncParams.ApprenticeshipLastUpdatedDate ? maxDateUpdated : syncParams.ApprenticeshipLastUpdatedDate;
            _syncRepository.SetApprenticeshipSyncParams(syncParams);
        }
    }

    public class TraineeshipApplicationsUpdater : IVacancyApplicationsUpdater
    {
        private readonly SyncRepository _syncRepository;
        
        public TraineeshipApplicationsUpdater(SyncRepository syncRepository)
        {
            _syncRepository = syncRepository;
        }

        public string CollectionName => "traineeships";
        public bool IsValidForIncrementalSync => _syncRepository.GetSyncParams().IsValidForTraineeshipIncrementalSync;
        public bool LoadAllCandidatesBeforeProcessing => false;
        public DateTime VacancyApplicationLastCreatedDate => _syncRepository.GetSyncParams().TraineeshipLastCreatedDate;
        public DateTime VacancyApplicationLastUpdatedDate => _syncRepository.GetSyncParams().TraineeshipLastUpdatedDate;
        public void UpdateSyncDates(DateTime maxDateCreated, DateTime maxDateUpdated)
        {
            var syncParams = _syncRepository.GetSyncParams();
            syncParams.TraineeshipLastCreatedDate = maxDateCreated > syncParams.TraineeshipLastCreatedDate ? maxDateCreated : syncParams.TraineeshipLastCreatedDate;
            syncParams.TraineeshipLastUpdatedDate = maxDateUpdated > syncParams.TraineeshipLastUpdatedDate ? maxDateUpdated : syncParams.TraineeshipLastUpdatedDate;
            _syncRepository.SetTraineeshipSyncParams(syncParams);
        }
    }
}