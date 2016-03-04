namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Users;

    public interface IProviderUserReadRepository
    {
        ProviderUser GetById(int id);

        ProviderUser GetByUsername(string username);

        IEnumerable<ProviderUser> GetAllByUkprn(string ukprn);
    }

    public interface IProviderUserWriteRepository
    {
        ProviderUser Create(ProviderUser providerUser);

        ProviderUser Update(ProviderUser providerUser);
    }
}
