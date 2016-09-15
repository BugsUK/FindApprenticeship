namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Candidate
{
    using System;
    using Web.Common.ViewModels.Locations;

    public class CandidateSummaryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressViewModel Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ApplicantId { get; set; }
    }
}