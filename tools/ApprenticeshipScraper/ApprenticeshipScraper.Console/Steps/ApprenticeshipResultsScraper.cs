namespace ApprenticeshipScraper.CmdLine.Steps
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Services;

    using CsQuery;

    public sealed class ApprenticeshipResultsScraper : IStep
    {
        private const int PageSize = 100;

        private const int WaitBetweenRequestsMs = 20;

        private const string UrlFormat =
            "/apprenticeships?ApprenticeshipLevel=All&Hash=-2063231487&Latitude=53.68984&Longitude=-1.82732&Location=Elland%20%28West%20Yorkshire%29&LocationType=NonNational&PageNumber={0}&ResultsPerPage={1}&SearchAction=Sort&SearchField=All&SearchMode=Keyword&SortType=ClosingDate&WithinDistance=0";

        private readonly ICreateDirectory directory;

        private readonly IUrlResolver resolver;

        public ApprenticeshipResultsScraper(IUrlResolver resolver, ICreateDirectory directory, IStepLogger logger)
        {
            this.resolver = resolver;
            this.directory = directory;
            this.Logger = logger;
        }

        public IStepLogger Logger { get; }

        public void Run(ApplicationArguments arguments)
        {
            var folder = Path.Combine(arguments.Directory, arguments.Site.ToString(), FolderNames.ApprenticeshipResults);
            var url = this.resolver.Resolve(arguments.Site);

            var rows = this.GetResultItems(url);
            var items = CovertToDynamic(rows);
            this.directory.CreateDirectoryIfMissing(folder);
            this.SaveToDisk(items, folder);
        }

        private void SaveToDisk(IEnumerable<dynamic> items, string folder)
        {
            foreach (var item in items)
            {
                var filename = Path.Combine(folder, item.VacancyId + ".html");
                File.WriteAllText(filename, item.Html);
            }
        }

        private IEnumerable<CQ> GetResultItems(string baseUrl)
        {
            using (var cookieAwareWebClient = new CookieAwareWebClient())
            {
                for (var i = 1; i < 1000; i++)
                    // NOTE: this page count will be a problem if the vacancy count goes above 100,000
                {
                    this.Logger.Info($"Downloading page {i}");
                    CQ dom = cookieAwareWebClient.DownloadString(baseUrl + string.Format(UrlFormat, i, PageSize));
                    Thread.Sleep(WaitBetweenRequestsMs);
                    var domObjects = dom[".search-results__item"];
                    if (domObjects.Length == 0)
                    {
                        break;
                    }

                    foreach (var item in domObjects)
                    {
                        yield return new CQ(item);
                    }
                }
            }
        }

        private static IEnumerable<dynamic> CovertToDynamic(IEnumerable<CQ> items)
        {
            foreach (var cq in items)
            {
                var vacancyLink = cq.Find(".vacancy-link");

                yield return new { Html = cq.RenderSelection(), VacancyId = vacancyLink.Attr("data-vacancy-id") };
            }
        }
    }
}