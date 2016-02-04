namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities;

    public interface IWriteRepository<T, TKey> where T : BaseEntity<TKey>
    {
        void Delete(TKey id);
        T Save(T entity);
    }
}
