namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Employers.Entities
{
    using System;
    using Domain.Entities.Organisations;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoEmployer : Employer
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}