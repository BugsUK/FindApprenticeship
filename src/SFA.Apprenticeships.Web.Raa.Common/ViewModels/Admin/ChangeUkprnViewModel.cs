namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeUkprnViewModel
    {
        [Display(Name = "New UKPRN")]
        public string Ukprn { get; set; }
    }
}