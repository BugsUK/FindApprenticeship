namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Users;

    public interface IProviderUserReadRepository : IReadRepository<ProviderUser>
    {
        ProviderUser Get(string username);
    }

    public interface IProviderUserWriteRepository : IWriteRepository<ProviderUser> {}
}
