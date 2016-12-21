namespace SFA.DAS.RAA.Api.Models
{
    /// <summary>
    /// Specifies which provider site to link an employer to
    /// </summary>
    public class EmployerLink
    {
        public int? ProviderSiteId { get; set; }
        public int? ProviderSiteEdsUrn { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsite { get; set; }
    }
}