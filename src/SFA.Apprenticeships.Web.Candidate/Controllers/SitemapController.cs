namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Http.Results;
    using System.Web.Mvc;
    using Common.SiteMap;

    // TODO: AG: US438: review caching policy.
    [OutputCache(Duration = 30, VaryByParam = "none")]
    public class SiteMapController : Controller
    {
        // TODO: AG: US438: consider adding a mediator here - but what value would be added?
        private readonly ISiteMapVacancyProvider _siteMapVacancyProvider;

        public SiteMapController(ISiteMapVacancyProvider siteMapVacancyProvider)
        {
            _siteMapVacancyProvider = siteMapVacancyProvider;
        }

        [Route("sitemap.xml")]
        public ActionResult Index()
        {
            var siteMapVacancies = _siteMapVacancyProvider.GetVacancies();

            if (siteMapVacancies == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpNotFoundResult();
        }
    }
}