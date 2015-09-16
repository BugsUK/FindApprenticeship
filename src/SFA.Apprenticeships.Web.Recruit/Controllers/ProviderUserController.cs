namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Attributes;
    using Common.Controllers;
    using Constants;
    using Providers;
    using ViewModels;
    using ViewModels.ProviderUser;

    public class ProviderUserController : ControllerBase<RecuitmentUserContext>
    {
        [AuthorizeUser(Roles = Roles.VerifiedEmail)]
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.VerifiedEmail)]
        public ActionResult Settings()
        {
            var sites = new List<SelectListItem>();
            sites.Add(new SelectListItem { Value = "1", Text = "Basing View, Basingstoke", Selected = true} );
            sites.Add(new SelectListItem { Value = "2", Text = "Great Charles Street Queensway, Birmingham" });
            sites.Add(new SelectListItem { Value = "3", Text = "St. Helens Street, Ipswich" });
            sites.Add(new SelectListItem { Value = "4", Text = "South Parade, Leeds" });
            sites.Add(new SelectListItem { Value = "5", Text = "24-26 Baltic Street West, London" });
            sites.Add(new SelectListItem { Value = "6", Text = "Dean Street, Newcastle upon Tyne" });
            sites.Add(new SelectListItem { Value = "7", Text = "Judson Road, Peterlee" });
            sites.Add(new SelectListItem { Value = "8", Text = "David Murray John Tower, Swindon" });
            sites.Add(new SelectListItem { Value = "9", Text = "6-10 Gills Yard, Wakefield" });
            sites.Add(new SelectListItem { Value = "10", Text = "Sheep Street, Wellingborough" });
            sites.Add(new SelectListItem { Value = "11", Text = "Sheet Street, Windsor" });

            ViewBag.Sites = sites;

            return View();
        }

        [HttpPost]
        public ActionResult Settings(ProviderUserViewModel providerUserView)
        {
            //Validate


            //Save

            return View();
        }

        public ActionResult VerifyEmail()
        {
            return View();
        }
    }
}