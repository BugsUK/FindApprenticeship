namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Repositories.Mongo.Common;
    using Infrastructure.Repositories.Mongo.Common.Configuration;
    using Infrastructure.Repositories.Mongo.Communication.Entities;
    using MongoDB.Driver.Linq;

    using SFA.Apprenticeships.Application.Interfaces;

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