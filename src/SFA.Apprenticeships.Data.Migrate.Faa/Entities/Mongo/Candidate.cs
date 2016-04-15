namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Mongo
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class Candidate
    {
        [BsonId]
        public Guid Id { get; set; }

        public int LegacyCandidateId { get; set; }
    }
}