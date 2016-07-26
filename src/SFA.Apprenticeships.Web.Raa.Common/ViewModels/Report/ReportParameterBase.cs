namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Report
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Web.Common.ViewModels;

    public abstract class ReportParameterBase
    {
        [Display(Name = ReportParametersMessages.FromDateMessages.LabelText)]
        public virtual DateViewModel FromDate { get; set; }

        [Display(Name = ReportParametersMessages.ToDateMessages.LabelText)]
        public virtual DateViewModel ToDate { get; set; }

        public virtual bool IsValid { get; set; }
    }
}