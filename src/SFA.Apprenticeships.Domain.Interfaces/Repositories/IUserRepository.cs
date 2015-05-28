namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Users;

    public interface IUserReadRepository : IReadRepository<User>
    {
        User Get(string username, bool errorIfNotFound = true);

        IEnumerable<Guid> GetUsersWithStatus(UserStatuses[] userStatuses);

        IEnumerable<Guid> GetPotentiallyDormantUsers(DateTime dormantAfterDateTime);
    }

    public interface IUserWriteRepository : IWriteRepository<User> {}
}
