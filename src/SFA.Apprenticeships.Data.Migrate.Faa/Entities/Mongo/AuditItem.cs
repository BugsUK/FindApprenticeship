namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Mongo
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    public class AuditItem
    {
        [BsonId]
        public Guid Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public Guid PrimaryEntityId { get; set; }
        public Guid? SecondaryEntityId { get; set; }
        public object Data { get; set; }
    }
}