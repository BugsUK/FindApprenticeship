namespace SFA.Apprenticeships.Domain.Entities.Providers
{
    using Organisations;

    //TODO: Perhaps rename to Relationship?
    public class ProviderSiteEmployerLink : BaseEntity
    {
        public string ProviderSiteErn { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public Employer Employer { get; set; }
    }
}