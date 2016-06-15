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

    internal class TraineeResultsScraper : IStep
    {
        private IUrlResolver resolver;

        private ICreateDirectory directory;

        private readonly IGlobalSettings settings;

        public const string UrlFormat =
            "/traineeships/search?Location=Elland%20%28West%20Yorkshire%29&LocationSearches=SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch.TraineeshipSearchViewModel%5B%5D&Longitude=-1.82732&Latitude=53.68984&Hash=-2063231487&WithinDistance=2000&SortType=Distance&PageNumber={0}&SearchAction=Sort&ResultsPerPage={1}";

        public TraineeResultsScraper(IUrlResolver resolver, ICreateDirectory directory, IStepLogger logger, IGlobalSettings settings)
        {
            this.resolver = resolver;
            this.directory = directory;
            this.settings = settings;
            this.Logger = logger;
        }

        public IStepLogger Logger { get; }

        public void Run(ApplicationArguments arguments)
        {
            var resultsFolder = this.directory.FindStepFolder(arguments, FolderNames.TraineeResults);
            this.directory.CreateDirectoryIfMissing(resultsFolder);

            var url = this.resolver.Resolve(arguments.Site);
            var rows = this.GetResultItems(url);
            var items = CovertToDynamic(rows);
            this.SaveToDisk(items, resultsFolder);
        }

        private IEnumerable<CQ> GetResultItems(string baseUrl)
        {
            using (var cookieAwareWebClient = new CookieAwareWebClient())
            {
                for (int i = 1; i < 1000; i++) // NOTE: this page count will be a problem if the vacancy count goes above 100,000
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    CQ dom = cookieAwareWebClient.DownloadString(baseUrl + string.Format(UrlFormat, i, this.settings.PageSize));
                    stopwatch.Stop();
                    var message = $"Page:{i} Elapsed: {stopwatch.ShortElapsed()}";
                    if (stopwatch.Elapsed.TotalSeconds <= 5)
                    {
                        this.Logger.Info(message);
                    }
                    else
                    {
                        this.Logger.Warn(message);
                    }

                    Thread.Sleep(this.settings.WaitBetweenRequestsMs);
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

        private void SaveToDisk(IEnumerable<dynamic> items, string folder)
        {
            foreach (var item in items)
            {
                var filename = Path.Combine(folder, item.VacancyId + ".html");
                File.WriteAllText(filename, item.Html);
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