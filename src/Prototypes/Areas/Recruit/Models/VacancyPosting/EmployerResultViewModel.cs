namespace SFA.Apprenticeships.Web.Recruit.ViewModels.VacancyPosting
{
    using Common.ViewModels.Locations;

    public class EmployerResultViewModel
    {
        public int EmployerId { get; set; }

        public string EmployerName { get; set; }

        public AddressViewModel Address { get; set; }

        public string Ern { get; set; }
    }
}