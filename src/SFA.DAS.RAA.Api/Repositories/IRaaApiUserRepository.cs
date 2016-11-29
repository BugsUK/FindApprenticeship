namespace SFA.DAS.RAA.Api.Repositories
{
    using Entities;

    public interface IRaaApiUserRepository
    {
        RaaApiUser GetUser(string apiKey);
    }
}