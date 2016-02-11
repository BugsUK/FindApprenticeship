namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Providers;

    public interface IProviderReadRepository
    {
        Provider Get(int providerId);

        Provider Get(string ukprn);
    }

    public interface IProviderWriteRepository
    {
        Provider Save(Provider provider);

        void Delete(int providerId);
    }
}
