namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Users;

    public interface IUserWriteRepository : IWriteRepository<User>
    {
        Guid SoftDelete(User entity);
    }
}