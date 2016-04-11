namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using SFA.Apprenticeships.Web.Recruit.Mediators.Home;

    public class HomeController : RecruitmentControllerBase
    {
        private readonly IHomeMediator _homeMediator;

        public HomeController(IHomeMediator homeMediator)            
        {
            _homeMediator = homeMediator;
        }

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
                
        public ActionResult ContactUs()
        {
            var userName = GetProviderUserName();
            var response = _homeMediator.GetContactMessageViewModel(userName);
            return View(response.ViewModel);
            
        }        
    }
}