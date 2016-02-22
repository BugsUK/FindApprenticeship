namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Attributes;
    using Vacancy;
    using Validators.Provider;

    [Validator(typeof (VacancyPartyViewModelValidator))]
    public class VacancyPartyViewModel
    {
        public const string PartialView = "Vacancy/EmployerDetails";

        public int ProviderSiteId { get; set; }

        public string ProviderSiteEdsErn { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.Description.LabelText)]
        public string Description { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.WebsiteUrl.LabelText)]
        public string WebsiteUrl { get; set; }

        public int EmployerId { get; set; }

        public EmployerViewModel Employer { get; set; }

        public Guid VacancyGuid { get; set; }

        public bool? IsEmployerLocationMainApprenticeshipLocation { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.NumberOfPositions.LabelText)]
        public int? NumberOfPositions { get; set; }

        public bool ComeFromPreview { get; set; }

        public VacancyStatus Status { get; set; }

        public long VacancyReferenceNumber { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.DescriptionComment.LabelText)]
        public string DescriptionComment { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.WebsiteUrlComment.LabelText)]
        public string WebsiteUrlComment { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.NumberOfPositionsComment.LabelText)]
        public string NumberOfPositionsComment { get; set; }
    }
}