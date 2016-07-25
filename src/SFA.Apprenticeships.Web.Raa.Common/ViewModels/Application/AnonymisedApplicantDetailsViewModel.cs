namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using System;
    using Domain.Entities.Candidates;
    using Web.Common.ViewModels.Locations;

    public class AnonymisedApplicantDetailsViewModel
    {
        public const string PartialView = "Application/AnonymisedApplicantDetails";

        public Guid ApplicantId { get; set; }
    }
}