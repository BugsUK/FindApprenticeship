namespace ApprenticeshipScraper.CmdLine.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using ApprenticeshipScraper.CmdLine.Extensions;
    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Services;
    using ApprenticeshipScraper.CmdLine.Services.Logger;
    using ApprenticeshipScraper.CmdLine.Settings;

    using CsQuery;

    public class TraineeDetailsScraper : IStep
    {
        public const string UrlFormat = "/traineeship/{0}";

        private readonly ICreateDirectory directory;

        private readonly IGlobalSettings settings;

        private readonly IRetryWebRequests retry;

        private readonly IUrlResolver resolver;

        private int total;

        public TraineeDetailsScraper(IUrlResolver resolver, ICreateDirectory directory, IThreadSafeStepLogger logger, IGlobalSettings settings, IRetryWebRequests retry)
        {
            this.resolver = resolver;
            this.directory = directory;
            this.settings = settings;
            this.retry = retry;
            this.Logger = logger;
        }

        public IStepLogger Logger { get; }

        public void Run(ApplicationArguments arguments)
        {
            var folder = Path.Combine(arguments.Directory, arguments.Site.ToString());
            var detailsFolder = Path.Combine(folder, FolderNames.TraineeDetails);
            this.directory.CreateDirectoryIfMissing(detailsFolder);

            var filenames = this.LookAtApprenticeshipFiles(folder);
            Parallel.ForEach(
                filenames,
                new ParallelOptions { MaxDegreeOfParallelism = this.settings.MaxDegreeOfParallelism },
                filename =>
                    {
                        using (var cookieAwareWebClient = new CookieAwareWebClient())
                        {
                            var item = this.DownloadItem(arguments.Site, filename, cookieAwareWebClient);
                            var section = this.ParsePage(item);
                            SavePage(detailsFolder, section);
                        }
                    });
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            CQ dom = this.retry.RetryWeb(
                () => cookieAwareWebClient.DownloadString(new Uri(url)),
                x => this.Logger.Error($"Thread:{Thread.CurrentThread.ManagedThreadId:00} Id:{id} Elapsed:{stopwatch.ShortElapsed()}", x));
            stopwatch.Stop();
            this.Logger.Info($"Record:{(this.total++).ToString("00000")} Thread:{Thread.CurrentThread.ManagedThreadId:00} Id:{id} Elapsed:{stopwatch.ShortElapsed()}");
            return new { Id = id, Dom = dom };
        }

        private IEnumerable<string> LookAtApprenticeshipFiles(string folder)
        {
            return Directory.EnumerateFiles(Path.Combine(folder, FolderNames.TraineeResults));
        }
    }
}