namespace SFA.Apprenticeships.Domain.Entities.Providers
{
    using System;
    using System.Collections.Generic;
    using Organisations;

    //TODO: Perhaps rename to Relationship?
    public class ProviderSiteEmployerLink
    {
        public int EntityId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string ProviderSiteErn { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public Employer Employer { get; set; }
    }

    //TODO: Discuss where these should go
    public class ProviderSiteEmployerLinkEqualityComparer : IEqualityComparer<ProviderSiteEmployerLink>
    {
        public bool Equals(ProviderSiteEmployerLink x, ProviderSiteEmployerLink y)
        {
            if (x == null && y == null) return true;
            if (x == null) return false;
            if (y == null) return false;
            if (x.Employer == null && y.Employer == null) return true;
            if (x.Employer == null) return false;
            if (y.Employer == null) return false;
            return x.Employer.Ern == y.Employer.Ern;
        }

        public int GetHashCode(ProviderSiteEmployerLink obj)
        {
            return obj.Employer == null ? 0 : obj.Employer.Ern.GetHashCode();
        }
    }
}