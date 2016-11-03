using System.ComponentModel.DataAnnotations;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    public class StandardSubjectAreaTierOneViewModel
    {
        [Display(Name = "ID")]
        public int StandardId { get; set; }
        [Display(Name = "SSAT1")]
        public string SSAT1Name { get; set; }
        [Display(Name = "Standard Sector")]
        public string StandardSectorName { get; set; }
        [Display(Name = "Standard")]
        public string StandardName { get; set; }
    }
}