namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Repositories.Communication.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Linq;

    public class ExpiringDraftsMetricsRepository : GenericMongoClient<MongoApprenticeshipApplicationExpiringDraft>, IExpiringDraftsMetricsRepository
    {
        public ExpiringDraftsMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.MetricsCommunicationsDb, "expiringdraftapplications");
        }

        public int GetDraftApplicationsProcessedToday()
        {
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);

            return Collection
                .AsQueryable()
                .Count(each => each.SentDateTime >= today && each.SentDateTime <= tomorrow);
        }
    }
}
