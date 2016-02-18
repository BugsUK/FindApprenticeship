namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.Entities
{
    using System;
    using Domain.Entities.Providers;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoProviderSite : ProviderSite
    {
        [BsonId]
        public int Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}