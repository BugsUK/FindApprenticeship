namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using Raa.Common.ViewModels.Report;

    public class ReportUnsuccessfulCandidatesParameters : ReportParameterBase
    {
        public string Type { get; set; }

        public string AgeRange { get; set; }

        public string ManagedBy { get; set; }

        public string Region { get; set; }

        public List<ListItem> TypeList => new List<ListItem>()
        {
            new ListItem("All", "-1"),
            new ListItem("Region", "1"),
            //new ListItem("LocalAuthority", "2"),
            //new ListItem("Postcode", "3")
        };

        public List<ListItem> ManagedByList { get; set; }

        public List<ListItem> RegionList { get; set; }

        public List<ListItem> AgeRangeList => new List<ListItem>()
        {
            new ListItem("All", "-1"),
            new ListItem("Up to 16", "1"),
            new ListItem("16 - 18", "2"),
            new ListItem("Up to 20", "3"),
            new ListItem("19 -24", "4"),
            new ListItem("25 +", "5")
        };
    }
}