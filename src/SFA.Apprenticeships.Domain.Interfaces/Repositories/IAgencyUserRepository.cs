namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Users;

    public interface IAgencyUserReadRepository
    {
        AgencyUser Get(string username);
    }

    public interface IAgencyUserWriteRepository
    {
        AgencyUser Save(AgencyUser entity);
    }
}