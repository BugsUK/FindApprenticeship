namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Users;

    public interface IUserWriteRepository : IWriteRepository<User>
    {
        void SoftDelete(User entity);
    }
}