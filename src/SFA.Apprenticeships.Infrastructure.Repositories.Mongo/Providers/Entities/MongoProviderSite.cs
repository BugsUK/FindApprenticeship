namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.Entities
{
    using System;
    using Domain.Entities.Raa.Parties;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoProviderSite : ProviderSite
    {
        [BsonId]
        public Guid Id
        {
            get { return ProviderSiteGuid; }
            set { ProviderSiteGuid = value; }
        }
    }
}