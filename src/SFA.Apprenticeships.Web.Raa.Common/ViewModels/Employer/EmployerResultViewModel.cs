namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Employer
{
    using Web.Common.ViewModels.Locations;

    public class EmployerResultViewModel
    {
        public int EmployerId { get; set; }

        public string EdsUrn { get; set; }

        public string EmployerName { get; set; }

        public AddressViewModel Address { get; set; }
    }
}