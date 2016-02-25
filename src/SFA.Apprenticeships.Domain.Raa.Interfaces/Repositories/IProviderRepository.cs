namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Raa.Parties;
    using Entities.Raa.Users;

    public interface IProviderReadRepository
    {
        Provider Get(int providerId);
        Provider GetViaUkprn(string ukprn);
    }

    public interface IProviderWriteRepository
    {
        void Delete(int providerId);
        Provider Save(Provider entity);
    }
}
