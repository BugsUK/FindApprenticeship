namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System;
    using Locations;

    public class Employer
    {
        public int EmployerId { get; set; }
        public Guid EmployerGuid { get; set; }
        public string EdsUrn { get; set; }
        public string Name { get; set; }
        public PostalAddress Address { get; set; }
    }
}