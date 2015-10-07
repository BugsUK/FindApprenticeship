namespace SFA.Apprenticeships.Web.Recruit.ViewModels.VacancyPosting
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;

    public class EmployerFilterViewModel
    {
        [Display(Name = EmployerFilterViewModelMessages.Ern.LabelText)]
        public string Ern { get; set; }

        [Display(Name = EmployerFilterViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        [Display(Name = EmployerFilterViewModelMessages.Postcode.LabelText)]
        public string Postcode { get; set; }

        public EmployerResultsViewModel EmployerResults { get; set; }
    }
}