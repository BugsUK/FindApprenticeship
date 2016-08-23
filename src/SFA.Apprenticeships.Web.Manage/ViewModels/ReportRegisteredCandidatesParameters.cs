
namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.UI.WebControls;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.Report;

    public class ReportRegisteredCandidatesParameters : ReportParameterBase
    {
        public string Type { get; set; }

        public List<ListItem> TypeList => new List<ListItem>()
        {
            new ListItem("All", "-1"),
            new ListItem("Region", "1"),
            new ListItem("Local Authority", "2"),
            //new ListItem("Postcode", "3")
        };

        public List<ListItem> RegionList { get; set; }

        public string Region { get; set; }

        public List<ListItem> AgeRangeList => new List<ListItem>()
        {
            new ListItem("All", "-1"),
            new ListItem("Up to 16", "1"),
            new ListItem("16 - 18", "2"),
            new ListItem("Up to 20", "3"),
            new ListItem("19 -24", "4"),
            new ListItem("25 +", "5")
        };

        [Display(Name = ReportParametersMessages.AgeRange.LabelText)]
        public string AgeRange { get; set; }

        public List<ListItem> LocalAuthoritiesList { get; set; }

        [Display(Name = ReportParametersMessages.LocalAuthority.LabelText)]
        public string LocalAuthority { get; set; }

        [Display(Name = ReportParametersMessages.MarketMessagesOnly.LabelText)]
        public bool MarketMessagesOnly { get; set; }

        [Display(Name = ReportParametersMessages.IncludeCandidateIds.LabelText)]
        public bool IncludeCandidateIds { get; set; }
    }
}