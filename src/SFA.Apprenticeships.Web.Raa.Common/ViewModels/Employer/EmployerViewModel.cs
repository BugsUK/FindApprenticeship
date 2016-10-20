namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Employer
{
    using Domain.Entities.Raa.Parties;
    using Web.Common.ViewModels.Locations;

    public class EmployerViewModel
    {
        public int EmployerId { get; set; }
        public string EdsUrn { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }
        public AddressViewModel Address { get; set; }
        public EmployerTrainingProviderStatuses Status { get; set; }
    }
}