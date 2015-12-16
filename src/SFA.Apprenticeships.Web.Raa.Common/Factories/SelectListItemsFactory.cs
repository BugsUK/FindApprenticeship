namespace SFA.Apprenticeships.Web.Raa.Common.Factories
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class SelectListItemsFactory
    {
        public static List<SelectListItem> GetPageSizes(int pageSize)
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Value = "5", Text = "5 per page", Selected = pageSize == 5},
                new SelectListItem {Value = "10", Text = "10 per page", Selected = pageSize == 10},
                new SelectListItem {Value = "25", Text = "25 per page", Selected = pageSize == 25},
                new SelectListItem {Value = "50", Text = "50 per page", Selected = pageSize == 50}
            };
        }
    }
}