namespace ApprenticeshipScraper.CmdLine.Steps
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Services;

    using CsQuery;

    using Newtonsoft.Json;

    public sealed class ApprenticeshipResultsScraper : IStep
    {
        private readonly IUrlResolver resolver;

        private readonly ICreateDirectory directory;

        private const int PageSize = 100;

        private const int WaitBetweenRequestsMs = 20;

        private const string UrlFormat =
            "/apprenticeships?ApprenticeshipLevel=All&Hash=-2063231487&Latitude=53.68984&Longitude=-1.82732&Location=Elland%20%28West%20Yorkshire%29&LocationType=NonNational&PageNumber={0}&ResultsPerPage={1}&SearchAction=Sort&SearchField=All&SearchMode=Keyword&SortType=ClosingDate&WithinDistance=0";

        private ICollection<string> Vacancies { get; set; }
        public IStepLogger Logger { get; }

        public ApprenticeshipResultsScraper(IUrlResolver resolver, ICreateDirectory directory, IStepLogger logger)
        {
            this.resolver = resolver;
            this.directory = directory;
            this.Logger = logger;
        }


        public void Run(ApplicationArguments arguments)
        {
            var folder = Path.Combine(arguments.Directory, arguments.Site.ToString() , FolderNames.ApprenticeshipResults);
            var url = this.resolver.Resolve(arguments.Site);
            this.Vacancies = new List<string>();

            var rows = this.GetResultItems(url);
            var items = CovertToDynamic(rows);
            this.directory.CreateDirectoryIfMissing(folder);
            this.SaveToDisk(items, folder);

            this.SaveVacancyList(this.Vacancies, Path.Combine(arguments.Directory, arguments.Site.ToString()));
        }

        private void SaveVacancyList(ICollection<string> vacancies, string folder)
        {
            Logger.Info($"Saving vacancy list");
            var json = JsonConvert.SerializeObject(vacancies);
            File.WriteAllText(Path.Combine(folder, "vacancies.json"), json);
        }

        private void SaveToDisk(IEnumerable<dynamic> items, string folder)
        {
            foreach (var item in items)
            {
                this.Vacancies.Add(item.VacancyId);
                var filename = Path.Combine(folder, item.VacancyId + ".html");
                File.WriteAllText(filename, item.Html);
            }
        }
        
        private IEnumerable<CQ> GetResultItems(string baseUrl)
        {
            using (var cookieAwareWebClient = new CookieAwareWebClient())
            {
                for (int i = 1; i < 1000; i++) // NOTE: this page count will be a problem if the vacancy count goes above 100,000
                {
                    Logger.Info($"Downloading page {i}");
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

                yield return
                    new
                    {
                        Html = cq.RenderSelection(),
                        VacancyId = vacancyLink.Attr("data-vacancy-id")
                    };
            }
        }
    }
}