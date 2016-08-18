namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Mongo
{
    using System;
    using Entities.Mongo;
    using Infrastructure.Repositories.Mongo.Common.Configuration;
    using MongoDB.Driver;
    using SFA.Infrastructure.Interfaces;

    public class UpdateVacancyApplicationsRepository
    {
        private readonly string _collectionName;
        private readonly ILogService _logService;
        private readonly IMongoDatabase _database;

        public UpdateVacancyApplicationsRepository(string collectionName, IConfigurationService configurationService, ILogService logService)
        {
            _collectionName = collectionName;
            _logService = logService;
            var connectionString = configurationService.Get<MongoConfiguration>().ApplicationsDb;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes)
        {
            _logService.Info($"Updating notes for {applicationId} to {notes}");

            var result = _database.GetCollection<VacancyApplication>(_collectionName).UpdateOne(
                Builders<VacancyApplication>.Filter.Eq(e => e.Id, applicationId),
                Builders<VacancyApplication>.Update.Set(e => e.Notes, notes));

            if (result.IsAcknowledged)
            {
                if (result.IsModifiedCountAvailable && result.ModifiedCount == 1)
                {
                    _logService.Info($"Updated notes for {applicationId} to {notes}");
                }
                else
                {
                    _logService.Warn($"Did not update notes for {applicationId} to {notes} as application wasn't found");
                }
            }
            else
            {
                var message = $"Updating notes for {applicationId} to {notes} failed! {result}";
                _logService.Error(message);
                throw new Exception(message);
            }
        }

        public void UpdateApplicationStatus(Guid applicationId, ApplicationStatuses updatedStatus)
        {
            _logService.Info($"Updating status for {applicationId} to {updatedStatus}");

            var result = _database.GetCollection<VacancyApplication>(_collectionName).UpdateOne(
                Builders<VacancyApplication>.Filter.Eq(e => e.Id, applicationId),
                Builders<VacancyApplication>.Update.Set(e => e.Status, updatedStatus));

            if (result.IsAcknowledged)
            {
                if (result.IsModifiedCountAvailable && result.ModifiedCount == 1)
                {
                    _logService.Info($"Updated status for {applicationId} to {updatedStatus}");
                }
                else
                {
                    _logService.Warn($"Did not update status for {applicationId} to {updatedStatus} as application wasn't found");
                }
            }
            else
            {
                var message = $"Updating status for {applicationId} to {updatedStatus} failed! {result}";
                _logService.Error(message);
                throw new Exception(message);
            }
        }
    }
}