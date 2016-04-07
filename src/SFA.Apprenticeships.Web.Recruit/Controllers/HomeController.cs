namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using SFA.Apprenticeships.Web.Common.Attributes;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Mediators;
    using SFA.Apprenticeships.Web.Recruit.ViewModels.Home;

    public class HomeController : RecruitmentControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        [HttpGet]        
        public async Task<ActionResult> ContactUs()
        {
            return await Task.Run<ActionResult>(() => this.View());
        }

        [HttpPost]       
        public async Task<ActionResult> ContactUs(ContactMessageViewModel model)
        {
            return await Task.Run<ActionResult>(() => this.View());
        }

    

    }
}