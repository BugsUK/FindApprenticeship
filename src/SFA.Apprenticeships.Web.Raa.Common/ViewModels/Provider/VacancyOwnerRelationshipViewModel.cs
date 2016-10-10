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

    [Validator(typeof (VacancyOwnerRelationshipViewModelValidator))]
    public class VacancyOwnerRelationshipViewModel
    {
        public const string PartialView = "Vacancy/EmployerDetails";

        public int VacancyOwnerRelationshipId { get; set; }

        public int ProviderSiteId { get; set; }

        [AllowHtml]
        [Display(Name = VacancyOwnerRelationshipViewModelMessages.EmployerDescription.LabelText)]
        public string EmployerDescription { get; set; }

        [Display(Name = VacancyOwnerRelationshipViewModelMessages.EmployerWebsiteUrl.LabelText)]
        public string EmployerWebsiteUrl { get; set; }

        public EmployerViewModel Employer { get; set; }

        public Guid VacancyGuid { get; set; }

        public bool? IsEmployerLocationMainApprenticeshipLocation { get; set; }

        [Display(Name = VacancyOwnerRelationshipViewModelMessages.NumberOfPositions.LabelText)]
        public int? NumberOfPositions { get; set; }

        public bool ComeFromPreview { get; set; }

        public VacancyStatus Status { get; set; }

        public int VacancyReferenceNumber { get; set; }

        [Display(Name = VacancyOwnerRelationshipViewModelMessages.EmployerDescriptionComment.LabelText)]
        public string EmployerDescriptionComment { get; set; }

        [Display(Name = VacancyOwnerRelationshipViewModelMessages.EmployerWebsiteUrlComment.LabelText)]
        public string EmployerWebsiteUrlComment { get; set; }

        [Display(Name = VacancyOwnerRelationshipViewModelMessages.NumberOfPositionsComment.LabelText)]
        public string NumberOfPositionsComment { get; set; }

        public bool IsEmployerAddressValid { get; set; }
    }
}