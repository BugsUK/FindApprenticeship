namespace SFA.Apprenticeships.Infrastructure.Repositories.Audit
{
    using System;
    using Domain.Entities.Audit;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;

    public class AuditRepository<T> : GenericMongoClient<MongoAuditItem<T>>, IAuditReadRepository<T>, IAuditWriteRepository<T>
    {
        public AuditItem<T> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public AuditItem<T> Save(AuditItem<T> entity)
        {
            throw new NotImplementedException();
        }
    }
}