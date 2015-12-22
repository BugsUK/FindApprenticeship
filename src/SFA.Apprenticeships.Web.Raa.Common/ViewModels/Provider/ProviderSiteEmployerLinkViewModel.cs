namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentValidation.Attributes;
    using Vacancy;
    using Validators.Provider;

    [Validator(typeof (ProviderSiteEmployerLinkViewModelValidator))]
    public class ProviderSiteEmployerLinkViewModel
    {
        public string ProviderSiteErn { get; set; }

        [Display(Name = ProviderSiteEmployerLinkViewModelMessages.Description.LabelText)]
        public string Description { get; set; }

        [Display(Name = ProviderSiteEmployerLinkViewModelMessages.WebsiteUrl.LabelText)]
        public string WebsiteUrl { get; set; }

        public EmployerViewModel Employer { get; set; }

        public Guid VacancyGuid { get; set; }

        public bool? IsEmployerLocationMainApprenticeshipLocation { get; set; }

        [Display(Name = ProviderSiteEmployerLinkViewModelMessages.NumberOfPositions.LabelText)]
        public int? NumberOfPositions { get; set; }

        public bool ComeFromPreview { get; set; }

        public ProviderVacancyStatuses Status { get; set; }

        public long VacancyReferenceNumber { get; set; }

        public string DescriptionComment { get; set; }

        public string WebsiteUrlComment { get; set; }

        public string NumberOfPositionsComment { get; set; }
    }
}