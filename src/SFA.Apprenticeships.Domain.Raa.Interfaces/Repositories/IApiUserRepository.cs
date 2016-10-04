namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Raa.Api;
    using Models;

    public interface IApiUserRepository
    {
        IEnumerable<ApiUser> SearchApiUsers(ApiUserSearchParameters searchParameters);
        ApiUser GetApiUser(Guid externalSystemId);
        ApiUser GetApiUser(string companyId);
        ApiUser Create(ApiUser apiUser);
        ApiUser Update(ApiUser apiUser);
    }
}