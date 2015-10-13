namespace SFA.Apprenticeships.Domain.Entities.Organisations
{
    using Locations;

    public class Employer : BaseEntity
    {
        public string Ern { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }
}