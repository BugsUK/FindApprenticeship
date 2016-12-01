namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.RaaApi
{
    using System;
    using Domain.Entities.Raa.RaaApi;
    using Domain.Raa.Interfaces.Repositories;

    public class RaaApiUserRepository : IRaaApiUserRepository
    {
        public RaaApiUser GetUser(Guid apiKey)
        {
            throw new System.NotImplementedException();
        }
    }
}