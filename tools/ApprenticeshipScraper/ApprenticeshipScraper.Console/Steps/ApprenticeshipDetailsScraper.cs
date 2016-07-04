namespace ApprenticeshipScraper.CmdLine.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using ApprenticeshipScraper.CmdLine.Extensions;
    using ApprenticeshipScraper.CmdLine.Models;
    using ApprenticeshipScraper.CmdLine.Services;
    using ApprenticeshipScraper.CmdLine.Services.Logger;
    using ApprenticeshipScraper.CmdLine.Settings;

    using CsQuery;

    public class ApprenticeshipDetailsScraper : IStep
    {
        public const string UrlFormat = "/apprenticeship/{0}";

        private readonly ICreateDirectory directory;

        private readonly IUrlResolver resolver;

        private readonly IRetryWebRequests retry;

        private readonly IGlobalSettings settings;

        private int total;

        public ApprenticeshipDetailsScraper(
            IUrlResolver resolver,
            ICreateDirectory directory,
            IThreadSafeStepLogger logger,
            IGlobalSettings settings,
            IRetryWebRequests retry)
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
            var detailsFolder = this.directory.FindStepFolder(arguments, FolderNames.ApprenticeshipDetails);
            var resultsFolder = this.directory.FindStepFolder(arguments, FolderNames.ApprenticeshipResults);

            this.directory.CreateDirectoryIfMissing(detailsFolder);

            var filenames = this.LookAtApprenticeshipFiles(resultsFolder);
            Parallel.ForEach(
                filenames,
                new ParallelOptions { MaxDegreeOfParallelism = this.settings.MaxDegreeOfParallelism },
                filename =>
                    {
                        try
                        {
                            using (var cookieAwareWebClient = new CookieAwareWebClient())
                            {
                                var item = this.DownloadItem(arguments.Site, filename, cookieAwareWebClient);
                                var section = this.ParsePage(item);
                                SavePage(detailsFolder, section);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Record:{(this.total++).ToString("00000")} Thread:{Thread.CurrentThread.ManagedThreadId:00} Id:{FilenameToId(filename)} Unexpected Error", ex);
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
            var id = FilenameToId(filename);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var url = this.resolver.Resolve(site) + string.Format(UrlFormat, id);
            CQ dom = this.retry.RetryWeb(
                () => cookieAwareWebClient.DownloadString(new Uri(url)),
                x => this.Logger.Error($"Thread:{Thread.CurrentThread.ManagedThreadId:00} Id:{id} Elapsed:{stopwatch.ShortElapsed()}",x));
            stopwatch.Stop();
            var message = $"Record:{(this.total++).ToString("00000")} Thread:{Thread.CurrentThread.ManagedThreadId:00} Id:{id} Elapsed:{stopwatch.ShortElapsed()}";
            if (stopwatch.Elapsed.Seconds <= 5)
            {
                this.Logger.Info(message);
            }
            else
            {
                this.Logger.Warn(message);
            }
            return new { Id = id, Dom = dom };
        }

        private IEnumerable<string> LookAtApprenticeshipFiles(string resultsFolder)
        {
            return Directory.EnumerateFiles(resultsFolder);
        }

        private static string FilenameToId(string filename)
        {
            return filename.Substring(filename.LastIndexOf("\\") + 1).Split('.').First();
        }

    }
}