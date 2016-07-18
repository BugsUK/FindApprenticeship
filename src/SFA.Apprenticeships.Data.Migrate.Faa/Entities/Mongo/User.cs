namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Mongo
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public int Status { get; set; }

        public DateTime? ActivationDate { get; set; }

        public DateTime? LastLogin { get; set; }
    }
}