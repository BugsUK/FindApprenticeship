namespace ApprenticeshipScraper.CmdLine.Steps
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using ApprenticeshipScraper.CmdLine.Extensions;
    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Services;
    using ApprenticeshipScraper.CmdLine.Services.Logger;
    using ApprenticeshipScraper.CmdLine.Settings;

    using CsQuery;

    public sealed class ApprenticeshipResultsScraper : IStep
    {
        private const string UrlFormat =
            "/apprenticeships?ApprenticeshipLevel=All&Hash=-2063231487&Latitude=53.68984&Longitude=-1.82732&Location=Elland%20%28West%20Yorkshire%29&LocationType=NonNational&PageNumber={0}&ResultsPerPage={1}&SearchAction=Sort&SearchField=All&SearchMode=Keyword&SortType=ClosingDate&WithinDistance=0";

        private readonly ICreateDirectory directory;

        private readonly IGlobalSettings settings;

        private readonly IUrlResolver resolver;

        public ApprenticeshipResultsScraper(IUrlResolver resolver, ICreateDirectory directory, IStepLogger logger, IGlobalSettings settings)
        {
            this.resolver = resolver;
            this.directory = directory;
            this.settings = settings;
            this.Logger = logger;
        }

        public IStepLogger Logger { get; }

        public void Run(ApplicationArguments arguments)
        {
            var folder = this.directory.FindStepFolder(arguments, FolderNames.ApprenticeshipResults);
            this.directory.CreateDirectoryIfMissing(folder);

            var url = this.resolver.Resolve(arguments.Site);
            var rows = this.GetResultItems(url);

            var items = CovertToDynamic(rows);
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
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    CQ dom = cookieAwareWebClient.DownloadString(baseUrl + string.Format(UrlFormat, i, this.settings.PageSize));
                    stopwatch.Stop();
                    Thread.Sleep(this.settings.WaitBetweenRequestsMs);
                    var message = $"Page:{i.ToString("000")} Elapsed:{stopwatch.ShortElapsed()}";
                    if (stopwatch.Elapsed.TotalSeconds <= 5)
                    {
                        this.Logger.Info(message);
                    }
                    else
                    {
                        this.Logger.Warn(message);
                    }
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