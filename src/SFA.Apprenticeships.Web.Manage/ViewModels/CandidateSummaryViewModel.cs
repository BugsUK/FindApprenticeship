namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System;
    using Common.ViewModels.Locations;

    public class CandidateSummaryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressViewModel Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}