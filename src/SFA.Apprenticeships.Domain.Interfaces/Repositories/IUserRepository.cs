namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Users;

    public interface IUserReadRepository : IReadRepository<User, Guid>
    {
        User Get(string username, bool errorIfNotFound = true);

        IEnumerable<Guid> GetUsersWithStatus(UserStatuses[] userStatuses);

        IEnumerable<Guid> GetPotentiallyDormantUsers(DateTime lastValidLogin);

        IEnumerable<Guid> GetDormantUsersPotentiallyEligibleForSoftDelete(DateTime dormantAfterDateTime);
    }

    public interface IUserWriteRepository : IWriteRepository<User, Guid> {}
}
