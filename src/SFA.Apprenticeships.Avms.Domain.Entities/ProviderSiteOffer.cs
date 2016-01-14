namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProviderSiteOffer")]
    public partial class ProviderSiteOffer
    {
        public int ProviderSiteOfferID { get; set; }

        public int ProviderSiteLocalAuthorityID { get; set; }

        public int ProviderSiteFrameworkID { get; set; }

        public bool Apprenticeship { get; set; }

        public bool AdvancedApprenticeship { get; set; }

        public bool HigherApprenticeship { get; set; }

        public virtual ProviderSiteFramework ProviderSiteFramework { get; set; }

        public virtual ProviderSiteLocalAuthority ProviderSiteLocalAuthority { get; set; }
    }
}
