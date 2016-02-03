namespace SFA.Apprenticeships.Domain.Entities
{
    using System;

    public class ReferenceNumber : BaseEntity<Guid>
    {
        public int LastReferenceNumber { get; set; }
    }
}
