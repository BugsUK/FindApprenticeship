namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Communication;

    public interface IContactMessageRepository : IWriteRepository<ContactMessage, Guid>
    {
    }
}
