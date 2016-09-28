namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ShareApplicationsViewModel
    {
        public ShareApplicationsViewModel()
        {
            ApplicationSummaries = new List<ApplicationSummaryViewModel>();
            SelectedApplicationIds = new List<Guid>();
        }

        public int VacancyReferenceNumber { get; set; }

        public string EmployerName { get; set; }

        public string ProviderName { get; set; }

        public VacancyType VacancyType { get; set; }

        public List<ApplicationSummaryViewModel> ApplicationSummaries { get; set; }

        public IEnumerable<Guid> SelectedApplicationIds { get; set; }

        public int NewApplicationsCount { get; set; }

        public int InProgressApplicationsCount { get; set; }

        public int SuccessfulApplicationsCount { get; set; }

        public int UnsuccessfulApplicationsCount { get; set; }

        [Display(Name = ShareApplicationsViewModelMessages.EmailAddressMessages.LabelText)]
        public string RecipientEmailAddress { get; set; }
    }
}