namespace SFA.Apprenticeships.Domain.Entities.Providers
{
    using Organisations;

    public class ProviderSiteEmployerLink : BaseEntity
    {
        public string ProviderSiteErn { get; set; }
        public string Ern { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public bool IsWebsiteUrlWellFormed { get; set; }
        public Employer Employer { get; set; }
    }
}