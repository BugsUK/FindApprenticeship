using SFA.Apprenticeships.Web.Common.ViewModels.Locations;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    public class EmployerResultViewModel
    {
        public int EmployerId { get; set; }

        public string EmployerName { get; set; }

        public AddressViewModel Address { get; set; }

        public string Ern { get; set; }
    }
}