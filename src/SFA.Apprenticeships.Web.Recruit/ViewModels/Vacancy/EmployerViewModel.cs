namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using Common.ViewModels.Locations;

    public class EmployerViewModel
    {
        public string Ern { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public AddressViewModel Address { get; set; }
    }
}