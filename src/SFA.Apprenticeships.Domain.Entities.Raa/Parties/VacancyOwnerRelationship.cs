namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System;

    public class VacancyOwnerRelationship
    {
        public int VacancyOwnerRelationshipId { get; set; }
        public Guid VacancyOwnerRelationshipGuid { get; set; }
        public int ProviderSiteId { get; set; }
        public int EmployerId { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsiteUrl { get; set; }

        public string AnonymousEmployerDescription { get; set; }

        public string AnonymousEmployerDescriptionComment { get; set; }

        public string AnonymousEmployerReason { get; set; }

        public string AnonymousEmployerReasonComment { get; set; }

        public bool? IsAnonymousEmployer { get; set; }

        public string OriginalFullName { get; set; }

        public string AnonymousAboutTheEmployer { get; set; }

        public string AnonymousAboutTheEmployerComment { get; set; }
    }
}
