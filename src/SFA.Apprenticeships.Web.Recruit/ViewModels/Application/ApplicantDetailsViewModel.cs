namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Application
{
    using System;
    using Common.ViewModels.Locations;
    using Domain.Entities.Candidates;

    public class ApplicantDetailsViewModel
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public AddressViewModel Address { get; set; }
        public double Distance { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DisabilityStatus? DisabilityStatus { get; set; }
    }
}