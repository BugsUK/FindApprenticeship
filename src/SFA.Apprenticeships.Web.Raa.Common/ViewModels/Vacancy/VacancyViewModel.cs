namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Provider;
    using Validators.Vacancy;
    using System.ComponentModel.DataAnnotations;
    using VacancyPosting;

    [Validator(typeof(VacancyResubmissionValidator))]
    public class VacancyViewModel
    {
        public long VacancyReferenceNumber { get; set; }

        public NewVacancyViewModel NewVacancyViewModel { get; set; }

        public VacancySummaryViewModel VacancySummaryViewModel { get; set; }

        public VacancyRequirementsProspectsViewModel VacancyRequirementsProspectsViewModel { get; set; }

        public VacancyQuestionsViewModel VacancyQuestionsViewModel { get; set; }

        public List<SelectListItem> ApprenticeshipLevels { get; set; }

        public string FrameworkName { get; set; }

        public string StandardName { get; set; }

        public ProviderSiteViewModel ProviderSite { get; set; }

        public ProviderVacancyStatuses Status { get; set; }

        [Display(Name = VacancyViewModelMessages.ResubmitOptin.LabelText)]
        public bool ResubmitOption { get; set; }

        public List<VacancyLocationAddressViewModel> LocationAddresses { get; set; }

        public bool IsEmployerLocationMainApprenticeshipLocation { get; set; }

        public int NumberOfPositions { get; set; }
    }
}