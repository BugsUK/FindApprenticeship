namespace SFA.Apprenticeships.Infrastructure.Repositories.Providers.Entities
{
    using System;
    using Domain.Entities.Providers;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoProviderSiteEmployerLink : ProviderSiteEmployerLink
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}