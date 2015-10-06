namespace SFA.Apprenticeships.Domain.Entities.Organisations
{
    public class Employer : BaseEntity
    {
        public string ProviderSiteErn { get; set; }
        public string Ern { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
    }
}