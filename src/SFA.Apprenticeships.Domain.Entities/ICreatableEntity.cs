using System;

namespace SFA.Apprenticeships.Domain.Entities
{
    public interface ICreatableEntity
    {
        DateTime CreatedDateTime { get; set; }
    }
}
