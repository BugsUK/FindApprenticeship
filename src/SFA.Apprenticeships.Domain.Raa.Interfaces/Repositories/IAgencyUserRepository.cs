namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.Users;

    public interface IAgencyUserReadRepository
    {
        AgencyUser GetByUsername(string username);
    }

    public interface IAgencyUserWriteRepository
    {
        AgencyUser Save(AgencyUser entity);
    }
}