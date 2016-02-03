namespace SFA.Apprenticeships.Domain.Entities
{
    using System;

    /// <summary>
    /// Base type for persistent domain entities
    /// </summary>
    public abstract class BaseEntity<TKey>
    {
        public TKey EntityId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }
    }
}
