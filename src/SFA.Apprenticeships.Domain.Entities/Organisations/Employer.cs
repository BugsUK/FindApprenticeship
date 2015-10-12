namespace SFA.Apprenticeships.Domain.Entities.Organisations
{
    using Locations;

    public class Employer : BaseEntity
    {
        public string ProviderSiteErn { get; set; }
        public string Ern { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public bool IsWebsiteUrlWellFormed { get; set; }
        public Address Address { get; set; }
    }
}