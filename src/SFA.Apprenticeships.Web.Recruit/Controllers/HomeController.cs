namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;

    using FluentValidation.Mvc;

    using SFA.Apprenticeships.Web.Common.Mediators;
    using SFA.Apprenticeships.Web.Recruit.Mediators.Home;
    using SFA.Apprenticeships.Web.Recruit.ViewModels.Home;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactMessageViewModel viewModel)
        {
            var userName = GetProviderUserName();
            var response = _homeMediator.SendContactMessage(userName,viewModel);
            switch (response.Code)
            {
                case HomeMediatorCodes.SendContactMessage.ValidationError:
                    ModelState.Clear();
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(viewModel);
                case HomeMediatorCodes.SendContactMessage.SuccessfullySent:
                    ModelState.Clear();
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return View(response.ViewModel);
                case HomeMediatorCodes.SendContactMessage.Error:
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return View(response.ViewModel);
            }

            throw new InvalidMediatorCodeException(response.Code);
        }
    }
}