namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Repositories.Communication.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Linq;

    public class ContactMessagesMetricsRepository : GenericMongoClient<MongoContactMessage>, IContactMessagesMetricsRepository
    {
        public ContactMessagesMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.MetricsCommunicationsDb, "contactmessages");
        }

        public int GetContactMessagesSentToday()
        {
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);

            return Collection
                .AsQueryable()
                .Count(each => each.DateCreated >= today && each.DateCreated <= tomorrow);
        }
    }
}