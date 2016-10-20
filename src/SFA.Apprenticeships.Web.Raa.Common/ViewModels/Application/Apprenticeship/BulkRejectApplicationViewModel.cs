namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application.Apprenticeship
{
    using Constants.ViewModels;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BulkRejectApplicationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ApplicationId { get; set; }
    }

    public class BulkApplicationsRejectViewModel
    {
        public IList<BulkRejectApplicationViewModel> BulkRejectApplications { get; set; }
        public int VacancyReferenceNumber { get; set; }
        public string VacancyTitle { get; set; }
        public IList<string> ApplicationIds { get; set; }

        [Display(Name = ApplicationViewModelMessages.UnSuccessfulReason.LabelText)]
        [Required(ErrorMessage = ApplicationViewModelMessages.UnSuccessfulReason.BulkRequiredErrorText)]
        public string UnSuccessfulReason { get; set; }
    }
}