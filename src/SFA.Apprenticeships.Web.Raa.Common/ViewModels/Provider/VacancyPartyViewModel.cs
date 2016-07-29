namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Attributes;
    using Vacancy;
    using Validators.Provider;

    [Validator(typeof (VacancyPartyViewModelValidator))]
    public class VacancyPartyViewModel
    {
        public const string PartialView = "Vacancy/EmployerDetails";

        public int VacancyPartyId { get; set; }

        public int ProviderSiteId { get; set; }

        [AllowHtml]
        [Display(Name = VacancyPartyViewModelMessages.EmployerDescription.LabelText)]
        public string EmployerDescription { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.EmployerWebsiteUrl.LabelText)]
        public string EmployerWebsiteUrl { get; set; }

        public EmployerViewModel Employer { get; set; }

        public Guid VacancyGuid { get; set; }

        public bool? IsEmployerLocationMainApprenticeshipLocation { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.NumberOfPositions.LabelText)]
        public int? NumberOfPositions { get; set; }

        public bool ComeFromPreview { get; set; }

        public VacancyStatus Status { get; set; }

        public int VacancyReferenceNumber { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.EmployerDescriptionComment.LabelText)]
        public string EmployerDescriptionComment { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.EmployerWebsiteUrlComment.LabelText)]
        public string EmployerWebsiteUrlComment { get; set; }

        [Display(Name = VacancyPartyViewModelMessages.NumberOfPositionsComment.LabelText)]
        public string NumberOfPositionsComment { get; set; }

        public bool IsEmployerAddressValid { get; set; }
    }
}