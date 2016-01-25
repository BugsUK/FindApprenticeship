namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Repositories.Mongo.Common;
    using Infrastructure.Repositories.Mongo.Common.Configuration;
    using Infrastructure.Repositories.Mongo.Communication.Entities;
    using MongoDB.Driver.Linq;

    public class SavedSearchAlertMetricsRepository : GenericMongoClient<MongoSavedSearchAlert>, ISavedSearchAlertMetricsRepository
    {
        public SavedSearchAlertMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.MetricsCommunicationsDb, "savedsearchalerts");
        }

        public int GetSavedSearchAlertsProcessedToday()
        {
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);

            return Collection
                .AsQueryable()
                .Count(each => each.SentDateTime >= today && each.SentDateTime <= tomorrow);
        }
    }
}