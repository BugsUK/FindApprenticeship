namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Web.Common.ViewModels.Locations;

    public class EmployerViewModel
    {
        public string Ern { get; set; }
        public string Name { get; set; }
        public AddressViewModel Address { get; set; }
    }
}