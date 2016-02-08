namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Users;

    public interface IAgencyUserReadRepository
    {
        AgencyUser Get(int id);

        AgencyUser Get(string username);
    }

    public interface IAgencyUserWriteRepository
    {
        void Delete(int id);

        AgencyUser Save(AgencyUser entity);
    }
}