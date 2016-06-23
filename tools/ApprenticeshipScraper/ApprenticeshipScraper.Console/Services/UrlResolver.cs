namespace ApprenticeshipScraper.CmdLine.Services
{
    using ApprenticeshipScraper.CmdLine.Models;

    internal class UrlResolver : IUrlResolver
    {
        public string Resolve(SiteEnum site)
        {
            switch (site)
            {
                case SiteEnum.Pre:
                    return "https://pre.findapprenticeship.service.gov.uk";
                case SiteEnum.Prod:
                    return "https://www.findapprenticeship.service.gov.uk";
            }

            return null;
        }
    }
}