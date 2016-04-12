namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Mongo
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        public Guid Id { get; set; }

        public int Status { get; set; }
    }
}