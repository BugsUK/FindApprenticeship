namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using FluentValidation.Attributes;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Provider;
    using Validators.Vacancy;

    [Validator(typeof(VacancyViewModelValidator))]
    public class VacancyViewModel
    {
        public long VacancyReferenceNumber { get; set; }

        public NewVacancyViewModel NewVacancyViewModel { get; set; }

        public VacancySummaryViewModel VacancySummaryViewModel { get; set; }

        public VacancyRequirementsProspectsViewModel VacancyRequirementsProspectsViewModel { get; set; }

        public VacancyQuestionsViewModel VacancyQuestionsViewModel { get; set; }

        public string FrameworkName { get; set; }

        public ProviderSiteViewModel ProviderSite { get; set; }

        public ProviderVacancyStatuses Status { get; set; }
    }
}