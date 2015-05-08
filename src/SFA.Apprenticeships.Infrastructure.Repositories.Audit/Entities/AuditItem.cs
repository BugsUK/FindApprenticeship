namespace SFA.Apprenticeships.Infrastructure.Repositories.Audit.Entities
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    public class AuditItem<T>
    {
        [BsonId]
        public Guid Id { get; set; }
        public DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public Guid PrimaryEntityId { get; set; }
        public Guid? SecondaryEntityId { get; set; }
        public T Data { get; set; }
    }
}