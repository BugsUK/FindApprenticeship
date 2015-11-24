namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    using System;
    using System.Collections.Generic;

    public class LocationSearchViewModel
    {
        public string PostcodeSearch { get; set; } 

        public List<VacancyLocationAddressViewModel> SearchResultAddresses { get; set; }
        public List<VacancyLocationAddressViewModel> Addresses { get; set; }
        public string ProviderSiteErn { get; set; }
        public string Ern { get; set; }
        public Guid VacancyGuid { get; set; }
    }
}