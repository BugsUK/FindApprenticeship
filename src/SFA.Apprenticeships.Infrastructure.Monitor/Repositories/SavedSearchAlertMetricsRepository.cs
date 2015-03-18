namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Communication.Entities;
    using Mongo.Common;
    using MongoDB.Driver.Linq;

    public class SavedSearchAlertMetricsRepository : GenericMongoClient<MongoSavedSearchAlert>, ISavedSearchAlertMetricsRepository
    {
        public SavedSearchAlertMetricsRepository(IConfigurationManager configurationManager)
            : base(configurationManager, "Communications.mongoDB", "savedsearchalerts")
        {
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