namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.RaaApi;

    public interface IRaaApiUserRepository
    {
        RaaApiUser GetUser(string apiKey);
    }
}