namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.UserProfiles.Entities
{
    using System;
    using Domain.Entities.Raa.Users;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoAgencyUser : AgencyUser
    {
        [BsonId]
        public Guid Id
        {
            get { return AgencyUserGuid; }
            set { AgencyUserGuid = value; }
        }
    }
}