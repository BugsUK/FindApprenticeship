namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using System;
    using Domain.Entities.Candidates;
    using Web.Common.ViewModels.Locations;

    public class ApplicantDetailsViewModel
    {
        public const string PartialView = "Application/ApplicantDetails";

        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public AddressViewModel Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DisabilityStatus? DisabilityStatus { get; set; }
    }
}