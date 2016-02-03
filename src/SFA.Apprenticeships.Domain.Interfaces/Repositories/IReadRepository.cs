namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities;

    public interface IReadRepository<T,TKey> where T : BaseEntity<TKey>
    {
        T Get(TKey id);
    }
}
