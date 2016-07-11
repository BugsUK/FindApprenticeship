namespace SetApplicationStatus.Console.Entities.Mongo
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class Application
    {
        [BsonId]
        public Guid Id { get; set; }

        public int LegacyApplicationId { get; set; }

        public ApplicationStatus Status { get; set; }

        public DateTime? DateUpdated { get; set; }
    }
}
