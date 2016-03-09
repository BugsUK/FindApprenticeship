namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IProviderReadRepository
    {
        Provider GetById(int providerId);
        Provider GetByUkprn(string ukprn);
        IEnumerable<Provider> GetByIds(IEnumerable<int> providerIds);
    }

    public interface IProviderWriteRepository
    {
        Provider Update(Provider entity);
    }
}
