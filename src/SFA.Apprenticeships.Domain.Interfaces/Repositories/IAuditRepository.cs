using System;

namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    public interface IAuditRepository
    {
        void Audit(object data, string eventType, Guid primaryEntityId, Guid? secondaryEntityId = null);
    }
}