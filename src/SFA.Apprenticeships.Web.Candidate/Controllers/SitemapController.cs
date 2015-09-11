namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Application.Vacancy.SiteMap;
    using Common.Constants;
    using Common.SiteMap;
    using Constants;

    [OutputCache(CacheProfile = CacheProfiles.SiteMap)]
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
                    new SiteMapItem(vacancy.ToUrl(), vacancy.LastModifiedDate, SiteMapChangeFrequency.Hourly));

            return new SiteMapResult(siteMapItems);
        }
    }
}