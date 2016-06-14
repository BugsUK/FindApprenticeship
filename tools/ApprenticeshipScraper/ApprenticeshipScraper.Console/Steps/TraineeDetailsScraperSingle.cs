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

    public class TraineeDetailsScraperSingle : IStep
    {
        public const string UrlFormat = "/traineeship/{0}";

        private const int WaitBetweenRequestsMs = 20;

        private readonly ICreateDirectory directory;

        private readonly IUrlResolver resolver;

        public TraineeDetailsScraperSingle(IUrlResolver resolver, ICreateDirectory directory, IStepLogger logger)
        {
            this.resolver = resolver;
            this.directory = directory;
            this.Logger = logger;
        }

        public IStepLogger Logger { get; }

        public void Run(ApplicationArguments arguments)
        {
            var folder = Path.Combine(arguments.Directory, arguments.Site.ToString());
            var detailsFolder = Path.Combine(folder, FolderNames.TraineeDetails);
            this.directory.CreateDirectoryIfMissing(detailsFolder);

            var filenames = this.LookAtApprenticeshipFiles(folder);
            var models = this.DownloadPages(filenames, arguments.Site);
            var sections = this.ParsePage(models);
            this.SavePages(sections, detailsFolder);
        }

        private void SavePages(IEnumerable<dynamic> sections, string folder)
        {
            var total = 0;
            foreach (var section in sections)
            {
                File.WriteAllText(Path.Combine(folder, section.Id + ".html"), section.Html);
                total++;
                if (total % 10 == 0)
                {
                    this.Logger.Info($"{total} records saved");
                }
            }
        }

        private IEnumerable<dynamic> ParsePage(IEnumerable<dynamic> models)
        {
            foreach (var model in models)
            {
                var dom = model.Dom["main"];
                yield return new { model.Id, Html = new CQ(dom).RenderSelection() };
            }
        }

        private IEnumerable<dynamic> DownloadPages(IEnumerable<string> filenames, SiteEnum site)
        {
            using (var cookieAwareWebClient = new CookieAwareWebClient())
            {
                foreach (var filename in filenames)
                {
                    var id = filename.Substring(filename.LastIndexOf("\\") + 1).Split('.').First();
                    var url = this.resolver.Resolve(site) + string.Format(UrlFormat, id);
                    CQ dom = cookieAwareWebClient.DownloadString(url);
                    yield return new { Id = id, Dom = dom };
                    Thread.Sleep(WaitBetweenRequestsMs);
                }
            }
        }

        private IEnumerable<string> LookAtApprenticeshipFiles(string folder)
        {
            return Directory.EnumerateFiles(Path.Combine(folder, FolderNames.TraineeResults));
        }
    }
}