namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Admin
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TransferVacanciesViewModel
    {
        [Display(Name = "Vacancy Reference Numbers", Description = "Enter a single vacancy reference number or a comma separated list of" +
                                                                 "vacancy reference numbers")]
        [RegularExpression(@"^([a-zA-Z0-9, ]+)$", ErrorMessage = "Enter Valid Vacancy Reference Numbers")]
        public string VacancyReferenceNumbers { get; set; }

        public IList<TransferVacanciesViewModel> VacanciesToBeTransferredVm { get; set; }
    }
}