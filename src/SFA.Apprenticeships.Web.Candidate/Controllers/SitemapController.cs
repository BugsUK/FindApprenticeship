namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Vacancy.SiteMap;
    using Common.SiteMap;

    // TODO: AG: US438: review page caching policy.
    [OutputCache(Duration = 30, VaryByParam = "none")]
    public class SiteMapController : Controller
    {
        private readonly ISiteMapVacancyProvider _siteMapVacancyProvider;

        public SiteMapController(ISiteMapVacancyProvider siteMapVacancyProvider)
        {
            _siteMapVacancyProvider = siteMapVacancyProvider;
        }

        [Route("sitemap.xml")]
        public ActionResult Index()
        {
            var vacancies = _siteMapVacancyProvider.GetVacancies();

            if (vacancies == null)
            {
                return new HttpNotFoundResult();
            }
 
            var siteMapItems = vacancies
                .Select(vacancy =>
                    new SiteMapItem(vacancy.ToUrl(), DateTime.UtcNow, SiteMapChangeFrequency.Hourly));

            return new SiteMapResult(siteMapItems);
        }
    }
}