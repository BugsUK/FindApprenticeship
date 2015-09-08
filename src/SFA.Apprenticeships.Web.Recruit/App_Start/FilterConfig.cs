using System.Web;
using System.Web.Mvc;

namespace SFA.Apprenticeships.Web.Recruit
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
