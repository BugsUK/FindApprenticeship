namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Users;

    public interface IProviderUserReadRepository
    {
        // TODO: SQL: AG: rename Get methods to be specific.
        ProviderUser Get(int id);

        ProviderUser Get(string username);

        // TODO: SQL: rename to GetByUkprn?
        IEnumerable<ProviderUser> GetForProvider(string ukprn);
    }

    public interface IProviderUserWriteRepository
    {
        ProviderUser Save(ProviderUser providerUser);
    }
}
