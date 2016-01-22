namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Communication.Entities
{
    using System;
    using Domain.Entities.Communication;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoApplicationStatusAlert : ApplicationStatusAlert
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        } 
    }
}