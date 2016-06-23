namespace ApprenticeshipScraper.CmdLine.Services
{
    using ApprenticeshipScraper.CmdLine.Models;

    public interface IUrlResolver
    {
        string Resolve(SiteEnum site);
    }
}