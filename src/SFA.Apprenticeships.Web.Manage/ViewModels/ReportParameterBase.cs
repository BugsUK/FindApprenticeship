namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Common.ViewModels;

    public abstract class ReportParameterBase
    {
        [Required(ErrorMessage = "To date is required")]
        public virtual DateViewModel ToDate { get; set; }

        [Required(ErrorMessage = "From date is required")]
        public virtual DateViewModel FromDate { get; set; }
    }
}