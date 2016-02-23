namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.Parties;

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