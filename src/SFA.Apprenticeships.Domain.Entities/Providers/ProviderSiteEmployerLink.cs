namespace SFA.Apprenticeships.Domain.Entities.Providers
{
    public class ProviderSiteEmployerLink : BaseEntity
    {
        public string ProviderSiteErn { get; set; }
        public string Ern { get; set; }
        public string Description { get; set; }
    }
}