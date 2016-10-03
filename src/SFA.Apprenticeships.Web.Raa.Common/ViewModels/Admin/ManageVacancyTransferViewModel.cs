using System.Collections.Generic;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class ManageVacancyTransferViewModel
    {
        public IList<int> VacancyReferenceNumbers { get; set; }

        [Display(Name = "Provider")]
        public string ProviderName { get; set; }
        [Display(Name = "Provider Site")]
        public string ProviderSiteName { get; set; }

        public int ProviderId { get; set; }
        public int ProviderSiteId { get; set; }


    }
}
