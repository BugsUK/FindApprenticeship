namespace SFA.Apprenticeships.Web.Common.Extensions
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public static class ListItemExtensions
    {
        public static List<ListItem> ToListOfListItem(this Dictionary<string, string> source)
        {
            return source.Select(x => new ListItem(x.Key, x.Value)).ToList();
        }
    }
}
