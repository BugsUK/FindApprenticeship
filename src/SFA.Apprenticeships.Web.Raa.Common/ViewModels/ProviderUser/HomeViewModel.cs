using System.Collections.Generic;
using System.Web.Mvc;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    using Candidate;
    using Provider;

    public class HomeViewModel
    {
        public ProviderUserViewModel ProviderUserViewModel { get; set; }

        public ProviderViewModel ProviderViewModel { get; set; }

        public List<SelectListItem> ProviderSites { get; set; }

        public CandidateSearchViewModel CandidateSearch { get; set; }

        public VacanciesSummaryViewModel VacanciesSummary { get; set; }
    }
}