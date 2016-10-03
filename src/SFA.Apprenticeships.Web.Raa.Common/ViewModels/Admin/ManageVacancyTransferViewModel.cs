using System.Collections.Generic;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Admin
{
    using System.ComponentModel.DataAnnotations;

    public class ManageVacancyTransferViewModel
    {
        public IList<int> VacancyReferenceNumbers { get; set; }

        [Display(Name = "ProviderId")]
        public int ProviderId { get; set; }
        [Display(Name = "ProviderSiteId")]
        public int ProviderSiteId { get; set; }
    }
}
