namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Application
{
    using System;
    using Common.ViewModels.Locations;
    using Domain.Entities.Applications;

    public class ApplicationSummaryViewModel
    {
        public Guid ApplicationId { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantLocation { get; set; }
        public GeoPointViewModel ApplicantGeoPoint { get; set; }
        public double Distance { get; set; }
        public DateTime DateApplied { get; set; }
        public ApplicationStatuses Status { get; set; }
    }
}