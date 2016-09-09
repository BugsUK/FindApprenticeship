namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeUkprnViewModel
    {
        [Display(Name = "New UKPRN")]
        public string Ukprn { get; set; }
    }
}