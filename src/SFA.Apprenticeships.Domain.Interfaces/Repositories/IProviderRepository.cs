namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Providers;

    public interface IProviderReadRepository : IReadRepository<Provider>
    {
        Provider Get(string ukprn);
    }

    public interface IProviderWriteRepository : IWriteRepository<Provider> { }
}