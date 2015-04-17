namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.SiteMap;

    // TODO: AG: US438: review caching policy.
    [OutputCache(Duration = 30, VaryByParam = "none")]
    public class SiteMapController : Controller
    {
        [Route("sitemap.xml")]
        public ActionResult Index()
        {
            var siteMapItems = new[]
            {
                new SiteMapItem("https://local.findapprenticeship.service.gov.uk/apprenticeship/443939", DateTime.Today, SiteMapChangeFrequency.Hourly),
                new SiteMapItem("https://local.findapprenticeship.service.gov.uk/apprenticeship/431501", DateTime.Today, SiteMapChangeFrequency.Hourly),
                new SiteMapItem("https://local.findapprenticeship.service.gov.uk/traineeship/437787", DateTime.Today, SiteMapChangeFrequency.Hourly)
            };

            return new SiteMapResult(siteMapItems);
        }
    }
}