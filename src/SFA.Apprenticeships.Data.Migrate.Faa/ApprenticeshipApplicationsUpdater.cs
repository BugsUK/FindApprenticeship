namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using Repository.Sql;

    public class ApprenticeshipApplicationsUpdater : IVacancyApplicationsUpdater
    {
        private readonly SyncRepository _syncRepository;
        
        public ApprenticeshipApplicationsUpdater(SyncRepository syncRepository)
        {
            _syncRepository = syncRepository;
        }

        public string CollectionName => "apprenticeships";
        public bool IsValidForIncrementalSync => _syncRepository.GetSyncParams().IsValidForApprenticeshipIncrementalSync;
        public DateTime VacancyApplicationLastCreatedDate => _syncRepository.GetSyncParams().ApprenticeshipLastCreatedDate;
        public DateTime VacancyApplicationLastUpdatedDate => _syncRepository.GetSyncParams().ApprenticeshipLastUpdatedDate;
        public void UpdateLastCreatedSyncDate(DateTime maxDateCreated)
        {
            var syncParams = _syncRepository.GetSyncParams();
            syncParams.ApprenticeshipLastCreatedDate = maxDateCreated > syncParams.ApprenticeshipLastCreatedDate ? maxDateCreated : syncParams.ApprenticeshipLastCreatedDate;
            _syncRepository.SetApprenticeshipSyncParams(syncParams);
        }
        public void UpdateLastUpdatedSyncDate(DateTime maxDateUpdated)
        {
            var syncParams = _syncRepository.GetSyncParams();
            syncParams.ApprenticeshipLastUpdatedDate = maxDateUpdated > syncParams.ApprenticeshipLastUpdatedDate ? maxDateUpdated : syncParams.ApprenticeshipLastUpdatedDate;
            _syncRepository.SetApprenticeshipSyncParams(syncParams);
        }
    }
}