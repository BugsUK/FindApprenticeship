namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.Entities
{
    using System;
    using Domain.Entities.Raa.Parties;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoProvider : Provider
    {
        [BsonId]
        public Guid Id
        {
            get { return ProviderGuid; }
            set { ProviderGuid = value; }
        }
    }
}