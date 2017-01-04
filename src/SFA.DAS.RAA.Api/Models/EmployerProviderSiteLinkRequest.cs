namespace SFA.DAS.RAA.Api.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Specifies which provider site to link an employer to
    /// </summary>
    public class EmployerProviderSiteLinkRequest
    {
        /// <summary>
        /// The employer's secondary identifier.
        /// </summary>
        [JsonIgnore]
        public int? EmployerEdsUrn { get; set; }
        /// <summary>
        /// The provider site's secondary identifier. You must supply this or the provider site's ID.
        /// The Provider associated with your API key must also have a relationship with the employer.
        /// </summary>
        //[Required(ErrorMessage = EmployerProviderSiteLinkMessages.MissingProviderSiteIdentifier)]
        public int? ProviderSiteEdsUrn { get; set; }
        /// <summary>
        /// The description of the employer for this link (required)
        /// </summary>
        //[Required(ErrorMessage = EmployerProviderSiteLinkMessages.EmployerDescription.RequiredErrorText)]
        public string EmployerDescription { get; set; }
        /// <summary>
        /// The employer's website for this link (optional)
        /// </summary>
        public string EmployerWebsiteUrl { get; set; }

        protected bool Equals(EmployerProviderSiteLinkRequest other)
        {
            return string.Equals(EmployerDescription, other.EmployerDescription) && EmployerEdsUrn == other.EmployerEdsUrn && string.Equals(EmployerWebsiteUrl, other.EmployerWebsiteUrl) && ProviderSiteEdsUrn == other.ProviderSiteEdsUrn;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmployerProviderSiteLinkRequest) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (EmployerDescription != null ? EmployerDescription.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ EmployerEdsUrn.GetHashCode();
                hashCode = (hashCode*397) ^ (EmployerWebsiteUrl != null ? EmployerWebsiteUrl.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ ProviderSiteEdsUrn.GetHashCode();
                return hashCode;
            }
        }
    }
}