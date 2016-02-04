namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Users;

    public interface IAuthenticationRepository : IReadRepository<UserCredentials, Guid>, IWriteRepository<UserCredentials, Guid>
    {
        UserCredentials Get(Guid id, bool errorIfNotFound);
    }
}
