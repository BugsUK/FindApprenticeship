namespace SFA.Apprenticeships.Domain.Entities.Raa
{
    using System;

    public interface IUpdatableEntity
    {
        DateTime? UpdatedDateTime { get; set; }
    }
}
