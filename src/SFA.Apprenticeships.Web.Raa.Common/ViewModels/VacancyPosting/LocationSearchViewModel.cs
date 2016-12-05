namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Attributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validators.VacancyPosting;
    using Web.Common.ViewModels;

    [Validator(typeof(LocationSearchViewModelClientValidator))]
    public class LocationSearchViewModel
    {
        public const string PartialView = "Vacancy/LocationSearch";

        public LocationSearchViewModel()
        {
            Addresses = new List<VacancyLocationAddressViewModel>();
        }

        [Display(Name = LocationSearchViewModelMessages.PostCodeSearch.LabelText)]
        public string PostcodeSearch { get; set; }

        public PageableViewModel<VacancyLocationAddressViewModel> SearchResultAddresses { get; set; }

        public List<VacancyLocationAddressViewModel> Addresses { get; set; }

        public string ProviderSiteEdsUrn { get; set; }

        public int ProviderSiteId { get; set; }

        public string EmployerEdsUrn { get; set; }

        public int EmployerId { get; set; }

        public Guid VacancyGuid { get; set; }

        [Display(Name = LocationSearchViewModelMessages.AdditionalLocationInformation.LabelText)]
        public string AdditionalLocationInformation { get; set; }

        public string Ukprn { get; set; }

        public bool ComeFromPreview { get; set; }

        public VacancyStatus Status { get; set; }

        public int VacancyReferenceNumber { get; set; }

        public VacancyLocationType EmployerApprenticeshipLocation { get; set; }

        public int CurrentPage { get; set; }

        public int TotalNumberOfPages { get; set; }

        [Display(Name = LocationSearchViewModelMessages.LocationAddressesComment.LabelText)]
        public string LocationAddressesComment { get; set; }

        [Display(Name = LocationSearchViewModelMessages.AdditionalLocationInformationComment.LabelText)]
        public string AdditionalLocationInformationComment { get; set; }

        public int VacancyOwnerRelationshipId { get; set; }

        public int AutoSaveTimeoutInSeconds { get; set; }
    }
}