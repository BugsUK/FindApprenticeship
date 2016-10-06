namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class TransferVacanciesViewModel
    {
        [Display(Name = "Vacancy Reference Numbers",
            Description = "Enter a single vacancy reference number or a comma separated list of " +
                          "vacancy reference numbers")]
        [RegularExpression(@"^([a-zA-Z0-9, ]+)$", ErrorMessage = "Enter Valid Vacancy Reference Numbers")]
        [Required]
        public string VacancyReferenceNumbers { get; set; }
    }
}