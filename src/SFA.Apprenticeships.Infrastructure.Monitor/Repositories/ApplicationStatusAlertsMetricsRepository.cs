namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using Domain.Entities.Applications;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Repositories.Communication.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Linq;

    public class ApplicationStatusAlertsMetricsRepository : GenericMongoClient<MongoApplicationStatusAlert>, IApplicationStatusAlertsMetricsRepository
    {
        public ApplicationStatusAlertsMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.MetricsCommunicationsDb, "applicationstatusalerts");
        }

        public int GetApplicationStatusAlertsProcessedToday()
        {
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);

            return Collection
                .AsQueryable()
                .Count(each => each.SentDateTime >= today && each.SentDateTime <= tomorrow);
        }

        public int GetApplicationStatusChangedTo(ApplicationStatuses applicationStatus, DateTime statusChangeStartDate,
            DateTime statusChangeEndDate)
        {
            return Collection
                .AsQueryable()
                .Count(each => each.DateCreated >= statusChangeStartDate && each.DateCreated < statusChangeEndDate && each.Status == applicationStatus);
        }
    }
}
