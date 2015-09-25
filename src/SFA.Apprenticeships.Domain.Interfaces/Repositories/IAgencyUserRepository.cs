namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Users;

    public interface IAgencyUserReadRepository : IReadRepository<AgencyUser>
    {
        AgencyUser Get(string username);
    }

    public interface IAgencyUserWriteRepository : IWriteRepository<AgencyUser> { }
}