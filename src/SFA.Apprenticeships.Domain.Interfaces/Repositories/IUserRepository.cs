namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Users;

    public interface IUserReadRepository : IReadRepository<User>
    {
        User Get(string username, bool errorIfNotFound = true);
        IEnumerable<User> GetUsersWithStatus(UserStatuses[] userStatuses);
    }

    public interface IUserWriteRepository : IWriteRepository<User> {}
}
