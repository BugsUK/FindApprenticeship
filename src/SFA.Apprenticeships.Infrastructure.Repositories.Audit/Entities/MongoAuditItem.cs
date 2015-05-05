namespace SFA.Apprenticeships.Infrastructure.Repositories.Audit.Entities
{
    using System;
    using Domain.Entities.Audit;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoAuditItem<T> : AuditItem<T>
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}
