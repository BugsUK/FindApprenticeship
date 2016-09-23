namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Web.Mvc;
    using Application.Interfaces;
    using Mediators.InformationRadiator;

    public class InformationRadiatorController : ManagementControllerBase
    {
        private readonly IInformationRadiatorMediator _informationRadiatorMediator;

        public InformationRadiatorController(IInformationRadiatorMediator informationRadiatorMediator, IConfigurationService configurationService, ILogService loggingService) : base(configurationService, loggingService)
        {
            _informationRadiatorMediator = informationRadiatorMediator;
        }

        public ActionResult Index()
        {
            var response = _informationRadiatorMediator.GetInformationRadiatorViewModel();

            return View(response.ViewModel);
        }
    }
}