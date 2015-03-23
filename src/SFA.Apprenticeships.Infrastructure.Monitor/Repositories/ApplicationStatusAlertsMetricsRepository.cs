namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Communication.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Linq;

    public class ApplicationStatusAlertsMetricsRepository : GenericMongoClient<MongoApplicationStatusAlert>, IApplicationStatusAlertsMetricsRepository
    {
        public ApplicationStatusAlertsMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>(MongoConfiguration.MongoConfigurationName);
            Initialise(config.CommunicationsDb, "applicationstatusalerts");
        }

        public int GetApplicationStatusAlertsProcessedToday()
        {
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);

            return Collection
                .AsQueryable()
                .Count(each => each.SentDateTime >= today && each.SentDateTime <= tomorrow);
        }
    }
}
