﻿namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Vacancies;

    public class SubmittedVacancyViewModel
    {
        public int VacancyReferenceNumber { get; set; }
        public int ProviderSiteId { get; set; }
        public bool Resubmitted { get; set; }
        public string VacancyText { get; set; }
        public bool IsMultiLocationVacancy { get; set; }
        public VacancyType VacancyType { get; set; }
    }
}