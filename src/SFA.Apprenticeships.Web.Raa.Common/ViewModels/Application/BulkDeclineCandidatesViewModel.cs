namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Attributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validators.VacancyStatus;

    [Validator(typeof(BulkDeclineCandidatesViewModelClientValidator))]
    public class BulkDeclineCandidatesViewModel
    {
        public VacancyApplicationsSearchViewModel VacancyApplicationsSearch { get; set; }

        public VacancyType VacancyType { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public int NewApplicationsCount { get; set; }

        public int InProgressApplicationsCount { get; set; }

        public IEnumerable<ApplicationSummaryViewModel> ApplicationSummaries { get; set; }

        public IEnumerable<Guid> SelectedApplicationIds { get; set; }

        [Display(Name = ApplicationViewModelMessages.UnSuccessfulReason.LabelText)]
        public string UnSuccessfulReason { get; set; }

        public bool UnSuccessfulReasonRequired { get; set; }
    }
}