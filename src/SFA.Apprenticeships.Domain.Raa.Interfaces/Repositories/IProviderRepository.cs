namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IProviderReadRepository
    {
        Provider Get(int providerId);
        Provider GetViaUkprn(string ukprn);
        IEnumerable<Provider> GetByIds(IEnumerable<int> providerIds);
    }

    public interface IProviderWriteRepository
    {
        void Delete(int providerId);
        Provider Save(Provider entity);
    }
}
