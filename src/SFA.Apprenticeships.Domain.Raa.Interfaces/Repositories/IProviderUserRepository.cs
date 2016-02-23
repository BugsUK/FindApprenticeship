namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Users;

    public interface IProviderUserReadRepository
    {
        ProviderUser Get(int providerUserId);

        ProviderUser Get(string username);

        IEnumerable<ProviderUser> GetForProvider(string ukprn);
    }

    public interface IProviderUserWriteRepository
    {
        void Delete(int providerUserId);

        ProviderUser Save(ProviderUser entity);
    }
}
