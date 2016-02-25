namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.UserProfiles.Entities
{
    using System;
    using Domain.Entities.Raa.Users;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoProviderUser : ProviderUser
    {
        [BsonId]
        public Guid Id
        {
            get { return ProviderUserGuid; }
            set { ProviderUserGuid = value; }
        }
    }
}