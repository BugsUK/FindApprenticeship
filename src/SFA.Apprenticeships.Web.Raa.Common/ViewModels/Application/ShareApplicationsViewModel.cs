namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using Web.Common.ViewModels;
    using Web.Common.ViewModels.Locations;

    public class ShareApplicationsViewModel
    {
        public int VacancyReferenceNumber { get; set; }

        public string EmployerName { get; set; }

        public VacancyType VacancyType { get; set; }

        public List<ApplicationSummaryViewModel> ApplicationSummaries { get; set; }

        public int NewApplicationsCount { get; set; }

        public int ViewedApplicationsCount { get; set; }

        public int SuccessfulApplicationsCount { get; set; }

        public int UnsuccessfulApplicationsCount { get; set; }

        public string RecipientEmailAddress { get; set; }
    }
}