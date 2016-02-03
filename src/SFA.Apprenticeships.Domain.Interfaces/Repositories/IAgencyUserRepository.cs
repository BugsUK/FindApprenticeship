namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Users;

    public interface IAgencyUserReadRepository : IReadRepository<AgencyUser, Guid>
    {
        AgencyUser Get(string username);
    }

    public interface IAgencyUserWriteRepository : IWriteRepository<AgencyUser, Guid> { }
}