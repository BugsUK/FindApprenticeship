namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Extensions;
    using Mediators.Provider;
    using Domain.Entities.Raa;
    using Application.Interfaces;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    [OwinSessionTimeout]
    public class ProviderController : RecruitmentControllerBase
    {
        private readonly IProviderMediator _providerMediator;

        public ProviderController(IProviderMediator providerMediator, IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
            _providerMediator = providerMediator;
        }

        [HttpGet]
        public ActionResult Sites()
        {
            var ukprn = User.GetUkprn();
            var response = _providerMediator.Sites(ukprn);
            var providerProfile = response.ViewModel;

            return View(providerProfile);
        }
    }
}