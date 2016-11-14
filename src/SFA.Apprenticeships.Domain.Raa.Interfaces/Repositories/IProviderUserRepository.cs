namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Users;
    using Models;

    public interface IProviderUserReadRepository
    {
        ProviderUser GetById(int id);

        ProviderUser GetByUsername(string username);

        IEnumerable<ProviderUser> GetAllByUkprn(string ukprn);

        ProviderUser GetByEmail(string email);

        IEnumerable<ProviderUser> Search(ProviderUserSearchParameters searchParameters);
    }

    public interface IProviderUserWriteRepository
    {
        ProviderUser Create(ProviderUser providerUser);

        ProviderUser Update(ProviderUser providerUser);
    }
}
