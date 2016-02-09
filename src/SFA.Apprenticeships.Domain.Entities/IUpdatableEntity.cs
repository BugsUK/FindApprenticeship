using System;

namespace SFA.Apprenticeships.Domain.Entities
{
    public interface IUpdatableEntity
    {
        DateTime? DateUpdated { get; set; }
    }
}
