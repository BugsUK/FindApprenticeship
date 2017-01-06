namespace SFA.DAS.RAA.Api.Models
{
    /// <summary>
    /// Specifies which provider site to link an employer to
    /// </summary>
    public class EmployerProviderSiteLink
    {
        /// <summary>
        /// The primary identifier for the link from an employer to a provider site
        /// </summary>
        public int EmployerProviderSiteLinkId { get; set; }
        /// <summary>
        /// The employer's primary identifier.
        /// </summary>
        public int EmployerId { get; set; }
        /// <summary>
        /// The employer's secondary identifier.
        /// </summary>
        public int EmployerEdsUrn { get; set; }
        /// <summary>
        /// The provider site's primary identifier. You must supply this or the provider site's EDSURN.
        /// The Provider associated with your API key must also have a relationship with the employer.
        /// </summary>
        public int ProviderSiteId { get; set; }
        /// <summary>
        /// The provider site's secondary identifier. You must supply this or the provider site's ID.
        /// The Provider associated with your API key must also have a relationship with the employer.
        /// </summary>
        public int ProviderSiteEdsUrn { get; set; }
        /// <summary>
        /// The description of the employer for this link (required)
        /// </summary>
        public string EmployerDescription { get; set; }
        /// <summary>
        /// The employer's website for this link (optional)
        /// </summary>
        public string EmployerWebsiteUrl { get; set; }

        protected bool Equals(EmployerProviderSiteLink other)
        {
            return EmployerProviderSiteLinkId == other.EmployerProviderSiteLinkId && EmployerId == other.EmployerId && EmployerEdsUrn == other.EmployerEdsUrn && ProviderSiteId == other.ProviderSiteId && ProviderSiteEdsUrn == other.ProviderSiteEdsUrn && string.Equals(EmployerDescription, other.EmployerDescription) && string.Equals(EmployerWebsiteUrl, other.EmployerWebsiteUrl);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmployerProviderSiteLink) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EmployerProviderSiteLinkId;
                hashCode = (hashCode*397) ^ EmployerId;
                hashCode = (hashCode*397) ^ EmployerEdsUrn;
                hashCode = (hashCode*397) ^ ProviderSiteId;
                hashCode = (hashCode*397) ^ ProviderSiteEdsUrn;
                hashCode = (hashCode*397) ^ (EmployerDescription != null ? EmployerDescription.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (EmployerWebsiteUrl != null ? EmployerWebsiteUrl.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}