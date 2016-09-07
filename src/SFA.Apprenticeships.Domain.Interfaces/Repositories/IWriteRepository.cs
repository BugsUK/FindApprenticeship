namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities;
    using System;

    public interface IWriteRepository<T> where T : BaseEntity
    {
        void Delete(Guid id);
        T Save(T entity);
    }
}
