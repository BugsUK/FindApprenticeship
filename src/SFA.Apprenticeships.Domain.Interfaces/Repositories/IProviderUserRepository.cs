namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Users;

    public interface IProviderUserReadRepository : IReadRepository<ProviderUser>
    {
        ProviderUser Get(string username);

        IEnumerable<ProviderUser> GetForProvider(string ukprn);
    }

    public interface IProviderUserWriteRepository : IWriteRepository<ProviderUser>
    {        
    }
}
