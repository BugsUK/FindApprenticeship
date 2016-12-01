namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System;
    using Entities.Raa.RaaApi;

    public interface IRaaApiUserRepository
    {
        RaaApiUser GetUser(Guid apiKey);
    }
}