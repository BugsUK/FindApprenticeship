namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System;
    using Locations;

    public class Employer : ICreatableEntity, IUpdatableEntity
    {
        public int EmployerId { get; set; }
        public string EdsErn { get; set; }
        public string Name { get; set; }
        public PostalAddress Address { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}