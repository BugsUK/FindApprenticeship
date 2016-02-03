namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Providers;

    public interface IProviderReadRepository : IReadRepository<Provider, Guid>
    {
        Provider Get(string ukprn);
    }

    public interface IProviderWriteRepository : IWriteRepository<Provider, Guid> { }
}