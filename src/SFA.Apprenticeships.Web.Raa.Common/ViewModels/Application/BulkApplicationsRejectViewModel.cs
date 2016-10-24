namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using Constants.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BulkApplicationsRejectViewModel
    {
        public IList<BulkRejectApplicationViewModel> BulkRejectApplications { get; set; }
        public int VacancyReferenceNumber { get; set; }
        public string VacancyTitle { get; set; }
        public IEnumerable<Guid> ApplicationIds { get; set; }

        [Display(Name = ApplicationViewModelMessages.UnSuccessfulReason.LabelText)]
        [Required(ErrorMessage = ApplicationViewModelMessages.UnSuccessfulReason.BulkRequiredErrorText)]
        public string UnSuccessfulReason { get; set; }
        public string ConfirmationStatusSentMessage { get; set; }
    }
}