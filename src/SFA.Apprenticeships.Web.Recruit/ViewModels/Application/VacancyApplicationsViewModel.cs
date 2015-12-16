﻿namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Application
{
    using System;
    using Common.ViewModels;
    using Common.ViewModels.Locations;
    using Domain.Entities.Vacancies.ProviderVacancies;

    public class VacancyApplicationsViewModel
    {
        public VacancyApplicationsSearchViewModel VacancyApplicationsSearch { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public GeoPointViewModel EmployerGeoPoint { get; set; }

        public string ShortDescription { get; set; }

        public DateTime ClosingDate { get; set; }

        public ProviderVacancyStatuses Status { get; set; }

        public int RejectedApplicationsCount { get; set; }

        public int UnresolvedApplicationsCount { get; set; }

        public PageableViewModel<ApplicationSummaryViewModel> ApplicationSummaries { get; set; }
    }
}