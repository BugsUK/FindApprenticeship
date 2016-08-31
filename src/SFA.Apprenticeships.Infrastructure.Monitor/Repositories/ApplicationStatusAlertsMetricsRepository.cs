namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using Domain.Entities.Applications;
    using Infrastructure.Repositories.Mongo.Common;
    using Infrastructure.Repositories.Mongo.Common.Configuration;
    using Infrastructure.Repositories.Mongo.Communication.Entities;
    using SFA.Infrastructure.Interfaces;
    using MongoDB.Driver.Linq;

    using SFA.Apprenticeships.Application.Interfaces;

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
