namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Communication.Entities;
    using Mongo.Common;
    using MongoDB.Driver.Linq;

    public class ContactMessagesMetricsRepository : GenericMongoClient<MongoContactMessage>, IContactMessagesMetricsRepository
    {
        public ContactMessagesMetricsRepository(IConfigurationManager configurationManager)
            : base(configurationManager, "Communications.mongoDB", "contactmessages")
        {
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