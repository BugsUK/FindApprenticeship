namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.Users;

    public interface IAgencyUserReadRepository
    {
        AgencyUser Get(string username);
    }

    public interface IAgencyUserWriteRepository
    {
        AgencyUser Save(AgencyUser entity);
    }
}