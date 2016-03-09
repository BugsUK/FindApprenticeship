namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;

    public class EmployerFilterViewModel
    {
        public string ProviderSiteErn { get; set; }

        [Display(Name = EmployerFilterViewModelMessages.EdsUrn.LabelText)]
        public string EdsUrn { get; set; }

        [Display(Name = EmployerFilterViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        [Display(Name = EmployerFilterViewModelMessages.Postcode.LabelText)]
        public string Postcode { get; set; }

        public IEnumerable<EmployerResultViewModel> EmployerResults { get; set; }
    }
}