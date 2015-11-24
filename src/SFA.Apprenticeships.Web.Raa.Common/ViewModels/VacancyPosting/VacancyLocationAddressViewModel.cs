namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    using Web.Common.ViewModels.Locations;

    public class VacancyLocationAddressViewModel
    {
        public VacancyLocationAddressViewModel()
        {
            Address = new AddressViewModel();
            NumberOfPositions = 1;
        }

        public AddressViewModel Address { get; set; } 

        public int NumberOfPositions { get; set; }
    }
}