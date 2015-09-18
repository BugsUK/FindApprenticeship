namespace SFA.Apprenticeships.Infrastructure.Repositories.UserProfiles.Entities
{
    using System;
    using Domain.Entities.Users;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoProviderUser : ProviderUser
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}