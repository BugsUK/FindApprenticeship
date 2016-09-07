namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using Entities.Users;
    using System;
    using System.Collections.Generic;

    public interface IUserReadRepository : IReadRepository<User>
    {
        User Get(string username, bool errorIfNotFound = true);

        IEnumerable<Guid> GetUsersWithStatus(UserStatuses[] userStatuses);

        IEnumerable<Guid> GetPotentiallyDormantUsers(DateTime lastValidLogin);

        IEnumerable<Guid> GetDormantUsersPotentiallyEligibleForSoftDelete(DateTime dormantAfterDateTime);
    }
}
