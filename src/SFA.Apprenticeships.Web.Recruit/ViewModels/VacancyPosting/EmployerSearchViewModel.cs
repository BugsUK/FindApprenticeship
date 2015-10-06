namespace SFA.Apprenticeships.Web.Recruit.ViewModels.VacancyPosting
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;

    public enum EmployerFilterType
    {
        Ern,
        NameAndLocation
    }

    public class EmployerSearchViewModel
    {
        public EmployerFilterType FilterType { get; set; }

        [Display(Name = EmployerSearchViewModelMessages.Ern.LabelText)]
        public string Ern { get; set; }

        [Display(Name = EmployerSearchViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        [Display(Name = EmployerSearchViewModelMessages.Location.LabelText, Description = EmployerSearchViewModelMessages.Location.LabelText)]
        public string Location { get; set; }
    }
}