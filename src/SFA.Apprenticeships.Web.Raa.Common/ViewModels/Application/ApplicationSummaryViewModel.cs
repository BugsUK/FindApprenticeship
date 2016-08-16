namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using System;
    using Domain.Entities.Applications;

    public class ApplicationSummaryViewModel
    {
        public Guid ApplicationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantID { get; set; }
        public string Notes { get; set; }
        public DateTime DateApplied { get; set; }
        public ApplicationStatuses Status { get; set; }
        public string AnonymousLinkData { get; set; }
    }
}