namespace ApprenticeshipScraper.CmdLine.Steps
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Services;
    using ApprenticeshipScraper.CmdLine.Services.Logger;

    using CsQuery;

    public class ApprenticeshipDetailsScraperSingle : IStep
    {
        public const string UrlFormat = "/apprenticeship/{0}";

        private const int WaitBetweenRequestsMs = 20;

        private readonly ICreateDirectory directory;

        private readonly IUrlResolver resolver;

        public ApprenticeshipDetailsScraperSingle(IUrlResolver resolver, ICreateDirectory directory, IStepLogger logger)
        {
            this.resolver = resolver;
            this.directory = directory;
            this.Logger = logger;
        }

        public IStepLogger Logger { get; }

        public void Run(ApplicationArguments arguments)
        {
            var detailsFolder = this.directory.FindStepFolder(arguments, FolderNames.ApprenticeshipDetails);
            var resultsFolder = this.directory.FindStepFolder(arguments, FolderNames.ApprenticeshipResults);

            this.directory.CreateDirectoryIfMissing(detailsFolder);

            var filenames = this.LookAtApprenticeshipFiles(resultsFolder);
            var models = this.DownloadPages(filenames, arguments.Site);
            var sections = this.ParsePages(models);
            this.SavePages(sections, detailsFolder);
        }

        private static void SavePage(string folder, dynamic section)
        {
            File.WriteAllText(Path.Combine(folder, section.Id + ".html"), section.Html);
        }

        private dynamic ParsePage(dynamic model)
        {
            var dom = model.Dom["main"];
            return new { model.Id, Html = new CQ(dom).RenderSelection() };
        }

        private dynamic DownloadItem(SiteEnum site, string filename, CookieAwareWebClient cookieAwareWebClient)
        {
            var id = filename.Substring(filename.LastIndexOf("\\") + 1).Split('.').First();
            var url = this.resolver.Resolve(site) + string.Format(UrlFormat, id);
            CQ dom = cookieAwareWebClient.DownloadString(url);
            return new { Id = id, Dom = dom };
        }

        private IEnumerable<string> LookAtApprenticeshipFiles(string folder)
        {
            return Directory.EnumerateFiles(folder);
        }

        private IEnumerable<dynamic> DownloadPages(IEnumerable<string> filenames, SiteEnum site)
        {
            using (var cookieAwareWebClient = new CookieAwareWebClient())
            {
                foreach (var filename in filenames)
                {
                    yield return this.DownloadItem(site, filename, cookieAwareWebClient);
                    Thread.Sleep(WaitBetweenRequestsMs);
                }
            }
        }

        private IEnumerable<dynamic> ParsePages(IEnumerable<dynamic> models)
        {
            foreach (var model in models)
            {
                yield return ParsePage(model);
            }
        }

        private void SavePages(IEnumerable<dynamic> sections, string folder)
        {
            var total = 0;
            foreach (var section in sections)
            {
                SavePage(folder, section);
                total++;
                if (total % 10 == 0)
                {
                    this.Logger.Info($"{total} records saved");
                }
            }
        }
    }
}