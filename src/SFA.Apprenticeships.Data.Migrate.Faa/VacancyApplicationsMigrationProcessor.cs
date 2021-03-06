﻿namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Application.Interfaces;
    using Entities;
    using Entities.Mongo;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using MongoDB.Driver;
    using Repository.Mongo;
    using Repository.Sql;

    public class VacancyApplicationsMigrationProcessor : IMigrationProcessor
    {
        private readonly IVacancyApplicationsUpdater _vacancyApplicationsUpdater;
        private readonly IApplicationMappers _applicationMappers;
        private readonly IGenericSyncRespository _genericSyncRespository;
        private readonly IGetOpenConnection _targetDatabase;
        private readonly ILogService _logService;
        
        private readonly VacancyRepository _vacancyRepository;
        private readonly CandidateRepository _candidateRepository;
        private readonly ApplicationRepository _sourceApplicationRepository;
        private readonly ApplicationHistoryRepository _sourceApplicationHistoryRepository;
        private readonly SubVacancyRepository _sourceSubVacancyRepository;
        private readonly ApplicationRepository _destinationApplicationRepository;
        private readonly ApplicationHistoryRepository _destinationApplicationHistoryRepository;
        private readonly SchoolAttendedRepository _schoolAttendedRepository;
        private readonly SubVacancyRepository _destinationSubVacancyRepository;
        private readonly VacancyApplicationsRepository _vacancyApplicationsRepository;
        private readonly UpdateVacancyApplicationsRepository _updateVacancyApplicationsRepository;

        private readonly ITableSpec _applicationTable = new ApplicationTable();
        private readonly ITableSpec _applicationHistoryTable = new ApplicationHistoryTable();
        private readonly ITableSpec _schoolsAttendedTable = new SchoolAttendedTable();
        private readonly ITableSpec _subVacanciesTable = new SubVacancyTable();

        public VacancyApplicationsMigrationProcessor(IVacancyApplicationsUpdater vacancyApplicationsUpdater, IApplicationMappers applicationMappers, IGenericSyncRespository genericSyncRespository, IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase, IConfigurationService configurationService, ILogService logService)
        {
            _vacancyApplicationsUpdater = vacancyApplicationsUpdater;
            _applicationMappers = applicationMappers;
            _genericSyncRespository = genericSyncRespository;
            _targetDatabase = targetDatabase;
            _logService = logService;

            _vacancyRepository = new VacancyRepository(targetDatabase);
            _candidateRepository = new CandidateRepository(targetDatabase);
            _sourceApplicationRepository = new ApplicationRepository(sourceDatabase);
            _sourceApplicationHistoryRepository = new ApplicationHistoryRepository(sourceDatabase, _logService);
            _sourceSubVacancyRepository = new SubVacancyRepository(sourceDatabase);
            _destinationApplicationRepository = new ApplicationRepository(targetDatabase);
            _destinationApplicationHistoryRepository = new ApplicationHistoryRepository(targetDatabase, _logService);
            _schoolAttendedRepository = new SchoolAttendedRepository(targetDatabase);
            _destinationSubVacancyRepository = new SubVacancyRepository(targetDatabase);
            _vacancyApplicationsRepository = new VacancyApplicationsRepository(_vacancyApplicationsUpdater.CollectionName, configurationService, logService);
            _updateVacancyApplicationsRepository = new UpdateVacancyApplicationsRepository(_vacancyApplicationsUpdater.CollectionName, configurationService, logService);
        }

        public void Process(CancellationToken cancellationToken)
        {
            if (_vacancyApplicationsUpdater.IsValidForIncrementalSync)
            {
                ExecuteIncrementalSync(cancellationToken);
            }
            else
            {
                ExecuteFullSync(cancellationToken);
            }
        }

        private void ExecuteFullSync(CancellationToken cancellationToken)
        {
            _logService.Warn($"ExecuteFullSync on {_vacancyApplicationsUpdater.CollectionName} collection with LastCreatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate} LastUpdatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate}");

            _logService.Info("Loading Vacancy Ids");
            var vacancyIds = _vacancyRepository.GetAllVacancyIds();
            _logService.Info($"Completed loading {vacancyIds.Count} Vacancy Ids");

            _logService.Info("Loading candidates");
            var candidateIds = _candidateRepository.GetAllCandidateIds();
            _logService.Info($"Completed loading {candidateIds.Count} candidates");

            var expectedCount = _vacancyApplicationsRepository.GetVacancyApplicationsCount(cancellationToken).Result;
            var cursor = _vacancyApplicationsRepository.GetAllVacancyApplications(cancellationToken).Result;
            ProcessApplications(cursor, expectedCount, vacancyIds, candidateIds, SyncType.Full, cancellationToken);
        }

        private void ExecuteIncrementalSync(CancellationToken cancellationToken)
        {
            _logService.Info($"ExecutePartialSync on {_vacancyApplicationsUpdater.CollectionName} collection with LastCreatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate} LastUpdatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate}");

            _logService.Info("Loading Vacancy Ids");
            var vacancyIds = _vacancyRepository.GetAllVacancyIds();
            _logService.Info($"Completed loading {vacancyIds.Count} Vacancy Ids");

            var candidates = new Dictionary<Guid, int>();

            //Inserts
            _logService.Info($"Processing new {_vacancyApplicationsUpdater.CollectionName}");
            var expectedCreatedCount = _vacancyApplicationsRepository.GetVacancyApplicationsCreatedSinceCount(_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate, cancellationToken).Result;
            var createdCursor = _vacancyApplicationsRepository.GetAllVacancyApplicationsCreatedSince(_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate, cancellationToken).Result;
            ProcessApplications(createdCursor, expectedCreatedCount, vacancyIds, candidates, SyncType.PartialByDateCreated, cancellationToken);
            _logService.Info($"Completed processing new {_vacancyApplicationsUpdater.CollectionName}");

            //Updates
            _logService.Info($"Processing updated {_vacancyApplicationsUpdater.CollectionName}");
            var expectedUpdatedCount = _vacancyApplicationsRepository.GetVacancyApplicationsUpdatedSinceCount(_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate, cancellationToken).Result;
            var updatedCursor = _vacancyApplicationsRepository.GetAllVacancyApplicationsUpdatedSince(_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate, cancellationToken).Result;
            ProcessApplications(updatedCursor, expectedUpdatedCount, vacancyIds, candidates, SyncType.PartialByDateUpdated, cancellationToken);
            _logService.Info($"Completed processing updated {_vacancyApplicationsUpdater.CollectionName}");
        }

        public void BulkUpsert(IList<ApplicationWithHistory> applicationsWithHistory, IDictionary<Guid, int> applicationIds)
        {
            //Bulk insert any applications with valid ids that are not already in the database
            _genericSyncRespository.BulkInsert(_applicationTable, applicationsWithHistory.Where(a => a.ApplicationWithSubVacancy.Application.ApplicationId != 0 && !applicationIds.ContainsKey(a.ApplicationWithSubVacancy.Application.ApplicationGuid)).Select(a => _applicationMappers.MapApplicationDictionary(a.ApplicationWithSubVacancy.Application)));

            //Now insert any remaining applications one at a time
            foreach (var applicationWithHistory in applicationsWithHistory.Where(a => a.ApplicationWithSubVacancy.Application.ApplicationId == 0))
            {
                //Ensure schools attended and application histories have the correct application id
                var applicationId = (int)_targetDatabase.Insert(applicationWithHistory.ApplicationWithSubVacancy.Application);
                if (applicationWithHistory.ApplicationWithSubVacancy.SchoolAttended != null)
                {
                    applicationWithHistory.ApplicationWithSubVacancy.SchoolAttended.ApplicationId = applicationId;
                }
                foreach (var applicationHistory in applicationWithHistory.ApplicationHistory)
                {
                    applicationHistory.ApplicationId = applicationId;
                }
            }

            //Finally, update existing applications
            _genericSyncRespository.BulkUpdate(_applicationTable, applicationsWithHistory.Where(a => a.ApplicationWithSubVacancy.Application.ApplicationId != 0 && applicationIds.ContainsKey(a.ApplicationWithSubVacancy.Application.ApplicationGuid)).Select(a => _applicationMappers.MapApplicationDictionary(a.ApplicationWithSubVacancy.Application)));

            //Insert new application history records
            var newApplicationHistories = applicationsWithHistory.SelectMany(a => a.ApplicationHistory).Where(a => a.ApplicationHistoryId == 0);
            _genericSyncRespository.BulkInsert(_applicationHistoryTable, newApplicationHistories.Select(ah => ah.MapApplicationHistoryDictionary()));

            //Update existing application history records
            var existingApplicationHistories = applicationsWithHistory.SelectMany(a => a.ApplicationHistory).Where(a => a.ApplicationHistoryId != 0);
            _genericSyncRespository.BulkUpdate(_applicationHistoryTable, existingApplicationHistories.Select(ah => ah.MapApplicationHistoryDictionary()));

            //Insert new schools attended
            var newSchoolsAttended = applicationsWithHistory.Where(a => a.ApplicationWithSubVacancy.SchoolAttended != null && a.ApplicationWithSubVacancy.SchoolAttended.SchoolAttendedId == 0).Select(a => a.ApplicationWithSubVacancy.SchoolAttended);
            _genericSyncRespository.BulkInsert(_schoolsAttendedTable, newSchoolsAttended.Select(sa => sa.MapSchoolAttendedDictionary()));

            //Update existing schools attended
            var existingSchoolsAttended = applicationsWithHistory.Where(a => a.ApplicationWithSubVacancy.SchoolAttended != null && a.ApplicationWithSubVacancy.SchoolAttended.SchoolAttendedId != 0).Select(a => a.ApplicationWithSubVacancy.SchoolAttended);
            _genericSyncRespository.BulkUpdate(_schoolsAttendedTable, existingSchoolsAttended.Select(sa => sa.MapSchoolAttendedDictionary()));

            var subVacancies = applicationsWithHistory.Where(a => a.ApplicationWithSubVacancy.SubVacancy != null).Select(a => a.ApplicationWithSubVacancy.SubVacancy).ToList();
            var existingSubVacancies = _destinationSubVacancyRepository.GetApplicationSummariesByIds(subVacancies.Select(sv => sv.AllocatedApplicationId));
            
            //Insert new sub vacancies
            _genericSyncRespository.BulkInsert(_subVacanciesTable, subVacancies.Where(sv => !existingSubVacancies.ContainsKey(sv.AllocatedApplicationId)).Select(sv => _applicationMappers.MapSubVacancyDictionary(sv)));

            //Update existing sub vacancies
            _genericSyncRespository.BulkUpdate(_subVacanciesTable, subVacancies.Where(sv => existingSubVacancies.ContainsKey(sv.AllocatedApplicationId)).Select(sv => _applicationMappers.MapSubVacancyDictionary(sv)));

            //Patch in any changes to the applications in Mongo based on what was discovered through processing
            foreach (var applicationWithSubVacancy in applicationsWithHistory.Select(a => a.ApplicationWithSubVacancy))
            {
                if (applicationWithSubVacancy.UpdateNotes)
                {
                    _updateVacancyApplicationsRepository.UpdateApplicationNotes(applicationWithSubVacancy.Application.ApplicationGuid, applicationWithSubVacancy.Application.AllocatedTo);
                }
                if (applicationWithSubVacancy.UpdateStatusTo.HasValue)
                {
                    _updateVacancyApplicationsRepository.UpdateApplicationStatus(applicationWithSubVacancy.Application.ApplicationGuid, applicationWithSubVacancy.UpdateStatusTo.Value);
                }
            }
        }

        private void ProcessApplications(IAsyncCursor<VacancyApplication> cursor, long expectedCount, HashSet<int> vacancyIds, IDictionary<Guid, int> candidateIds, SyncType syncType, CancellationToken cancellationToken)
        {
            var count = 0;
            while (cursor.MoveNextAsync(cancellationToken).Result && !cancellationToken.IsCancellationRequested)
            {
                var batch = cursor.Current.ToList();
                if (batch.Count == 0) continue;
                _logService.Info($"Processing {batch.Count} {_vacancyApplicationsUpdater.CollectionName}");

                var maxDateCreated = batch.Max(a => a.DateCreated);
                var maxDateUpdated = batch.Max(a => a.DateUpdated) ?? DateTime.MinValue;

                LoadCandidates(candidateIds, batch);
                var destinationCandidateIds = batch.Where(a => candidateIds.ContainsKey(a.CandidateId)).Select(a => candidateIds[a.CandidateId]).ToArray();
                batch = batch.Where(a => a.Vacancy != null && vacancyIds.Contains(a.Vacancy.Id) && candidateIds.ContainsKey(a.CandidateId)).ToList();

                var destinationApplicationIds = _destinationApplicationRepository.GetApplicationIdsByGuid(batch.Select(a => a.Id));
                var candidateApplicationIds = _destinationApplicationRepository.GetApplicationIdsByCandidateIds(destinationCandidateIds);
                var applicationIds = _applicationMappers.GetApplicationIds(destinationApplicationIds, candidateApplicationIds, batch, candidateIds);

                var legacyApplicationIds = batch.Where(a => a.LegacyApplicationId != 0).Select(a => a.LegacyApplicationId).ToArray();
                var sourceApplicationSummaries = _sourceApplicationRepository.GetApplicationSummariesByIds(legacyApplicationIds);
                var applicationHistoryIds = _destinationApplicationHistoryRepository.GetApplicationHistoryIdsByApplicationIds(applicationIds.Values);
                var sourceApplicationHistorySummaries = _sourceApplicationHistoryRepository.GetApplicationHistorySummariesByApplicationIds(legacyApplicationIds);
                var schoolAttendedIds = _schoolAttendedRepository.GetSchoolAttendedIdsByApplicationIds(applicationIds.Values);
                var subVacancies = _sourceSubVacancyRepository.GetApplicationSummariesByIds(legacyApplicationIds);
                var applicationsWithHistory = batch.Select(a => _applicationMappers.MapApplicationWithHistory(a, candidateIds[a.CandidateId], applicationIds, sourceApplicationSummaries, schoolAttendedIds, subVacancies, applicationHistoryIds, sourceApplicationHistorySummaries)).ToList();

                count += applicationsWithHistory.Count;
                _logService.Info($"Processing {applicationsWithHistory.Count} {_vacancyApplicationsUpdater.CollectionName}");
                BulkUpsert(applicationsWithHistory, applicationIds);

                if (syncType == SyncType.Full)
                {
                    _vacancyApplicationsUpdater.UpdateLastCreatedSyncDate(maxDateCreated);
                    //Deliberate as date updated could skip some updates post full sync
                    _vacancyApplicationsUpdater.UpdateLastUpdatedSyncDate(maxDateCreated);
                }
                if (syncType == SyncType.PartialByDateCreated)
                {
                    _vacancyApplicationsUpdater.UpdateLastCreatedSyncDate(maxDateCreated);
                }
                if (syncType == SyncType.PartialByDateUpdated)
                {
                    _vacancyApplicationsUpdater.UpdateLastUpdatedSyncDate(maxDateUpdated);
                }

                var percentage = ((double)count / expectedCount) * 100;
                _logService.Info($"Processed batch of {applicationsWithHistory.Count} {_vacancyApplicationsUpdater.CollectionName} and {count} {_vacancyApplicationsUpdater.CollectionName} out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete. LastCreatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate} LastUpdatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate}");
            }
        }

        private void LoadCandidates(IDictionary<Guid, int> candidateIds, IEnumerable<VacancyApplication> vacancyApplications)
        {
            var newCandidateIds = vacancyApplications.Select(a => a.CandidateId).Except(candidateIds.Keys).ToArray();
            if (newCandidateIds.Any())
            {
                _logService.Info($"Loading {newCandidateIds.Length} candidates");
                foreach (var candidateIdKvp in _candidateRepository.GetCandidateIdsByGuid(newCandidateIds))
                {
                    candidateIds.Add(candidateIdKvp);
                }
                _logService.Info($"Completed loading batch of {newCandidateIds.Length} candidates");
            }
        }
    }
}