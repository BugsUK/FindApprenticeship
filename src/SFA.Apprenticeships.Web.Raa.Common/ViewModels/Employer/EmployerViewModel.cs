namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Employer
{
    using Web.Common.ViewModels.Locations;

    public class EmployerViewModel
    {
        public int EmployerId { get; set; }
        public string EdsUrn { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }
        public AddressViewModel Address { get; set; }
    }
}