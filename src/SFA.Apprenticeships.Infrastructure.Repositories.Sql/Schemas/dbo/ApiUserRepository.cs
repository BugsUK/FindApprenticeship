namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Api;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;

    public class ApiUserRepository : IApiUserRepository
    {
        public IEnumerable<ApiUser> SearchApiUsers(ApiUserSearchParameters searchParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}