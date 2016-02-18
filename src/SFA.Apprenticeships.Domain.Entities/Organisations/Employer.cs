namespace SFA.Apprenticeships.Domain.Entities.Organisations
{
    using System;
    using Locations;

    public class Employer : BaseEntity<int>
    {
        public string Ern { get; set; }
        public string Name { get; set; }
        public PostalAddress Address { get; set; }
    }
}