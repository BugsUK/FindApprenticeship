namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Web.Common.ViewModels;

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

        public string ProviderSiteEdsErn { get; set; }

        public int ProviderSiteId { get; set; }

        public string EmployerErn { get; set; }

        public int EmployerId { get; set; }

        public Guid VacancyGuid { get; set; }

        [Display(Name= LocationSearchViewModelMessages.AdditionalLocationInformation.LabelText)]
        public string AdditionalLocationInformation { get; set; }

        public string Ukprn { get; set; }

        public bool ComeFromPreview { get; set; }

        public VacancyStatus Status { get; set; }

        public long VacancyReferenceNumber { get; set; }

        public bool IsEmployerLocationMainApprenticeshipLocation { get; set; }
		
        public int CurrentPage { get; set; }

        public int TotalNumberOfPages { get; set; }

        [Display(Name = LocationSearchViewModelMessages.LocationAddressesComment.LabelText)]
        public string LocationAddressesComment { get; set; }

        [Display(Name = LocationSearchViewModelMessages.AdditionalLocationInformationComment.LabelText)]
        public string AdditionalLocationInformationComment { get; set; }
    }
}